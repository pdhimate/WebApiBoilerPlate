namespace Api.BusinessEntities
{
    public class AddressDto
    {
        public string Street { get; set; }

        public string Landmark { get; set; }

        public string BuildingName { get; set; }

        public string AreaName { get; set; }

        public string ZipCode { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string Country { get; set; }

        public decimal Latitude { get; set; }

        public decimal Longitude { get; set; }

        public string MapUrlLink { get; set; }

        public string AddressString { get; set; }
    }
}
