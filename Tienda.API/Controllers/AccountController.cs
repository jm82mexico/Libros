
using Microsoft.AspNetCore.Mvc;
using Tienda.Application.Contracts.Identity;
using Tienda.Application.Models.Identity;

namespace Tienda.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AccountController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponse>> Login([FromBody] AuthRequest request)
        {
            var response = await _authService.Login(request);
            return Ok(response);
        }

        [HttpPost("register")]
        public async Task<ActionResult<RegistrationResponse>> Register([FromBody] RegistrationRequest request)
        {
            var response = await _authService.Register(request);
            return Ok(response);
        }

        
    }
}