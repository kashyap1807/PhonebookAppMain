using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace PhonebookAPI.Dtos
{
    [ExcludeFromCodeCoverage]
    public class LoginDto
    {
        [Required(ErrorMessage = "Username is required.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
