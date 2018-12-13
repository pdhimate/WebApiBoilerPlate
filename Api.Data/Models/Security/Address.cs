using Api.Data.Models.Master;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api.Data.Models.Security
{
    [Table("Addresses", Schema = Constants.DatabaseSchemas.SecurityDataSchemaName)]
    public class Address : BaseModel
    {
        public string Street { get; set; }

        public string Landmark { get; set; }

        /// <summary>
        /// Building or society name
        /// </summary>
        public string BuildingName { get; set; }

        /// <summary>
        /// The name of the locality or area
        /// </summary>
        public string AreaName { get; set; }

        [Required]
        [MaxLength(12)]
        public string ZipCode { get; set; }

        public decimal? Latitude { get; set; }

        public decimal? Longitude { get; set; }

        /// <summary>
        /// Stores a Google or Bing Maps url that points to this Address.
        /// Might contain link for any other maps provider.
        /// </summary>
        public string MapLink { get; set; }

        /// <summary>
        /// As a good practise, always try to store the entire address as text in this field.
        /// </summary>
        public string AddressString { get; set; }

        #region Foreign keys

        /// <summary>
        /// The Id corresponding to the city for this address.
        /// </summary>
        public long CityId { get; set; }

        [ForeignKey(nameof(CityId))]
        public virtual City City { get; private set; }

        #endregion
    }
}
