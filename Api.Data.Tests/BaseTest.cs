using Api.TestHelper;
using NUnit.Framework;
using System;
using System.Configuration;
using System.Data.Entity;
using System.Linq;

namespace Api.Data.IntegrationTests
{
    /// <summary>
    /// Provides for a base class containing the test setup and cleanup methods, which are common to all the integration tests.
    /// </summary>
    public abstract class BaseTest
    {
        /// <summary>
        /// Creates the test database. Inserts common test data in it.
        /// Currently inserts <see cref="Models.Address"/>
        /// </summary>
        [OneTimeSetUp]
        public virtual void SetUp()
        {
            // Creates a new database
            using (var dbContext = new AppDatabaseContext())
            {
                AppDatabaseContext.Create();
            };

            // Add common test data to the database
            AddTestCountries();
            AddTestStates();
            AddTestCities();
            AddTestAddresses();
        }

        [OneTimeTearDown]
        public virtual void CleanUp()
        {
            // Delete the database
            using (var dbContext = new AppDatabaseContext())
            {
                dbContext.Database.Delete();
            };
        }

        #region Private methods to add common test data

        private void AddTestAddresses()
        {
            var addresses = DummyDataGenerator.GetAddresses(10);

            using (var appDbcontext = new AppDatabaseContext())
            {
                var countries = appDbcontext.Countries.ToList();
                var states = appDbcontext.States.ToList();
                var cities = appDbcontext.Cities.ToList();
                foreach (var address in addresses)
                {
                    var random = new Random();
                    var country = countries[random.Next(0, countries.Count - 1)];
                    var state = states[random.Next(0, states.Count - 1)];
                    var city = cities[random.Next(0, cities.Count - 1)];
                    address.CityId = city.Id;
                    appDbcontext.Addresses.Add(address);
                }
                appDbcontext.SaveChanges();
            }
        }

        private void AddTestCountries()
        {
            var countries = DummyDataGenerator.GetCountries(10);
            using (var appDbcontext = new AppDatabaseContext())
            {
                foreach (var country in countries)
                {
                    appDbcontext.Countries.Add(country);
                }
                appDbcontext.SaveChanges();
            }
        }

        private void AddTestStates()
        {
            var states = DummyDataGenerator.GetStates(10);
            using (var appDbcontext = new AppDatabaseContext())
            {
                var country = appDbcontext.Countries.First();
                foreach (var state in states)
                {
                    state.CountryId = country.Id;
                    appDbcontext.States.Add(state);
                }
                appDbcontext.SaveChanges();
            }
        }

        private void AddTestCities()
        {
            var cities = DummyDataGenerator.GetCities(10);
            using (var appDbcontext = new AppDatabaseContext())
            {
                var state = appDbcontext.States.First();
                foreach (var city in cities)
                {
                    city.StateId = state.Id;
                    appDbcontext.Cities.Add(city);
                }
                appDbcontext.SaveChanges();
            }
        }


        #endregion
    }
}
