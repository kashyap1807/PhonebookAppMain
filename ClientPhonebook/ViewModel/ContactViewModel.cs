using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Diagnostics.Metrics;


namespace ClientPhonebook.ViewModel
{
    public class ContactViewModel
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
        public byte[] ImageBytes { get; set; }

        public int CountryId { get; set; }
        public int StateId { get; set; }
        public string Gender { get; set; }
        public bool IsFavourite { get; set; }
        public DateTime BirthDate { get; set; }

        public virtual CountryViewModel Country { get; set; }
        public virtual StateViewModel State { get; set; }
    }
}
