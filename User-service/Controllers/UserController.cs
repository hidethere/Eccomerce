using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using User_service.Domain;
using User_service.Dto;
using User_service.Service;

namespace User_service.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService) 
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] User user)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            ServiceResultDto result = await _userService.RegisterAsync(user);

            if (result == null) 
            {
                return Conflict(result);
            }

            return Ok(result);
            

        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequestDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            ServiceResultDto result = await _userService.LoginAsync(loginRequestDto);

            if (result == null)
            {
                return Conflict(result);
            }

            return Ok(result);
        }
    }
}
