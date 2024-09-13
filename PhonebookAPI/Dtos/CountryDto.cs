using PhonebookAPI.Models;
using System.Diagnostics.CodeAnalysis;

namespace PhonebookAPI.Dtos
{
    [ExcludeFromCodeCoverage]
    public class CountryDto
    {
        public int CountryId { get; set; }
        public string CountryName { get; set; }

        //public virtual ICollection<Contact> Contacts { get; set; }
        //public virtual ICollection<State> States { get; set; }
    }
}
