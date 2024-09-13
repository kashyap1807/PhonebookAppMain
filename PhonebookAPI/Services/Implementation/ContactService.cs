using PhonebookAPI.Data.Contract;
using PhonebookAPI.Dtos;
using PhonebookAPI.Models;
using PhonebookAPI.Services.Contract;
using System.Collections.Generic;

namespace PhonebookAPI.Services.Implementation
{
    public class ContactService : IContactService
    {
        private readonly IContactRepository _contactRepository;
        private readonly IFileService _fileService;

        public ContactService(IContactRepository contactRepository, IFileService fileService)
        {
            _contactRepository = contactRepository;
            _fileService = fileService;
        }

        public ServiceResponse<IEnumerable<ContactDto>> GetPaginatedContactsStartingWithLetter(int page, int pageSize, string? search_string, string sort_name, bool show_favourites)
        {
            var response = new ServiceResponse<IEnumerable<ContactDto>>();
            try
            {
                var contacts = _contactRepository.GetPaginatedContactsStartingWithLetter(page, pageSize, search_string, sort_name, show_favourites);
                if (contacts != null && contacts.Any())
                {
                    List<ContactDto> contactDtoList = new List<ContactDto>();
                    foreach (var contact in contacts)
                    {
                        ContactDto contactDto = new ContactDto();
                        contactDto.contactId = contact.ContactId;
                        contactDto.FirstName = contact.FirstName;
                        contactDto.LastName = contact.LastName;
                        contactDto.ContactNumber = contact.ContactNumber;
                        contactDto.Email = contact.Email;
                        contactDto.Company = contact.Company;
                        contactDto.FileName = contact.FileName;
                        contactDto.ImageBytes = contact.Image;

                        contactDto.CountryId = contact.CountryId;
                        contactDto.StateId = contact.StateId;
                        contactDto.Gender = contact.Gender;
                        contactDto.IsFavourite = contact.IsFavourite;
                        contactDto.BirthDate = contact.BirthDate;

                        contactDto.Country = new Country()
                        {
                            CountryId = contact.CountryId,
                            CountryName = contact.Country.CountryName,
                        };

                        contactDto.State = new State()
                        {
                            StateId = contact.StateId,
                            StateName = contact.State.StateName,
                            CountryId = contact.CountryId
                        };

                        contactDtoList.Add(contactDto);
                    }

                    response.Data = contactDtoList;
                    response.Total = _contactRepository.TotalContacts(search_string, show_favourites);
                    response.Success = true;
                    response.Message = "Success";
                }
                else
                {
                    response.Success = false;
                    response.Message = "No record found";
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }
            

            return response;
        }

        public ServiceResponse<int> TotalContacts(string? search_string, bool show_favourites)
        {
            var response = new ServiceResponse<int>();
            try
            {
                int count = _contactRepository.TotalContacts(search_string, show_favourites);

                response.Data = count;
                response.Success = true;
            }
            catch(Exception ex)
            {
                response.Success=false;
                response.Message = ex.Message;
            }
            

            return response;
        }

        //public ServiceResponse<int> TotalContactsStartingWithLetter(char? ch)
        //{
        //    var response = new ServiceResponse<int>();
        //    int count = _contactRepository.TotalContactsStartingWithLetter(ch);

        //    response.Data = count;
        //    response.Success = true;

        //    return response;
        //}

        //public ServiceResponse<IEnumerable<ContactDto>> GetAllContact()
        //{
        //    var response = new ServiceResponse<IEnumerable<ContactDto>>();
        //    var contacts = _contactRepository.GetAll();

        //    if (contacts != null && contacts.Any())
        //    {
        //        List<ContactDto> contactDtoList = new List<ContactDto>();
        //        foreach (var contact in contacts)
        //        {
        //            ContactDto contactDto = new ContactDto();
        //            contactDto.contactId = contact.ContactId;
        //            contactDto.FirstName = contact.FirstName;
        //            contactDto.LastName = contact.LastName;
        //            contactDto.ContactNumber = contact.ContactNumber;
        //            contactDto.Gender = contact.Gender;
        //            contactDto.CountryId = contact.CountryId;
        //            contactDto.StateId = contact.StateId;
        //            contactDto.IsFavourite = contact.IsFavourite;
        //            contactDto.Email = contact.Email;
        //            contactDto.Company = contact.Company;
        //            contactDto.FileName = contact.FileName;
        //            contactDto.ImageBytes = contact.Image;
        //            contactDto.Country = new Country()
        //            {
        //                CountryId = contact.Country.CountryId,
        //                CountryName = contact.Country.CountryName,
        //            };
        //            contactDto.State = new State()
        //            {
        //                StateId = contact.State.StateId,
        //                StateName = contact.State.StateName,
        //                CountryId = contact.State.CountryId,
        //            };
        //            contactDtoList.Add(contactDto);
        //        }

        //        response.Data = contactDtoList;
        //        response.Success = true;
        //        response.Message = "Success";
        //    }
        //    else
        //    {
        //        response.Success = false;
        //        response.Message = "No record found";
        //    }

        //    return response;
        //}

        //Procedure
        public ServiceResponse<IEnumerable<AllContactSP>> GetAllContactSP()
        {
            var response = new ServiceResponse<IEnumerable<AllContactSP>>();
            try
            {
                var report = _contactRepository.GetAllContactSP();
                if (report != null && report.Any())
                {
                    response.Success = true;
                    response.Data = report;
                    response.Message = "Found";
                }
                else
                {
                    response.Success = false;
                    response.Message = "Not found";
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }
            

            return response;
        }

        public ServiceResponse<IEnumerable<GetAllContactByCountryDto>> GetAllContactByCountry()
        {
            var response = new ServiceResponse<IEnumerable<GetAllContactByCountryDto>>();
            try
            {
                var report = _contactRepository.GetAllContactByCountry();
                if (report != null && report.Any())
                {
                    response.Success = true;
                    response.Data = report;
                    response.Message = "Record Found";
                }
                else
                {
                    response.Success = false;
                    response.Message = "No Record Found";
                }
            }
            catch (Exception ex)
            {
                response.Success= false;
                response.Message = ex.Message;
            }
           
            return response;
        }

        public ServiceResponse<IEnumerable<GetAllContactByGenderDto>> GetAllContactByGender()
        {
            var response = new ServiceResponse<IEnumerable<GetAllContactByGenderDto>>();
            try
            {
                var report = _contactRepository.GetAllContactByGender();
                if (report != null && report.Any())
                {
                    response.Success = true;
                    response.Data = report;
                    response.Message = "Record Found";
                }
                else
                {
                    response.Success = false;
                    response.Message = "No Record Found";
                }
            }
            catch(Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }
            
            return response;
        }

        public ServiceResponse<IEnumerable<GetAllContactByStateIdDto>> GetAllContactByStateId(int stateId)
        {
            var response = new ServiceResponse<IEnumerable<GetAllContactByStateIdDto>>();
            try
            {
                var report = _contactRepository.GetAllContactByStateId(stateId);
                if (report != null && report.Any())
                {
                    response.Success = true;
                    response.Data = report;
                    response.Message = "Record Found";
                }
                else
                {
                    response.Success = false;
                    response.Message = "No Record Found";
                }
            }
            catch(Exception ex)
            {
                response.Success = false;
                response.Message= ex.Message;
            }
            
            return response;
        }

        public ServiceResponse<IEnumerable<GetAllContactByMonthDto>> GetAllContactByMonth(int month)
        {
            var response = new ServiceResponse<IEnumerable<GetAllContactByMonthDto>>();
            try
            {
                var report = _contactRepository.GetAllContactByMonth(month);
                if (report != null && report.Any())
                {
                    response.Success = true;
                    response.Data = report;
                    response.Message = "Record Found";
                }
                else
                {
                    response.Success = false;
                    response.Message = "No Record Found";
                }
            }
            catch(Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }
            
            return response;
        }

        public ServiceResponse<ContactDto>  GetContactById(int id)
        {
            var response = new ServiceResponse<ContactDto>();
            try
            {
                var contact = _contactRepository.GetContact(id);

                if (contact != null)
                {
                    var contactDto = new ContactDto()
                    {
                        contactId = contact.ContactId,
                        FirstName = contact.FirstName,
                        LastName = contact.LastName,
                        Email = contact.Email,
                        Company = contact.Company,
                        ContactNumber = contact.ContactNumber,
                        FileName = contact.FileName,
                        CountryId = contact.CountryId,
                        StateId = contact.StateId,
                        Gender = contact.Gender,
                        IsFavourite = contact.IsFavourite,
                        ImageBytes = contact.Image,
                        BirthDate = contact.BirthDate,

                        Country = new Country()
                        {
                            CountryId = contact.CountryId,
                            CountryName = contact.Country.CountryName,
                        },
                        State = new State()
                        {
                            StateId = contact.StateId,
                            StateName = contact.State.StateName,
                            CountryId = contact.CountryId,
                        }

                    };

                    response.Data = contactDto;
                    response.Success = true;
                    response.Message = "Record Found";
                }
                else
                {
                    response.Success = false;
                    response.Message = "No Record Found";
                }
            }
            catch(Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }
            
            return response;

        }


        public ServiceResponse<string> AddContact(Contact contact)
        {

            var response = new ServiceResponse<string>();
            try
            {
                if (contact == null)
                {
                    response.Success = false;
                    response.Message = "Something went wrong. Please try after sometime.";
                    return response;
                }

                if (_contactRepository.ContactExists(contact.ContactId, contact.ContactNumber))
                {
                    response.Success = false;
                    response.Message = "Contact already exists.";
                    return response;
                }

                var result = _contactRepository.InsertContact(contact);

                if (result)
                {
                    response.Data = contact.ContactId.ToString();
                    response.Success = true;
                    response.Message = "Contact saved successfully.";

                }
                else
                {
                    response.Data = null;
                    response.Success = false;
                    response.Message = "Something went wrong. Please try after sometime.";
                }
            }
            catch(Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }
            
            return response;

        }

        public ServiceResponse<string> ModifyContact(UpdateContactDto contact)
        {
            var response = new ServiceResponse<string>();
            try
            {
                if (contact == null)
                {
                    response.Success = false;
                    response.Message = "Something went wrong. Please try after sometime.";
                    return response;
                }

                if (_contactRepository.ContactExists(contact.contactId, contact.ContactNumber))
                {
                    response.Success = false;
                    response.Message = "Contact already exists.";
                    return response;
                }

                var modifyContact = _contactRepository.GetContact(contact.contactId);
                if (modifyContact != null)
                {
                    modifyContact.FirstName = contact.FirstName;
                    modifyContact.LastName = contact.LastName;
                    modifyContact.Email = contact.Email;
                    modifyContact.Company = contact.Company;
                    modifyContact.ContactNumber = contact.ContactNumber;
                    modifyContact.CountryId = contact.CountryId;
                    modifyContact.StateId = contact.StateId;
                    modifyContact.Gender = contact.Gender;
                    modifyContact.IsFavourite = contact.IsFavourite;
                    modifyContact.BirthDate = contact.BirthDate;
                    byte[] imageBytes;
                    // Get bytes and file name if image is not null
                    if (contact.Image != null)
                    {
                        imageBytes = _fileService.ToByteArray(contact.Image);
                        modifyContact.Image = imageBytes;

                        // Getting the file name
                        modifyContact.FileName = contact.Image.FileName;
                    }
                    var result = _contactRepository.UpdateContact(modifyContact);

                    response.Success = result;
                    response.Message = result ? "Contact updated successfully." : "Something went wrong. Please try after sometime.";
                }
                else
                {
                    response.Success = false;
                    response.Message = "Something went wrong. Please try after sometime.";
                    return response;
                }
            }
            catch(Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }
            
            return response;
        }

        public ServiceResponse<string> RemoveContact(int id)
        {
            var response = new ServiceResponse<string>();
            try
            {
                if (id < 0)
                {
                    response.Success = false;
                    response.Message = "No record to delete.";

                }

                var result = _contactRepository.DeleteContact(id);
                response.Success = result;
                response.Message = result ? "Contact deleted successfully." : "Something went wrong, please try after sometime.";
            }
            catch(Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }
            
            return response;
        }
    }
}
