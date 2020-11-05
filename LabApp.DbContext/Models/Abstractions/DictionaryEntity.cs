using System.ComponentModel.DataAnnotations;

namespace LabApp.Server.Data.Models.Abstractions
{
    public abstract class DictionaryEntity
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Value { get; set; }
    }
}