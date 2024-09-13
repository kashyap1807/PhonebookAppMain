using ClientPhonebook.Infrastructure;
using ClientPhonebook.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics.CodeAnalysis;

namespace ClientPhonebook.Controllers
{
    public class ContactController : Controller
    {

        private readonly IHttpClientService _httpClientService;

        private readonly IConfiguration _configuration;

        private string endPoint;

        public ContactController(IHttpClientService httpClientService, IConfiguration configuration)
        {
            _httpClientService = httpClientService;
            _configuration = configuration;
            endPoint = _configuration["EndPoint:CivicaApi"];
        }


        public IActionResult Index(int page = 1, int pageSize = 4, string? search_string = null, string sort_name = "default", bool show_favourites = false)
        {
            var apiUrl = $"{endPoint}Contact/GetAllContactsWithPaginationFavSearchSort"
                + "?page=" + page
                + "&pageSize=" + pageSize
                + "&search_string=" + search_string
                + "&sort_name=" + sort_name
                + "&show_favourites=" + show_favourites;

            var totalCountApiUrl = $"{endPoint}Contact/GetTotalContactsWithSearchFav"
                + "?search_string=" + search_string
                + "&show_favourites=" + show_favourites;


            ServiceResponse<int> countResponse = new ServiceResponse<int>();
            ServiceResponse<IEnumerable<ContactViewModel>> response = new ServiceResponse<IEnumerable<ContactViewModel>>();

            response = _httpClientService.ExecuteApiRequest<ServiceResponse<IEnumerable<ContactViewModel>>>
                (apiUrl, HttpMethod.Get, HttpContext.Request);

            countResponse = _httpClientService.ExecuteApiRequest<ServiceResponse<int>>
                (totalCountApiUrl, HttpMethod.Get, HttpContext.Request);

            var totalCount = countResponse.Data;

            var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);


            ViewBag.page = page;
            ViewBag.pageSize = pageSize;
            ViewBag.TotalPages = totalPages;
            ViewBag.search_string = search_string;
            ViewBag.sort_name = sort_name;
            ViewBag.show_favourites = show_favourites;

            if (response.Success)
            {
             
                return View(response.Data);
            }

            return View(new List<ContactViewModel>());
        }


        private IEnumerable<CountryViewModel> GetCountries()
        {
            var apiUrl = $"{endPoint}Country/GetAllCountry";

            ServiceResponse<IEnumerable<CountryViewModel>> response = new ServiceResponse<IEnumerable<CountryViewModel>>();

            response = _httpClientService.ExecuteApiRequest<ServiceResponse<IEnumerable<CountryViewModel>>>
                (apiUrl, HttpMethod.Get, HttpContext.Request);

            return response.Data;
        }



        [Authorize]
        [HttpGet]
        public IActionResult Create()
        {
            IEnumerable<CountryViewModel> countries = GetCountries();
            ViewBag.Countries = countries;
            return View();
        }

        [HttpPost]
        public IActionResult Create(AddContactViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                // Adding file name string to view model
                //IFormFile imageFile = viewModel.File;
                //if (imageFile != null && imageFile.Length > 0)
                //{
                //    var fileName = AddImageFileToPath(imageFile);
                //    viewModel.FileName = fileName;
                //}


                var apiUrl = $"{endPoint}Contact/Create";
                var response = _httpClientService.PostHttpResponseMessageFormData<AddContactViewModel>(apiUrl, viewModel, HttpContext.Request);

                if (response.IsSuccessStatusCode)
                {
                    string data = response.Content.ReadAsStringAsync().Result;
                    var serviceResponse = JsonConvert.DeserializeObject<ServiceResponse<AddContactViewModel>>(data);

                    if (serviceResponse != null && serviceResponse.Success && serviceResponse.Data != null)
                    {
                        return View(serviceResponse.Data);
                    }
                    else
                    {
                        TempData["SuccessMessage"] = serviceResponse.Message;
                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    string errorData = response.Content.ReadAsStringAsync().Result;
                    var errorResponse = JsonConvert.DeserializeObject<ServiceResponse<AddContactViewModel>>(errorData);

                    if (errorResponse != null)
                    {
                        TempData["ErrorMessage"] = errorResponse.Message;
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Something went wrong please try after some time.";
                    }
                    return RedirectToAction("Index");
                }
            }

            IEnumerable<CountryViewModel> countries = GetCountries();
            ViewBag.Countries = countries;
            return View(viewModel);

        }

        [Authorize]
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var apiUrl = $"{endPoint}Contact/GetContactById/" + id;
            var response = _httpClientService.GetHttpResponseMessage<UpdateContactViewModel>(apiUrl, HttpContext.Request);

            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                var serviceResponse = JsonConvert.DeserializeObject<ServiceResponse<UpdateContactViewModel>>(data);

                if (serviceResponse != null && serviceResponse.Success && serviceResponse.Data != null)
                {
                    IEnumerable<CountryViewModel> countries = GetCountries();
                    ViewBag.Countries = countries;
                    ViewBag.StateId = serviceResponse.Data.StateId;
                    return View(serviceResponse.Data);
                }
                else
                {
                    TempData["ErrorMessage"] = serviceResponse?.Message;
                    return RedirectToAction("Index");
                }
            }
            else
            {
                string errorData = response.Content.ReadAsStringAsync().Result;
                var errorResponse = JsonConvert.DeserializeObject<ServiceResponse<UpdateContactViewModel>>(errorData);

                if (errorResponse != null)
                {
                    TempData["ErrorMessage"] = errorResponse.Message;
                }
                else
                {
                    TempData["ErrorMessage"] = "Something went wrong please try after some time.";
                }

                return RedirectToAction("Index");
            }
            
        }

        [HttpPost]
        public IActionResult Edit(UpdateContactViewModel contactViewModel)
        {
            if (ModelState.IsValid)
            {
                // Adding file name string to view model
                //IFormFile imageFile = contactViewModel.File;
                //if (imageFile != null && imageFile.Length > 0)
                //{
                //    var fileName = AddImageFileToPath(imageFile);
                //    contactViewModel.FileName = fileName;
                //}

                var apiUrl = $"{endPoint}Contact/ModifyContact";
                HttpResponseMessage response = _httpClientService.PutHttpResponseMessageFormData(apiUrl, contactViewModel, HttpContext.Request);

                if (response.IsSuccessStatusCode)
                {
                    string successResponse = response.Content.ReadAsStringAsync().Result;
                    var serviceResponse = JsonConvert.DeserializeObject<ServiceResponse<string>>(successResponse);
                    TempData["successMessage"] = serviceResponse.Message;
                    return RedirectToAction("Index");
                }
                else
                {
                    string errorResponse = response.Content.ReadAsStringAsync().Result;
                    var serviceResponse = JsonConvert.DeserializeObject<ServiceResponse<string>>(errorResponse);

                    if (serviceResponse != null)
                    {
                        TempData["ErrorMessage"] = serviceResponse?.Message;
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Something went wrong please try after some time.";
                    }
                }
            }

            IEnumerable<CountryViewModel> countries = GetCountries();
            ViewBag.Countries = countries;
            return View(contactViewModel);

        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            var apiUrl = $"{endPoint}Contact/GetContactById/" + id;
            var response = _httpClientService.GetHttpResponseMessage<ContactViewModel>(apiUrl, HttpContext.Request);

            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                var serviceResponse = JsonConvert.DeserializeObject<ServiceResponse<ContactViewModel>>(data);

                if (serviceResponse != null && serviceResponse.Success && serviceResponse.Data != null)
                {
                    if (serviceResponse.Data.ImageBytes != null)
                    {
                        ViewBag.imageByteString = Convert.ToBase64String(serviceResponse.Data.ImageBytes);
                    }
                    else
                    {
                        ViewBag.imageByteString = null;
                    }
                    return View(serviceResponse.Data);
                }
                else
                {
                    TempData["ErrorMessage"] = serviceResponse.Message;
                    return RedirectToAction("Index");
                }
            }
            else
            {
                string errorData = response.Content.ReadAsStringAsync().Result;
                var errorResponse = JsonConvert.DeserializeObject<ServiceResponse<ContactViewModel>>(errorData);

                if (errorResponse != null)
                {
                    TempData["ErrorMessage"] = errorResponse.Message;
                }
                else
                {
                    TempData["ErrorMessage"] = "Something went wrong please try after some time.";
                }

                return RedirectToAction("Index");
            }
            
        }

        [Authorize]
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var apiUrl = $"{endPoint}Contact/GetContactById/" + id;
            var response = _httpClientService.GetHttpResponseMessage<ContactViewModel>(apiUrl, HttpContext.Request);

            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                var serviceResponse = JsonConvert.DeserializeObject<ServiceResponse<ContactViewModel>>(data);

                if (serviceResponse != null && serviceResponse.Success && serviceResponse.Data != null)
                {
                    return View(serviceResponse.Data);
                }
                else
                {
                    TempData["ErrorMessage"] = serviceResponse?.Message;
                    return RedirectToAction("Index");
                }
            }
            else
            {
                string errorData = response.Content.ReadAsStringAsync().Result;
                var errorResponse = JsonConvert.DeserializeObject<ServiceResponse<ContactViewModel>>(errorData);

                if (errorResponse != null)
                {
                    TempData["ErrorMessage"] = errorResponse.Message;
                }
                else
                {
                    TempData["ErrorMessage"] = "Something went wrong please try after some time.";
                }

                return RedirectToAction("Index");
            }
            
        }

        [HttpPost]
        public IActionResult DeleteConfirmed(int contactId)
        {
            var apiUrl = $"{endPoint}Contact/Remove/" + contactId;
            //var response = _httpClientService.GetHttpResponseMessage<string>(apiUrl, HttpContext.Request);

            var response = _httpClientService.ExecuteApiRequest<ServiceResponse<string>>
                ($"{apiUrl}", HttpMethod.Delete, HttpContext.Request);

            if (response.Success)
            {
                TempData["SuccessMessage"] = response.Message;
                return RedirectToAction("Index");
            }
            else
            {
                TempData["ErrorMessage"] = response.Message;
                return RedirectToAction("Index");
            }

            return View();
        }

        [ExcludeFromCodeCoverage]

        private string AddImageFileToPath(IFormFile imageFile)
        {
            // Process the uploaded file(eq. save it to disk)
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads", imageFile.FileName);

            // Save the file to storage and set path
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                imageFile.CopyTo(stream);
                return imageFile.FileName;
            }
        }


    }
}
