using System;
using System.ComponentModel.DataAnnotations;

namespace Api.Data.Models.Meta
{
    public abstract class BaseEntry
    {
        public BaseEntry()
        {
            Id = Guid.NewGuid().ToString();
        }

        [Key]
        public string Id { get; private set; }
    }
}
