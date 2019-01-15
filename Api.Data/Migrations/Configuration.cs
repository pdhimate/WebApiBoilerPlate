namespace Api.Data.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Models;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Api.Data.Models.Security;
    using Models.Master;
    using System.Configuration;
    using Security;
    using Api.Data.CosmosDb.Models;
    using Api.Data.CosmosDb.Repositories;
    using Api.Data.CosmosDb.Helpers;
    using System.Threading.Tasks;

    internal sealed class Configuration : DbMigrationsConfiguration<AppDatabaseContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = false;
        }

        protected override void Seed(AppDatabaseContext context)
        {
            // SQL server seeding
            InsertMandatoryData(context);
            InsertAdminUser(context);

            // Cosmos db seeding
            InsertAdminUserPost(context);
        }

        #region Private helpers

        #region Addresses master data

        private readonly IReadOnlyCollection<string> _countries = new List<string>
        {
            "India",
            "Canada",
            "Unites States of America",
            "Australia"
        };

        private readonly IReadOnlyCollection<string> _indiaStates = new List<string>
        {
            "Maharashtra",
            "Goa",
            "Karnataka",
        };

        private readonly IReadOnlyCollection<string> _indiaMaharashtraCities = new List<string>
        {
            "Pune",
            "Nasik",
            "Mumbai",
        };

        #endregion

        /// <summary>
        /// Inserts master data and other static data required by the api/app to be functional.
        /// </summary>
        /// <param name="context"></param>
        private void InsertMandatoryData(AppDatabaseContext context)
        {
            #region UserRoles

            var userRoles = new List<AppRole>();
            foreach (var role in AppRoles.GetAll())
            {
                userRoles.Add(new AppRole { Name = role });
            };

            foreach (var role in userRoles)
            {
                context.Roles.AddOrUpdate(r => r.Name, role);
                context.SaveChanges();
            }

            #endregion

            #region Countries

            foreach (var countryName in _countries)
            {
                var country = new Country { Name = countryName };
                context.Countries.AddOrUpdate(c => c.Name, country);
                context.SaveChanges();
            }

            #endregion

            #region India - states

            var india = context.Countries.FirstOrDefault(c => c.Name == "India");
            foreach (var stateName in _indiaStates)
            {
                var state = new State { Name = stateName, CountryId = india.Id };
                context.States.AddOrUpdate(c => c.Name, state);
                context.SaveChanges();
            }

            #endregion

            #region India - Maharashtra - cities

            var indiaMahrashtra = context.States.FirstOrDefault(c => c.Name == "Maharashtra");
            foreach (var cityName in _indiaMaharashtraCities)
            {
                var city = new City { Name = cityName, StateId = indiaMahrashtra.Id };
                context.Cities.AddOrUpdate(c => c.Name, city);
                context.SaveChanges();
            }

            #endregion

            #region Clients

            var activeClient = new Client();
            activeClient.Id = "AngularWebApp";
            activeClient.AllowedOrigin = "*"; // TODO: replace with the actual client url. 
            activeClient.ClientType = ClientType.JavaScript;
            activeClient.IsActive = true;
            activeClient.Name = "Website";
            activeClient.RefreshTokenLifeTime = 365 * 24 * 60; // 1 year
            activeClient.Secret = Encryptor.GetHash(Guid.NewGuid().ToString());

            // For integration tests only
            var inactiveClient = new Client();
            inactiveClient.Id = "Tests-InactiveWebApp";
            inactiveClient.AllowedOrigin = "*";
            inactiveClient.ClientType = ClientType.JavaScript;
            inactiveClient.IsActive = false;
            inactiveClient.Name = " Website inactive";
            inactiveClient.RefreshTokenLifeTime = 365 * 24 * 60; // 1 year
            inactiveClient.Secret = Encryptor.GetHash(Guid.NewGuid().ToString());

            var nativeActiveClient = new Client();
            nativeActiveClient.Id = "AndroidApp";
            nativeActiveClient.AllowedOrigin = "*";
            nativeActiveClient.ClientType = ClientType.NativeConfidential;
            nativeActiveClient.IsActive = true;
            nativeActiveClient.Name = "Android App";
            nativeActiveClient.RefreshTokenLifeTime = 180 * 24 * 60; //  6 months
            nativeActiveClient.Secret = Encryptor.GetHash(Guid.NewGuid().ToString());

            context.Clients.AddOrUpdate(c => c.Id, activeClient);
            context.Clients.AddOrUpdate(c => c.Id, inactiveClient);
            context.Clients.AddOrUpdate(c => c.Id, nativeActiveClient);

            context.SaveChanges();

            #endregion
        }

        private readonly string AdminEmail = "YourAdminUserEmail@SomeEmailProvider.com";
        private void InsertAdminUser(AppDatabaseContext context)
        {
            var userStore = new UserStore<AppUser, AppRole, long, ExternalUserLogin, AppUserAppRoleMapping, AppUserClaim>(context);
            var userManager = new UserManager<AppUser, long>(userStore);

            // Add user
            var user = new AppUser
            {
                UserName = Data.Constants.DefaultUsers.AdminUserName,
                Email = AdminEmail,
                EmailConfirmed = true,
                FirstName = "Administrator",
                CreatedOnUtc = DateTime.UtcNow,
            };
            userManager.Create(user, "abcd12#$");

            // Assign Admin role to the user
            var adminUser = userManager.FindByEmail(user.Email);
            var adminRole = context.Roles.FirstOrDefault(r => r.Name == AppRoles.AdminRole);
            var usersRolesMap = new AppUserAppRoleMapping
            {
                UserId = adminUser.Id,
                RoleId = adminRole.Id
            };
            if (!context.AppUsersRolesMap.Any(urm => urm.RoleId == usersRolesMap.RoleId
                                                  && urm.UserId == usersRolesMap.UserId))
            {
                context.AppUsersRolesMap.Add(usersRolesMap);
                context.SaveChanges();
            }
        }

        private readonly string AdminUserFirstPostGUID = "DA5B3550-659A-4DA2-A5E1-27353F52AA64";
        private void InsertAdminUserPost(AppDatabaseContext context)
        {
            var adminUser = context.Users.First(u => u.Email == AdminEmail);

            using (var client = CosmosDbHelper.GetDocumentClient())
            {
                var postRepo = new TextPostRepo(client);
                var existingPost = new TextPost
                {
                    CreatedByUserId = adminUser.Id,
                    CreatedByUserName = adminUser.FirstName + adminUser.LastName,
                    Note = "This is the first text post, posted on behalf of the admin user while seeding the database!",
                    Id = AdminUserFirstPostGUID
                };
                postRepo.InsertOrUpdateAsync(existingPost).Wait();
            }
        }

        #endregion
    }
}
