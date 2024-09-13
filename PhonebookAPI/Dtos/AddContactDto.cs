﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using PhonebookAPI.Models;
using System.Diagnostics.CodeAnalysis;

namespace PhonebookAPI.Dtos
{
    [ExcludeFromCodeCoverage]
    public class AddContactDto
    {
        

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

        [DisplayName("Image")]
        public IFormFile? Image { get; set; } 

        public int CountryId { get; set; }
        public int StateId { get; set; }
        public string Gender { get; set; }
        public bool IsFavourite { get; set; }

        public DateTime BirthDate { get; set; }

        //public virtual Country Country { get; set; }
        //public virtual State State { get; set; }

        //public IFormFile File { get; set; }
    }
}