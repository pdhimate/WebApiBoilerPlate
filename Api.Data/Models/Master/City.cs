using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Data.Models.Master
{
    [Table("Cities", Schema = Constants.DatabaseSchemas.MasterDataSchemaName)]
    public class City : BaseModel
    {
        [Required]
        [MaxLength(400)]
        public string Name { get; set; }

        #region Foreign keys

        /// <summary>
        /// The Id corresponding to the country for this city.
        /// </summary>
        [Index(IsClustered = true)]
        public long StateId { get; set; }

        [ForeignKey(nameof(StateId))]
        public virtual State State { get; set; }

        #endregion
    }
}
