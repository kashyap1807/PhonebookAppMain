﻿namespace PhonebookAPI.Dtos
{
    public class GetAllContactByCountryDto
    {
        public int CountryId { get; set; }
        public string CountryName { get; set; }

        public int TotalContacts { get; set; }
    }
}
