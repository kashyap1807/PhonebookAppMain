using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace PhonebookAPI.Dtos
{
    public class GetAllContactByMonthDto
    {
        public int contactId { get; set; }


        [Required(ErrorMessage = "First name is required")]
        [DisplayName("First Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required")]
        [DisplayName("Last Name")]
        public string LastName { get; set; }

        public string Email { get; set; }

        public string Company { get; set; }


        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?\d{3}\)?[-.\s]?\d{3}[-.\s]?\d{4}$", ErrorMessage = "Invalid contact number.")]
        [Required(ErrorMessage = "Contact Number is required")]
        [DisplayName("Contact Number")]
        public string ContactNumber { get; set; }


        public string? FileName { get; set; } = string.Empty;
        public int CountryId { get; set; }
        public int StateId { get; set; }
        public string Gender { get; set; }
        public bool IsFavourite { get; set; }
        public byte[]? Image { get; set; }
        public DateTime BirthDate { get; set; }
        public string CountryName { get; set; }
        public string StateName { get; set; }
    }
}
