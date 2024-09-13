using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace ClientPhonebook.ViewModel
{
    public class UserViewModel
    {
        public int userId {  get; set; }

        [Required(ErrorMessage = "First name is require")]
        [StringLength(15)]
        [DisplayName("First Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is require")]
        [StringLength(15)]
        [DisplayName("Last Name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Login id is require")]
        [StringLength(15)]
        [DisplayName("Login Id")]
        public string LoginId { get; set; }

        [Required(ErrorMessage = "Email address is require")]
        [StringLength(50)]
        [EmailAddress]
        [RegularExpression(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$", ErrorMessage = "Invalid email format.")]
        [DisplayName("Email Address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Phone number is require")]
        [StringLength(15)]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?\d{3}\)?[-.\s]?\d{3}[-.\s]?\d{4}$", ErrorMessage = "Invalid contact number.")]
        [DisplayName("Contact Number")]
        public string ContactNumber { get; set; }
    }
}
