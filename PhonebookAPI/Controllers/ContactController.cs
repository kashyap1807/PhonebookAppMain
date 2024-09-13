using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PhonebookAPI.Dtos;
using PhonebookAPI.Models;
using PhonebookAPI.Services.Contract;

namespace PhonebookAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class ContactController : ControllerBase
    {
        private readonly IContactService _contactService;
        private readonly IFileService _fileService;

        public ContactController(IContactService contactService, IFileService fileService)
        {
            _contactService = contactService;
            _fileService = fileService;
        }


        [HttpGet("GetAllContactsWithPaginationFavSearchSort")]
        public IActionResult GetAllPaginatedContactsStartingWithLetter(int page = 1, int pageSize = 5, string? search_string = null, string sort_name = "default", bool show_favourites = false)
        {
            var response = _contactService.GetPaginatedContactsStartingWithLetter(page, pageSize, search_string, sort_name, show_favourites);
            if (!response.Success)
            {
                return NotFound(response);
            }

            return Ok(response);
        }

        

        [HttpGet("GetTotalContactsWithSearchFav")]
        public IActionResult GetTotalContacts(string? search_string, bool show_favourites)
        {
            var response = _contactService.TotalContacts(search_string, show_favourites);
            if (!response.Success)
            {
                return NotFound(response);
            }

            return Ok(response);
        }


        [HttpGet("GetContactById/{id}")]
        public IActionResult GetContactById(int id)
        {
            var response = _contactService.GetContactById(id);

            if (!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        //[Authorize]
        [HttpPost("Create")]
        public IActionResult AddContact([FromForm]AddContactDto addContactDto)
        {
            var contact = new Contact()
            {
                FirstName = addContactDto.FirstName,
                LastName = addContactDto.LastName,
                Email = addContactDto.Email,
                Company = addContactDto.Company,
                ContactNumber = addContactDto.ContactNumber,
                //FileName = addContactDto.FileName,
                Gender = addContactDto.Gender,
                IsFavourite = addContactDto.IsFavourite,
                CountryId = addContactDto.CountryId,
                StateId = addContactDto.StateId,
                BirthDate = addContactDto.BirthDate,


            };
            byte[] imageBytes;
            // Get bytes and file name if image is not null
            if (addContactDto.Image != null)
            {
                imageBytes = _fileService.ToByteArray(addContactDto.Image);
                contact.Image = imageBytes;

                // Getting the file name
                contact.FileName = addContactDto.Image.FileName;
            }
            var result = _contactService.AddContact(contact);

            return !result.Success ? BadRequest(result) : Ok(result);
        }

        //[Authorize]
        [HttpPut("ModifyContact")]
        public IActionResult UpdateContact([FromForm]UpdateContactDto updateContactDto)
        {
            var contact = new Contact()
            {
                ContactId = updateContactDto.contactId,
                FirstName = updateContactDto.FirstName,
                LastName = updateContactDto.LastName,
                Email = updateContactDto.Email,
                Company = updateContactDto.Company,
                ContactNumber = updateContactDto.ContactNumber,
                //FileName = updateContactDto.FileName,
                Gender = updateContactDto.Gender,
                IsFavourite = updateContactDto.IsFavourite,
                CountryId = updateContactDto.CountryId,
                StateId = updateContactDto.StateId,
                BirthDate = updateContactDto.BirthDate,

            };
            

            var result = _contactService.ModifyContact(updateContactDto);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            else
            {
                return Ok(result);
            }
        }
        

        //[Authorize]
        [HttpDelete("Remove/{id}")]
        public IActionResult RemoveContact(int id)
        {
            if (id > 0)
            {
                var response = _contactService.RemoveContact(id);
                if (!response.Success)
                {
                    return BadRequest(response);
                }
                else
                {
                    return Ok(response);
                }
            }
            else
            {
                return BadRequest("Please enter proper data.");
            }

        }

        //Store procedure
        [HttpGet("GetAllContactsSP")]
        public IActionResult GetAllContactsSP()
        {
            var response = _contactService.GetAllContactSP();
            if (!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpGet("GetAllContactByCountry")]
        public IActionResult GetContactByCountry()
        {
            var response = _contactService.GetAllContactByCountry();
            if (!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpGet("GetAllContactByGenderSP")]
        public IActionResult GetAllContactByGender()
        {
            var response = _contactService.GetAllContactByGender();
            if (!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpGet("GetAllContactByStateIdSP/{stateId}")]
        public IActionResult GetAllContactByStateId(int stateId)
        {
            var response = _contactService.GetAllContactByStateId(stateId);
            if (!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);
        }


        [HttpGet("GetAllContactByMonth/{month}")]
        public IActionResult GetAllContactByMonth(int month)
        {
            var response = _contactService.GetAllContactByMonth(month);
            if (!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

    }
}
