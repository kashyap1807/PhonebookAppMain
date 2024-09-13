using ClientPhonebook.Infrastructure;
using ClientPhonebook.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ClientPhonebook.Controllers
{
    
    public class AuthController : Controller
    {
        private readonly IHttpClientService _httpClientService;

        private readonly IConfiguration _configuration;

        private string endPoint;

        public AuthController(IHttpClientService httpClientService, IConfiguration configuration)
        {
            _httpClientService = httpClientService;
            _configuration = configuration;
            endPoint = _configuration["EndPoint:CivicaApi"];
        }

        [HttpGet]
        public IActionResult RegisterUser()
        {
            return View();
        }

        [HttpPost]
        public IActionResult RegisterUser(RegisterViewModel registerViewModel)
        {
            if (ModelState.IsValid)
            {
                var apiUrl = $"{endPoint}Auth/Register";
                var response = _httpClientService.PostHttpResponseMessage(apiUrl, registerViewModel, HttpContext.Request);

                if (response.IsSuccessStatusCode)
                {
                    string data = response.Content.ReadAsStringAsync().Result;
                    var serviceResponse = JsonConvert.DeserializeObject<ServiceResponse<string>>(data);

                    if (serviceResponse != null && serviceResponse.Success && serviceResponse.Data != null)
                    {
                        return View(serviceResponse.Data);
                    }
                    else
                    {
                        TempData["SuccessMessage"] = serviceResponse.Message;
                        return RedirectToAction("RegisterSuccess");
                    }
                }
                else
                {
                    string errorData = response.Content.ReadAsStringAsync().Result;
                    var errorResponse = JsonConvert.DeserializeObject<ServiceResponse<string>>(errorData);

                    if (errorResponse != null)
                    {
                        TempData["ErrorMessage"] = errorResponse.Message;
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Something went wrong please try after some time.";
                    }
                    //return RedirectToAction("Index");
                }

            }
            return View(registerViewModel);
        }

        public IActionResult RegisterSuccess()
        {
            return View();
        }

        [HttpGet]
        public IActionResult LoginUser()
        {
            return View();
        }

        [HttpPost]
        public IActionResult LoginUser(LoginViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                string apiUrl = $"{endPoint}Auth/Login";
                var response = _httpClientService.PostHttpResponseMessage(apiUrl, viewModel, HttpContext.Request);
                if (response.IsSuccessStatusCode)
                {
                    string successResponse = response.Content.ReadAsStringAsync().Result;
                    var serviceResponse = JsonConvert.DeserializeObject<ServiceResponse<string>>(successResponse);

                    string token = serviceResponse.Data;
                    Response.Cookies.Append("jwtToken", token, new CookieOptions
                    {
                        HttpOnly = false,
                        Secure = true,
                        //SameSite = SameSiteMode.Strict
                        SameSite = SameSiteMode.None,
                        Expires = DateTime.UtcNow.AddHours(1)
                    });

                    return RedirectToAction("Index", "Contact");
                }
                else
                {
                    string errorResponse = response.Content.ReadAsStringAsync().Result;
                    var serviceResponse = JsonConvert.DeserializeObject<ServiceResponse<string>>(errorResponse);

                    if (serviceResponse != null)
                    {
                        TempData["ErrorMessage"] = serviceResponse.Message;
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Something went wrong, please try after some time.";
                    }
                }
            }
            return View(viewModel);


        }

        public IActionResult Logout()
        {
            Response.Cookies.Delete("jwtToken");

            return RedirectToAction("LoginUser", "Auth");
        }

        [HttpGet]
        public IActionResult GetUserDetails(string id)
        {
            var apiUrl = $"{endPoint}User/GetUserByLoginId/" + id;
            var response = _httpClientService.GetHttpResponseMessage<UserViewModel>(apiUrl, HttpContext.Request);

            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                var serviceResponse = JsonConvert.DeserializeObject<ServiceResponse<UserViewModel>>(data);

                if (serviceResponse != null && serviceResponse.Success && serviceResponse.Data != null)
                {
                    return View(serviceResponse.Data);
                }
                else
                {
                    TempData["errorMessage"] = serviceResponse.Message;
                    return RedirectToAction("Index");
                }

            }
            else
            {
                string errorData = response.Content.ReadAsStringAsync().Result;
                var errorResponse = JsonConvert.DeserializeObject<ServiceResponse<UserViewModel>>(errorData);

                if (errorResponse != null)
                {
                    TempData["errorMessage"] = errorResponse.Message;
                }
                else
                {
                    TempData["errorMessage"] = "Something went wrong. Please try after sometime.";
                }
                return RedirectToAction("Index");
            }

        }

        [HttpGet]
        public IActionResult UpdateUser(string id)
        {
            var apiUrl = $"{endPoint}User/GetUserByLoginId/" + id;
            var response = _httpClientService.GetHttpResponseMessage<UserViewModel>(apiUrl, HttpContext.Request);

            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                var serviceResponse = JsonConvert.DeserializeObject<ServiceResponse<UserViewModel>>(data);

                if (serviceResponse != null && serviceResponse.Success && serviceResponse.Data != null)
                {
                    return View(serviceResponse.Data);
                }
                else
                {
                    TempData["errorMessage"] = serviceResponse.Message;
                    return RedirectToAction("Index");
                }

            }
            else
            {
                string errorData = response.Content.ReadAsStringAsync().Result;
                var errorResponse = JsonConvert.DeserializeObject<ServiceResponse<UserViewModel>>(errorData);

                if (errorResponse != null)
                {
                    TempData["errorMessage"] = errorResponse.Message;
                }
                else
                {
                    TempData["errorMessage"] = "Something went wrong. Please try after sometime.";
                }
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public IActionResult UpdateUser(UserViewModel user)
        {
            if (ModelState.IsValid)
            {
                var apiUrl = $"{endPoint}User/UpdateUser";
                var response = _httpClientService.PutHttpResponseMessage(apiUrl, user, HttpContext.Request);
                if (response.IsSuccessStatusCode)
                {
                    string successResponse = response.Content.ReadAsStringAsync().Result;
                    var serviceResponse = JsonConvert.DeserializeObject<ServiceResponse<string>>(successResponse);
                    string token = serviceResponse.Data;
                    Response.Cookies.Append("jwtToken", token, new CookieOptions
                    {
                        HttpOnly = false,
                        Secure = true,
                        //SameSite = SameSiteMode.Strict
                        SameSite = SameSiteMode.None,
                        Expires = DateTime.UtcNow.AddHours(1)
                    });
                    TempData["successMessage"] = serviceResponse.Message;
                    return RedirectToAction("GetUserDetails", "Auth", new { id = user.LoginId });

                }
                else
                {
                    string errorResponse = response.Content.ReadAsStringAsync().Result;
                    var serviceResponse = JsonConvert.DeserializeObject<ServiceResponse<string>>(errorResponse);

                    if (serviceResponse != null)
                    {
                        TempData["ErrorMessage"] = serviceResponse.Message;
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Somethong went wrong, please try after sometime.";
                    }

                }


            }
            return View(user);
        }

        [HttpGet]
        public IActionResult ChangePassword(string id)
        {
            var apiUrl = $"{endPoint}User/GetUserByLoginId/" + id;
            var response = _httpClientService.GetHttpResponseMessage<ChangePasswordViewModel>(apiUrl, HttpContext.Request);

            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                var serviceResponse = JsonConvert.DeserializeObject<ServiceResponse<ChangePasswordViewModel>>(data);

                if (serviceResponse != null && serviceResponse.Success && serviceResponse.Data != null)
                {
                    return View(serviceResponse.Data);
                }
                else
                {
                    TempData["errorMessage"] = serviceResponse?.Message;
                    return RedirectToAction("LoginUser");
                }

            }
            else
            {
                string errorData = response.Content.ReadAsStringAsync().Result;
                var errorResponse = JsonConvert.DeserializeObject<ServiceResponse<ChangePasswordViewModel>>(errorData);

                if (errorResponse != null)
                {
                    TempData["errorMessage"] = errorResponse.Message;
                }
                else
                {
                    TempData["errorMessage"] = "Something went wrong. Please try after sometime.";
                }
                return RedirectToAction("LoginUser");
            }
        }


        [HttpPost]
        public IActionResult ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var apiUrl = $"{endPoint}Auth/ChangePassword";
                var response = _httpClientService.PutHttpResponseMessage(apiUrl, model, HttpContext.Request);
                if (response.IsSuccessStatusCode)
                {
                    string successResponse = response.Content.ReadAsStringAsync().Result;
                    var serviceResponse = JsonConvert.DeserializeObject<ServiceResponse<string>>(successResponse);
                    TempData["successMessage"] = serviceResponse.Message;
                    return RedirectToAction("Logout","Auth");

                }
                else
                {
                    string errorResponse = response.Content.ReadAsStringAsync().Result;
                    var serviceResponse = JsonConvert.DeserializeObject<ServiceResponse<string>>(errorResponse);

                    if (serviceResponse != null)
                    {
                        TempData["ErrorMessage"] = serviceResponse.Message;
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Somethong went wrong, please try after sometime.";
                    }

                }
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var apiUrl = $"{endPoint}Auth/ForgotPassword";
                var response = _httpClientService.PutHttpResponseMessage(apiUrl, model, HttpContext.Request);
                if (response.IsSuccessStatusCode)
                {
                    string successResponse = response.Content.ReadAsStringAsync().Result;
                    var serviceResponse = JsonConvert.DeserializeObject<ServiceResponse<string>>(successResponse);
                    TempData["successMessage"] = serviceResponse.Message;
                    return RedirectToAction("LoginUser", "Auth");

                }
                else
                {
                    string errorResponse = response.Content.ReadAsStringAsync().Result;
                    var serviceResponse = JsonConvert.DeserializeObject<ServiceResponse<string>>(errorResponse);

                    if (serviceResponse != null)
                    {
                        TempData["ErrorMessage"] = serviceResponse.Message;
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Somethong went wrong, please try after sometime.";
                    }

                }
            }
            return View(model);
        }

    }
}
