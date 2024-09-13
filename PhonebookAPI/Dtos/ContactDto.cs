using PhonebookAPI.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace PhonebookAPI.Dtos
{
    [ExcludeFromCodeCoverage]
    public class ContactDto
    {
        public int contactId { get; set; }

        [Required]
        [StringLength(15)]
        public string FirstName { get; set; }
        [Required]
        [StringLength(15)]
        public string LastName { get; set; }

        [Required]
        [StringLength(50)]
        public string Email { get; set; }

        [Required]
        [StringLength(50)]
        public string Company { get; set; }

        [Required]
        [StringLength(15)]
        public string ContactNumber { get; set; }

        public string FileName { get; set; }

        public int CountryId { get; set; }
        public int StateId { get; set; }
        public string Gender { get; set; }
        public bool IsFavourite { get; set; }

        public byte[] ImageBytes { get; set; }

        public DateTime BirthDate { get; set; }

        public virtual Country Country { get; set; }
        public virtual State State { get; set; }
    }
}
