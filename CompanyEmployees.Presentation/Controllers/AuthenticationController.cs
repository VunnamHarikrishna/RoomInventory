using CompanyEmployees.Presentation.ValidationFilter_Attribute;
using Contracts;
using Microsoft.AspNetCore.Mvc;
using Shared.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyEmployees.Presentation.Controllers
{
    [Route("api/authentication")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IServiceManager _service;

        public AuthenticationController(IServiceManager service) => _service = service;

        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> RegisterUser([FromBody] UserForRegistrationDto userForRegistration)
        {
            var result = await  _service.AuthenticationService.RegisterUser(userForRegistration);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.TryAddModelError(error.Code, error.Description);
                }
                return BadRequest(ModelState);
            }
            return StatusCode(201);
        }
        [HttpPost("login")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> Authenticate([FromBody] UserForAuthenticationDto user)
        {
            try
            {
                if (user is null)
                    return BadRequest("UserForAuthenticationDto object is null");

                if (!await _service.AuthenticationService.ValidateUser(user))
                    return Unauthorized();
                var tokenDto = await _service.AuthenticationService.CreateToken(populateExp: true);

                return Ok(tokenDto);
            }
            catch(Exception ex)
            { 
                // Log the exception (not shown here for brevity)
                return StatusCode(500, "Internal server error");
            }

        }
    }

    }
