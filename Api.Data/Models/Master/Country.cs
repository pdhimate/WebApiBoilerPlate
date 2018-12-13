using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Data.Models.Master
{
    [Table("Countries", Schema = Constants.DatabaseSchemas.MasterDataSchemaName)]
    public class Country : BaseModel
    {
        [Index(IsClustered = true, IsUnique = true)]
        [Required]
        [MaxLength(400)]
        public string Name { get; set; }
    }

}
