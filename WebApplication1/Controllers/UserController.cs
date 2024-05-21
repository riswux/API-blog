using BLL.Interface;
using BLL.Service;
using DTO.Helpers;
using DTO.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using WebApplication1.Middleware;

namespace WebApplication1.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly JwtTokenGenerator _jwtTokenGenerator;
        private readonly ITokenLifetimeManager _tokenLifetimeManager;


        public UserController(IUserService userService, JwtTokenGenerator jwtTokenGenerator, ITokenLifetimeManager tokenLifetimeManager)
        {
            _userService = userService;
            _jwtTokenGenerator = jwtTokenGenerator;
            _tokenLifetimeManager = tokenLifetimeManager;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromBody] UserRegisterModel registerUserDto)
        {
            try
            {
                var user = await _userService.RegisterUserAsync(registerUserDto);
                var token = _jwtTokenGenerator.GenerateToken(user.Id.ToString(), user.Email);
                return Ok(new { Token = token });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginUser([FromBody] LoginCredentials loginUserDto)
        {
            try
            {
                var user = await _userService.LoginUserAsync(loginUserDto);
                var token = _jwtTokenGenerator.GenerateToken(user.Id.ToString(), user.Email);
                return Ok(new { Token = token });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            try
            {
                Request.Headers.TryGetValue("Authorization", out StringValues authorization);
                if (string.IsNullOrWhiteSpace(authorization))
                {
                    return Ok(new { Status = "Failed", Message = "Authorization header missing" });
                }

                string bearerToken = authorization.ToString().Replace("Bearer ", string.Empty, StringComparison.InvariantCultureIgnoreCase);

                _tokenLifetimeManager.SignOut(new JwtSecurityToken(bearerToken));

                return Ok(new { Status = "Success", Message = "Logged out successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Status = "Error", Message = ex.Message });
            }
        }


        [HttpGet("profile")]
        [Authorize]
        public async Task<IActionResult> GetProfile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Unauthorized();
            }

            var user = await _userService.GetUserByIdAsync(Guid.Parse(userId));
            if (user == null)
            {
                return NotFound();
            }

            var userDto = new UserDto
            {
                Id = user.Id,
                CreateTime = user.CreateTime,
                FullName = user.FullName,
                BirthDate = user.BirthDate,
                Gender = user.Gender,
                Email = user.Email,
                phoneNumber = user.phoneNumber
            };

            return Ok(userDto);
        }

        [HttpPut("profile")]
        [Authorize]
        public async Task<IActionResult> EditProfile([FromBody] UserEditModel editProfileDto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Unauthorized();
            }

            try
            {
                var user = await _userService.EditUserProfileAsync(Guid.Parse(userId), editProfileDto);
                var userDto = new UserDto
                {
                    Id = user.Id,
                    CreateTime = user.CreateTime,
                    FullName = user.FullName,
                    BirthDate = user.BirthDate,
                    Gender = user.Gender,
                    Email = user.Email,
                    phoneNumber = user.phoneNumber
                };

                return Ok(userDto);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }

}
