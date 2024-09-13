using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PhonebookAPI.Services.Contract;
using System.Diagnostics.CodeAnalysis;

namespace PhonebookAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class CountryController : ControllerBase
    {
        private readonly ICountryService _countryService;
        
        public CountryController(ICountryService countryService)
        {
            _countryService = countryService;
        }

        [HttpGet("GetAllCountry")]
        public IActionResult GetAllCountry()
        {
            var response = _countryService.GetAllCountry();
            if(!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpGet("GetCountryById/{id}")]
        public IActionResult GetCountryById(int id)
        {
            var response = _countryService.GetCoutryById(id);

            if(!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);
        }
    }
}
