using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NewZealandWalks.Models.DTOs;
using NewZealandWalks.Repositories;

namespace NewZealandWalks.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ITokenRepository _tokenRepository;

        public AuthController(UserManager<IdentityUser> userManager,ITokenRepository tokenRepository)
        {
            _userManager = userManager;
            _tokenRepository = tokenRepository;
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

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO loginRequestDTO) 
        {
            var user = await _userManager.FindByEmailAsync(loginRequestDTO.Username);

            if (user != null) 
            {
                var checkPasswordResult =  await _userManager.CheckPasswordAsync(user, loginRequestDTO.Password);
                if (checkPasswordResult) 
                {
                    //Get ROles fot this user
                    var roles = await _userManager.GetRolesAsync(user);
                    if (roles != null) 
                    {
                        //Create Token
                        var jwtToken = _tokenRepository.CreateJWTToken(user,roles.ToList());
                        var response = new LoginResponseDTO
                        {
                            JwtToken = jwtToken,
                        };
                        return Ok(response);
                    }
                    
                }
            }
            return BadRequest("UserName or Password was incorrect");
                    
        }

    }
}
 