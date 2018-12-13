using Api.Data.Migrations;
using Api.Data.Models;
using Api.Data.Models.Master;
using Api.Data.Models.Meta;
using Api.Data.Models.Security;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Text;

namespace Api.Data
{
    public class AppDatabaseContext : IdentityDbContext<AppUser, AppRole, long, ExternalUserLogin, AppUserAppRoleMapping, AppUserClaim>
    {
        public AppDatabaseContext()
            : base(Constants.WebConfig.DatabaseConnectionKey)
        {
        }

        #region Overriden methods

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Changes the default schema from dbo to Core, for all the models
            modelBuilder.HasDefaultSchema(Constants.DatabaseSchemas.CoreDataSchemaName);

            // For the models derived from Asp Identity models,
            // we need to configure table names, table schemas here. 
            // The TableAttribute does not work in the respective classes.
            modelBuilder.Entity<AppUserAppRoleMapping>().ToTable("AppUserAppRoleMappings", Constants.DatabaseSchemas.SecurityDataSchemaName);
            modelBuilder.Entity<ExternalUserLogin>().ToTable("ExternalUserLogins", Constants.DatabaseSchemas.SecurityDataSchemaName);
            modelBuilder.Entity<AppUserClaim>().ToTable("AppUserClaims", Constants.DatabaseSchemas.SecurityDataSchemaName);
            modelBuilder.Entity<AppRole>().ToTable("AppRoles", Constants.DatabaseSchemas.SecurityDataSchemaName);
            modelBuilder.Entity<AppUser>().ToTable("AppUsers", Constants.DatabaseSchemas.SecurityDataSchemaName);
        }

        /// <summary>
        /// Saves changes made in this context to the underlying database.
        /// Also serves as a wrapper for SaveChanges adding the DB Entity Validation Messages to the generated exception
        /// </summary>
        /// <returns>Number of objects changed in the database</returns>
        public override int SaveChanges()
        {
            try
            {
                return base.SaveChanges();
            }
            catch (DbEntityValidationException dbException)
            {
                var stringbuilder = new StringBuilder();

                foreach (var failure in dbException.EntityValidationErrors)
                {
                    stringbuilder.AppendFormat("{0} failed validation\n", failure.Entry.Entity.GetType());
                    foreach (var error in failure.ValidationErrors)
                    {
                        stringbuilder.AppendFormat("- {0} : {1}", error.PropertyName, error.ErrorMessage);
                        stringbuilder.AppendLine();
                    }
                }

                // Add the original exception as the innerException
                throw new DbEntityValidationException(
                    "Entity Validation Failed - errors follow:\n" + stringbuilder.ToString(),
                    dbException
                );
            }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Creates a new instance od the database context.
        /// </summary>
        /// <returns></returns>
        public static AppDatabaseContext Create()
        {
            SetDatabaseCreationPolicies();
            return new AppDatabaseContext();
        }

        #endregion

        #region Private methods

        private static void SetDatabaseCreationPolicies()
        {
            // Creates a database if it does not exist
            Database.SetInitializer(new CreateDatabaseIfNotExists<AppDatabaseContext>());

            // Ensures that migrations have run and seed method is run
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<AppDatabaseContext, Configuration>());
        }

        #endregion

        #region Tables

        #region CoreDataSchema tables


        #endregion CoreDataSchema tables

        #region SecurityDataSchema tables
        public IDbSet<Address> Addresses { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public IDbSet<AppUserAppRoleMapping> AppUsersRolesMap { get; set; }

        #endregion

        #region MetaDataSchema tables

        public IDbSet<LogEntry> LogEntries { get; set; }

        #endregion

        #region Master schema tables

        public IDbSet<Country> Countries { get; set; }
        public IDbSet<State> States { get; set; }
        public IDbSet<City> Cities { get; set; }

        #endregion

        #endregion Tables
    }
}
