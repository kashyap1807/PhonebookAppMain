using PhonebookApp.Data.Contract;
using PhonebookApp.Models;

namespace PhonebookApp.Data.Implementation
{
    public class ContactRepository : IContactRepository
    {
        private AppDbContext _appDbContext;

        public ContactRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public IEnumerable<Contact> GetAll()
        {
            List<Contact> c = _appDbContext.Contacts.ToList();
            return c;
        }

        public int TotalContacts()
        {
            return _appDbContext.Contacts.Count();
        }

        public IEnumerable<Contact> GetPaginatedCategories(int page, int pageSize)
        {
            int skip = (page - 1) * pageSize;
            return _appDbContext.Contacts
                .OrderBy(c => c.contactId)
                .Skip(skip)
                .Take(pageSize)
                .ToList();
        }

        public Contact? GetContact(int id)
        {
            var c = _appDbContext.Contacts.FirstOrDefault(c => c.contactId == id);
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
            var c = _appDbContext.Contacts.FirstOrDefault(c => c.contactId != contactId && c.ContactNumber == num);
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
