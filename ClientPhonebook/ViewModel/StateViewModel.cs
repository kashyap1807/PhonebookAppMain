
namespace ClientPhonebook.ViewModel
{
    public class StateViewModel
    {
        public int StateId { get; set; }
        public string StateName { get; set; }
        public int CountryId { get; set; }

        public virtual CountryViewModel Country { get; set; }
    }
}
