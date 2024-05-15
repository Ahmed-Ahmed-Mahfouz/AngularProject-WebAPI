using AngularProject.Configurations;
using AngularProject.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AngularProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthManagementController : ControllerBase
    {
        private readonly ILogger<AuthManagementController> _logger;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly JwtConfig _jwtConfig;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AuthManagementController(
            ILogger<AuthManagementController> logger,
            UserManager<IdentityUser> userManager,
            IOptionsMonitor<JwtConfig> _optionsMonitor,
            RoleManager<IdentityRole> roleManager)
        {
            _logger = logger;
            _userManager = userManager;
            _jwtConfig = _optionsMonitor.CurrentValue;
            _roleManager = roleManager;
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] UserRegistrationRequestDTO requestDTO)
        {
            if (ModelState.IsValid)
            {
                var emailExist = await _userManager.FindByEmailAsync(requestDTO.Email);

                if (emailExist != null)
                    return BadRequest("Email already exists");

                if (string.IsNullOrEmpty(requestDTO.Name))
                    return BadRequest("Name is required.");


                var newUser = new IdentityUser()
                {
                    Email = requestDTO.Email,
                    UserName = requestDTO.Name
                };

                var isCreated = await _userManager.CreateAsync(newUser, requestDTO.Password);

                if (isCreated.Succeeded)
                {
                    var defaultRole = "User";

                    if(requestDTO.Email == "admin@example.com")
                    {
                        defaultRole = "Admin";
                    }

                    await _userManager.AddToRoleAsync(newUser, defaultRole);

                    var roles = await _userManager.GetRolesAsync(newUser);

                    var jwtToken = GenerateJwtToken(newUser, roles);

                    return Ok(new RegistrationRequestResponse()
                    {
                        Result = true,
                        Token = jwtToken
                    });
                }
                return BadRequest(isCreated.Errors.Select(x => x.Description).ToList());
            }

            return BadRequest("Invalid payload");
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] UserLoginRequestDTO requestDTO)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(requestDTO.Email);

                if (user != null && await _userManager.CheckPasswordAsync(user, requestDTO.Password))
                {
                    var roles = await _userManager.GetRolesAsync(user);

                    var jwtToken = GenerateJwtToken(user, roles);

                    return Ok(new LoginRequestResponse()
                    {
                        Result = true,
                        Token = jwtToken,
                        UserName = user.UserName
                    });
                }

                return BadRequest("Invalid authentication");
            }

            return BadRequest("Invalid payload");
        }

        private string GenerateJwtToken(IdentityUser user, IList<string> roles)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtConfig.Secret);

            var claims = new List<Claim>
    {
        new Claim("Id", user.Id),
        new Claim(JwtRegisteredClaimNames.Email, user.Email),
        new Claim(JwtRegisteredClaimNames.Sub, user.Email),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
    };

            // Add a claim for each role
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(6),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512)
            };

            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            var jwtToken = jwtTokenHandler.WriteToken(token);

            return jwtToken;
        }

    }
}
