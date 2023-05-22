using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models
{
    [Table("owner")]
    public class Owner
    {
        public Guid OwnerId { get; set; }

        [Required(ErrorMessage = "Le nom est obligatoire")]
        [StringLength(60, ErrorMessage = "Le nom ne doit pas dépasser 60 caractères")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "La date de naissance est obligatoire")]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "L'adresse est obligatoire")]
        [StringLength(100, ErrorMessage = "L'adresse ne doit pas dépasser 100 caractères")]
        public string? Address { get; set; }

        public ICollection<Account>? Accounts { get; set; }
    }
}
