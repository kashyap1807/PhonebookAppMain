﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace PhonebookAPI.Models
{
    [ExcludeFromCodeCoverage]
    public partial class State
    {
        public State()
        {
            Contacts = new HashSet<Contact>();
        }

        public int StateId { get; set; }
        public string StateName { get; set; }
        public int CountryId { get; set; }

        public virtual Country Country { get; set; }
        public virtual ICollection<Contact> Contacts { get; set; }
    }
}