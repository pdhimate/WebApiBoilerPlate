using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api.Data.Models
{
    /// <summary>
    /// Provides a basic implementation of a database enity.
    /// </summary>
    public abstract class BaseModel : IBaseModel<long>
    {
        /// <summary>
        /// Primary key
        /// </summary>
        [Key]
        [Column(TypeName = "BigInt")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
    }

    public interface IBaseModel<T>
    {
        /// <summary>
        /// Unique Identifier, generally used as primary key.
        /// </summary>
        T Id { get; set; }
    }
}
