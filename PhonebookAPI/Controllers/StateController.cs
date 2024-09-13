using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PhonebookAPI.Services.Contract;
using System.Diagnostics.CodeAnalysis;

namespace PhonebookAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class StateController : ControllerBase
    {
        public readonly IStateService _stateService;

        public StateController(IStateService stateService)
        {
            _stateService = stateService;
        }

        [HttpGet("GetStateByCountryId/{countryId}")]
        public IActionResult GetStateByCountryId(int countryId)
        {
            var response = _stateService.GetStateByCountryId(countryId);

            if (!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);
        }
    }
}
