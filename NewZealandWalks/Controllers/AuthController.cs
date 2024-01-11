using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NewZealandWalks.Models.DTOs;

namespace NewZealandWalks.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;

        public AuthController(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }
        //POST:/api/Auth/Register
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDTO registerRequestDTO)
        {
            var identityUser = new IdentityUser
            {
                UserName = registerRequestDTO.UserName,
                Email = registerRequestDTO.UserName
            };
            var identityResult =  await _userManager.CreateAsync(identityUser, registerRequestDTO.Password);
            
            if (identityResult.Succeeded) 
            {
                // Add roles to this user
                if (registerRequestDTO.Roles != null && registerRequestDTO.Roles.Any()) 
                {
                    identityResult = await _userManager.AddToRolesAsync(identityUser, registerRequestDTO.Roles);
                    if (identityResult.Succeeded) 
                    {
                        return Ok("User Was registered ! Please Login.");
                    }
                }
            }
            return BadRequest("Something Went wrong");

        }

    }
}
 