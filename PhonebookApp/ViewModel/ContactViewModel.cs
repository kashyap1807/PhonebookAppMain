using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PhonebookApp.ViewModel
{
    public class ContactViewModel
    {
        [DisplayName("ContactId")]
        public int contactId { get; set; }

        [Required(ErrorMessage = "First name is required.")]
        [StringLength(15)]
        [DisplayName("First Name")]
        public string FirstName { get; set; }
        
        [Required(ErrorMessage = "Last name is required.")]
        [StringLength(15)]
        [DisplayName("Last Name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [StringLength(50)]
        [EmailAddress]
        [RegularExpression(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$", ErrorMessage = "Invalid email format.")]
        [DisplayName("Email Address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Company name is required.")]
        [StringLength(50)]
        [DisplayName("Company Name")]
        public string Company { get; set; }

        [Required(ErrorMessage = "Contact number is required.")]
        [StringLength(15)]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?\d{3}\)?[-.\s]?\d{3}[-.\s]?\d{4}$", ErrorMessage = "Invalid contact number.")]
        [DisplayName("Contact Number")]
        public string ContactNumber { get; set; }

        public string FileName { get; set; } = string.Empty;

        [DisplayName("File")]
        //[Required(ErrorMessage = "File is required.")]
        public IFormFile File { get; set; }
    }
}
