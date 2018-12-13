using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Data.Models.Master
{
    [Table("States", Schema = Constants.DatabaseSchemas.MasterDataSchemaName)]
    public class State : BaseModel
    {
        [Required]
        [MaxLength(400)]
        public string Name { get; set; }

        #region Foreign keys

        /// <summary>
        /// The Id corresponding to the country for this state.
        /// </summary>
        [Index(IsClustered = true)]
        public long CountryId { get; set; }

        [ForeignKey(nameof(CountryId))]
        public virtual Country Country { get; set; }

        #endregion
    }
}
