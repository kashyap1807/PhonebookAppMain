using PhonebookAPI.Models;
using System.Diagnostics.CodeAnalysis;

namespace PhonebookAPI.Dtos
{
    [ExcludeFromCodeCoverage]
    public class StateDto
    {
        public int StateId { get; set; }
        public string StateName { get; set; }
        public int CountryId { get; set; }

        //public virtual Country Country { get; set; }
        //public virtual ICollection<Contact> Contacts { get; set; }
    }
}
