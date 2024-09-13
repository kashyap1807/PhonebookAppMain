using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PhonebookApp.Data;
using PhonebookApp.Models;
using PhonebookApp.Services.Contract;
using PhonebookApp.ViewModel;

namespace PhonebookApp.Controllers
{
    [Authorize]
    public class ContactController : Controller
    {

        private readonly IContactService _contactService;

        public ContactController(IContactService contactService)
        {
            _contactService = contactService;
        }


        public IActionResult Index()
        {
            var contacts = _contactService.GetAllContact();
            if (contacts != null && contacts.Any())
            {
                return View("Index",contacts);
            }

            
            return View("Index",new List<Contact>());
        }

        [Authorize]
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var contacts = _contactService.GetContact(id);
            if (contacts == null)
            {
                return NotFound();
            }

            return View(contacts);
        }

        [HttpPost]
        public IActionResult Edit(Contact contact)
        {

            if (ModelState.IsValid)
            {
                var message = _contactService.ModifyContact(contact);
                if (message == "Contact already exists." || message == "Something went wrong, please try after sometime.")
                {
                    TempData["ErrorMessage"] = message;
                }
                else
                {
                    TempData["SuccessMessage"] = message;
                    return RedirectToAction("Index");
                }
            }
            return View(contact);
        }

        [Authorize]
        [HttpGet]
        public IActionResult Create()
        {

            return View();
        }

        [HttpPost]
        public IActionResult Create(ContactViewModel contactViewModel)
        {

            if (ModelState.IsValid)
            {

                var contact = new Contact()
                {
                    FirstName = contactViewModel.FirstName,
                    LastName = contactViewModel.LastName,
                    Email = contactViewModel.Email,
                    Company = contactViewModel.Company,
                    ContactNumber = contactViewModel.ContactNumber,
                    
                };


                var result = _contactService.AddContact(contact, contactViewModel.File);
                if (result == "Contact already exists." || result == "Something went wrong, please try after sometime.")
                {
                    TempData["ErrorMessage"] = result;
                }
                else if (result == "Contact saved successfully.")
                {
                    TempData["SuccessMessage"] = result;
                    return RedirectToAction("Index");
                }
            }

            return View(contactViewModel);
        }

        public IActionResult Details(int id)
        {
            var contacts = _contactService.GetContact(id);
            if (contacts == null)
            {
                return NotFound();
            }

            return View(contacts);
        }

        [Authorize]
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var contacts = _contactService.GetContact(id);
            if (contacts == null)
            {
                return NotFound();
            }

            return View(contacts);
        }

        [HttpPost]
        public IActionResult DeleteConfirmed(int contactId)
        {
            var result = _contactService.RemoveContact(contactId);

            if (result == "Contact deleted successfully.")
            {
                TempData["SuccessMessage"] = result;
            }
            else
            {
                TempData["ErrorMessage"] = result;
            }

            return RedirectToAction("Index");

        }
    }
}
