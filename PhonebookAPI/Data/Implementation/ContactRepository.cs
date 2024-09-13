using Microsoft.EntityFrameworkCore;
using PhonebookAPI.Data.Contract;
using PhonebookAPI.Dtos;
using PhonebookAPI.Models;

namespace PhonebookAPI.Data.Implementation
{
    public class ContactRepository : IContactRepository
    {
        private IAppDbContext _appDbContext;

        public ContactRepository(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        //public IEnumerable<Contact> GetAll()
        //{
        //    List<Contact> c = _appDbContext.Contacts.Include(c => c.Country).Include(s => s.State).ToList();
        //    return c;
        //}

        //Store Procedures
        public IEnumerable<AllContactSP> GetAllContactSP()
        {
            var contacts = _appDbContext.Procedures.GetAllContact();
            return contacts;
        }

        public IEnumerable<GetAllContactByCountryDto> GetAllContactByCountry()
        {
            var contacts = _appDbContext.Procedures.GetAllContactByCountry();
            return contacts;    
        }

        public IEnumerable<GetAllContactByGenderDto> GetAllContactByGender()
        {
            var contacts = _appDbContext.Procedures.GetAllContactByGender();
            return contacts;
        }

        public IEnumerable<GetAllContactByStateIdDto> GetAllContactByStateId(int stateId)
        {
            var contacts = _appDbContext.Procedures.GetAllContactByStateId(stateId);
            return contacts;
        }

        public IEnumerable<GetAllContactByMonthDto> GetAllContactByMonth(int month)
        {
            var contacts = _appDbContext.Procedures.GetAllContactByMonth(month);
            return contacts;
        }

        //---------------------------------------
        public IEnumerable<Contact> GetPaginatedContactsStartingWithLetter(int page, int pageSize,string? search_string, string sort_name, bool show_favourites)
        {
            var contacts = _appDbContext.Contacts.Include(c => c.Country).Include(s => s.State).AsQueryable();

            if (show_favourites)
            {
                contacts = contacts.Where(c => c.IsFavourite);
            }

            if (search_string != null)
            {
                
                    search_string = search_string.Trim('*');
                    contacts = contacts.Where(c=>c.FirstName.ToLower().StartsWith(search_string.ToLower()) || c.LastName.ToLower().StartsWith(search_string.ToLower()) || c.ContactNumber.Contains(search_string));
                
            }

            if (sort_name == "asc")
            {
                contacts = contacts.OrderBy(c => c.FirstName);
            }
            else if(sort_name == "desc")
            {
                contacts = contacts.OrderByDescending(c => c.FirstName);
            }
            else
            {
                contacts = contacts.OrderBy(c=>c.ContactId);
            }

            int skip = (page - 1) * pageSize;

            return contacts
                .Skip(skip)
                .Take(pageSize)
                .ToList();
        }
        
        

        public int TotalContacts(string? search_string,bool show_favourites)
        {
            var contacts = _appDbContext.Contacts.Include(c => c.Country).Include(c => c.State).AsQueryable();

            if (show_favourites)
            {
                contacts = contacts.Where(c => c.IsFavourite);
            }
            if (search_string != null)
            {
                
                    search_string = search_string.Trim('*');
                    contacts = contacts.Where(c => c.FirstName.ToLower().StartsWith(search_string.ToLower()) || c.LastName.ToLower().StartsWith(search_string.ToLower()) || c.ContactNumber.Contains(search_string));
       
            }

            return contacts.Count();
        }

        //public int TotalContactsStartingWithLetter(char? ch)
        //{
        //    return _appDbContext.Contacts.Where(c => c.FirstName.StartsWith(ch.ToString().ToLower())).Count();
        //}


        public Contact? GetContact(int id)
        {
            var c = _appDbContext.Contacts.Include(c => c.Country).Include(s => s.State).FirstOrDefault(c => c.ContactId == id);
            return c;
        }

        public bool InsertContact(Contact c)
        {
            var result = false;
            if (c != null)
            {
                _appDbContext.Contacts.Add(c);
                _appDbContext.SaveChanges();
                result = true;
            }

            return result;
        }

        public bool UpdateContact(Contact contacts)
        {
            var result = false;
            if (contacts != null)
            {
                _appDbContext.Contacts.Update(contacts);
                //_appDbContext.Entry(category).State = EntityState.Modified;
                _appDbContext.SaveChanges();
                result = true;
            }
            return result;
        }

        public bool DeleteContact(int id)
        {
            var result = false;
            var c = _appDbContext.Contacts.Find(id);
            if (c != null)
            {
                _appDbContext.Contacts.Remove(c);
                _appDbContext.SaveChanges();
                result = true;
            }

            return result;
        }


        public bool ContactExists(string num)
        {
            var c = _appDbContext.Contacts.FirstOrDefault(c => c.ContactNumber == num);
            if (c != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool ContactExists(int contactId, string num)
        {
            var c = _appDbContext.Contacts.FirstOrDefault(c => c.ContactId != contactId && c.ContactNumber == num);
            if (c != null)
            {
                return true;
            }

            else
            {
                return false;
            }

        }
    }
}
