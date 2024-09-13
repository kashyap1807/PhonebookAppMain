﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace PhonebookAPI.Models
{
    [ExcludeFromCodeCoverage]
    public partial class Contact
    {
        public int ContactId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Company { get; set; }
        public string ContactNumber { get; set; }
        public string FileName { get; set; }
        public int CountryId { get; set; }
        public int StateId { get; set; }
        public string Gender { get; set; }
        public bool IsFavourite { get; set; }
        public byte[] Image { get; set; }

        public DateTime BirthDate { get; set; }

        public virtual Country Country { get; set; }
        public virtual State State { get; set; }
    }
}