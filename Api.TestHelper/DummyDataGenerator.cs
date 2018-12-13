using Api.Data.Models;
using Api.Data.Models.Master;
using Api.Data.Models.Security;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Api.TestHelper
{
    /// <summary>
    /// Generates dummy data using the <see cref="Api.Data.Models"/>.
    /// </summary>
    public static class DummyDataGenerator
    {
        internal const string TestStringPrefix = "Test_";

        public static readonly Random Random = new Random(DateTime.UtcNow.Millisecond);

        /// <summary>
        /// Generated the specified number of addresses
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public static List<Address> GetAddresses(int count)
        {
            var addresses = new List<Address>();
            var countries = GetCountries(count);
            var states = GetStates(count);
            var cities = GetCities(count);

            for (int i = 1; i <= count; i++)
            {
                var address = new Address();
                ReflectionUtility.SetPropertyName(address, () => address.AddressString, TestStringPrefix + i, "Flat: " + i + ", City" + cities[i - 1].Name + ", Pin: 4000" + i);
                ReflectionUtility.SetPropertyName(address, () => address.AreaName, TestStringPrefix, i.ToString());
                ReflectionUtility.SetPropertyName(address, () => address.BuildingName, TestStringPrefix, i.ToString());
                address.CityId = cities[i - 1].Id;
                address.Id = i;
                ReflectionUtility.SetPropertyName(address, () => address.Landmark, TestStringPrefix, i.ToString());
                address.Latitude = (decimal)Random.NextDouble();
                address.Longitude = (decimal)Random.NextDouble();
                ReflectionUtility.SetPropertyName(address, () => address.MapLink, "http://map.google.com/", i.ToString());
                ReflectionUtility.SetPropertyName(address, () => address.Street, TestStringPrefix, i.ToString());
                address.ZipCode = Random.Next(1000, 9999).ToString() + i.ToString();

                addresses.Add(address);
            }

            return addresses;
        }

        public static List<Country> GetCountries(int count)
        {
            var countries = new List<Country>();

            for (int i = 1; i <= count; i++)
            {
                var country = new Country();
                ReflectionUtility.SetPropertyName(country, () => country.Name, TestStringPrefix + "Country", i.ToString());
                countries.Add(country);
            }

            return countries;
        }

        public static List<State> GetStates(int count)
        {
            var countries = GetCountries(count);
            var states = new List<State>();

            for (int i = 1; i <= count; i++)
            {
                var state = new State();
                state.Name = TestStringPrefix + "State" + nameof(State.Name) + i.ToString();
                state.CountryId = countries[i - 1].Id;
                states.Add(state);
            }

            return states;
        }

        public static List<City> GetCities(int count)
        {
            var states = GetStates(count);
            var cities = new List<City>();

            for (int i = 1; i <= count; i++)
            {
                var city = new City();
                city.Name = TestStringPrefix + "City" + nameof(City.Name) + i.ToString();
                city.StateId = states[i - 1].Id;
                cities.Add(city);
            }

            return cities;
        }


        /// <summary>
        /// Generates the specified number of users.
        /// </summary>
        /// <param name="count">The number of users to generate.</param>
        /// <param name="addresses">The addresses to be associated as foriegn keys to the users being generated. If any.</param>
        /// <returns></returns>
        public static List<AppUser> GetUsers(int count, List<Address> addresses)
        {
            var users = new List<AppUser>();

            for (int i = 1; i <= count; i++)
            {
                var user = new AppUser();

                if (addresses != null)
                {
                    var randomAddressIndex = Random.Next(addresses.Count * -1, addresses.Count - 1);
                    if (randomAddressIndex >= 0)
                    {
                        user.DefaultAddressId = addresses.ElementAt(randomAddressIndex).Id;
                    }
                }

                user.CreatedOnUtc = DateTime.UtcNow;
                user.EmailConfirmed = (i / Random.Next(i, count)) == 0;
                ReflectionUtility.SetPropertyName(user, () => user.FirstName, TestStringPrefix, i.ToString());
                user.Id = i;
                ReflectionUtility.SetPropertyName(user, () => user.LastName, TestStringPrefix, i.ToString());
                ReflectionUtility.SetPropertyName(user, () => user.MiddleName, TestStringPrefix, i.ToString());
                user.PasswordHash = Guid.NewGuid().ToString();
                user.PhoneNumber = Random.Next(100000000, 999999999).ToString() + i;
                ReflectionUtility.SetPropertyName(user, () => user.UserName, TestStringPrefix, i.ToString());
                ReflectionUtility.SetPropertyName(user, () => user.Email, TestStringPrefix, i.ToString() + "@bitartist.com");

                users.Add(user);
            }

            return users;
        }

    }
}
