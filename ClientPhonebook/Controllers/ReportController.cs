using ClientPhonebook.Infrastructure;
using ClientPhonebook.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ClientPhonebook.Controllers
{
    public class ReportController : Controller
    {
        private readonly IHttpClientService _httpClientService;

        private readonly IConfiguration _configuration;

        private string endPoint;

        public ReportController(IHttpClientService httpClientService, IConfiguration configuration)
        {
            _httpClientService = httpClientService;
            _configuration = configuration;
            endPoint = _configuration["EndPoint:CivicaApi"];
        }

        [HttpGet]
        public IActionResult GetContactByCountry()
        {
            var apiUrl = $"{endPoint}Contact/GetAllContactByCountry";
            var response = _httpClientService.GetHttpResponseMessage<GetContactByCountryViewModel>(apiUrl, HttpContext.Request);


            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                var serviceResponse = JsonConvert.DeserializeObject<ServiceResponse<IEnumerable<GetContactByCountryViewModel>>>(data);

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
                var errorResponse = JsonConvert.DeserializeObject<ServiceResponse<IEnumerable<GetContactByCountryViewModel>>>(errorData);

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

        [HttpGet]
        public IActionResult GetContactByGender()
        {
            var apiUrl = $"{endPoint}Contact/GetAllContactByGenderSP";
            var response = _httpClientService.GetHttpResponseMessage<GetContactByGenderViewModel>(apiUrl, HttpContext.Request);


            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                var serviceResponse = JsonConvert.DeserializeObject<ServiceResponse<IEnumerable<GetContactByGenderViewModel>>>(data);

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
                var errorResponse = JsonConvert.DeserializeObject<ServiceResponse<IEnumerable<GetContactByGenderViewModel>>>(errorData);

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

        //-----------GetContactByState-----------
        private IEnumerable<CountryViewModel> GetCountries()
        {
            var apiUrl = $"{endPoint}Country/GetAllCountry";

            ServiceResponse<IEnumerable<CountryViewModel>> response = new ServiceResponse<IEnumerable<CountryViewModel>>();

            response = _httpClientService.ExecuteApiRequest<ServiceResponse<IEnumerable<CountryViewModel>>>
                (apiUrl, HttpMethod.Get, HttpContext.Request);

            return response.Data;
        }

        
        public IActionResult GetContactByState(int stateId)
        {

            var apiUrl = $"{endPoint}Contact/GetAllContactByStateIdSP/"+stateId;
            var response = _httpClientService.GetHttpResponseMessage<GetContactByStateViewModel>(apiUrl, HttpContext.Request);

            IEnumerable<CountryViewModel> countries = GetCountries();
            ViewBag.Countries = countries;

            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                var serviceResponse = JsonConvert.DeserializeObject<ServiceResponse<IEnumerable<GetContactByStateViewModel>>>(data);

                if (serviceResponse != null && serviceResponse.Success && serviceResponse.Data != null)
                {
                    return View(serviceResponse.Data);
                }
                else
                {
                    //TempData["ErrorMessage"] = serviceResponse?.Message;
                    return View();
                }
            }
            else
            {
                string errorData = response.Content.ReadAsStringAsync().Result;
                var errorResponse = JsonConvert.DeserializeObject<ServiceResponse<IEnumerable<GetContactByStateViewModel>>>(errorData);

                if (errorResponse != null)
                {
                    TempData[""] = errorResponse.Message;
                }
                else
                {
                    TempData["ErrorMessage"] = "Something went wrong please try after some time.";
                }

                return View();
            }
            
        }

        //----------------GetContactByMonth
        [HttpGet]
        public IActionResult GetContactByMonth(int month = 0)
        {
            if (month < 1 || month > 12)
            {
                return View();
            }

            var apiUrl = $"{endPoint}Contact/GetAllContactByMonth/" + month;
            var response = _httpClientService.GetHttpResponseMessage<GetContactByMonthViewModel>(apiUrl, HttpContext.Request);

            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                var serviceResponse = JsonConvert.DeserializeObject<ServiceResponse<IEnumerable<GetContactByMonthViewModel>>>(data);

                if (serviceResponse != null && serviceResponse.Success && serviceResponse.Data != null)
                {
                    return View(serviceResponse.Data);
                }
                else
                {
                    TempData["ErrorMessage"] = serviceResponse?.Message;
                    return View();
                }
            }
            else
            {
                string errorData = response.Content.ReadAsStringAsync().Result;
                var errorResponse = JsonConvert.DeserializeObject<ServiceResponse<IEnumerable<GetContactByMonthViewModel>>>(errorData);

                if (errorResponse != null)
                {
                    TempData["ErrorMessage"] = errorResponse.Message;
                }
                else
                {
                    TempData["ErrorMessage"] = "Something went wrong please try after some time.";
                }

                return View();

            }
        }
    }
}
