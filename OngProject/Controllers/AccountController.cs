using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OngProject.Core.Models;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using OngProject.Core.Models.DTOs.Account;
using OngProject.Repositories.Interfaces;
using OngProject.Services.Interfaces;
using Swashbuckle.AspNetCore.Annotations;


namespace OngProject.Controllers
{
    [Route("auth/")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<Users> _userManager;
        private readonly SignInManager<Users> _signInManager;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _accessor;
        private readonly IEmailService _emailService;

        public AccountController(UserManager<Users> userManager, SignInManager<Users> signInManager, IMapper mapper, 
            IConfiguration configuration, IEmailService emailService,
             IUnitOfWork unitOfWork, IHttpContextAccessor accessor) 
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
            _configuration = configuration;
            _unitOfWork = unitOfWork;
            _accessor = accessor;
            _emailService = emailService;
        }

        #region Sample Resquest
        ///<remarks>
        /// Sample request:
        ///
        ///     POST /auth/register
        ///     {
        ///         "userName": "marita",
        ///         "firstName": "Marita",
        ///         "lastName": "Gómez",
        ///         "email": "maritagomez@gmail.com",
        ///         "passwordHash": "Password@123"
        ///     }
        ///
        /// </remarks>
        #endregion
        #region Documentation
        [SwaggerOperation(Summary = "User Login")]
        [SwaggerResponse(200, "Logged in. Returns Token")]
        [SwaggerResponse(400, "BadRequest. Something went wrong, try again.")]
        [SwaggerResponse(500, "Internal server error. An error occurred while processing your request.")]
        #endregion
        [HttpPost("login")]
        public async Task<ActionResult<UserToken>> Login([FromBody]LoginUserDto user)
        {
            var users = _userManager.Users.SingleOrDefault(x => x.Email == user.Email);
            
            if (user == null) return Unauthorized("Username or password incorrect");
            
            var result = await _signInManager.CheckPasswordSignInAsync(users, user.Password, false);

            if (result.Succeeded)
            {
                return await BuildToken(user.Email);
            }
            else
            {
                return BadRequest("Inicio de sesión no válido");
            }
            
        }

        #region Sample Resquest
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /auth/register
        ///     {
        ///         "userName": "marita",
        ///         "firstName": "Marita",
        ///         "lastName": "Gómez",
        ///         "email": "maritagomez@gmail.com",
        ///         "passwordHash": "Password@123"
        ///     }
        ///
        /// </remarks>
        #endregion
        #region Documentation
        [SwaggerOperation(Summary = "Register user", Description = ".")]
        [SwaggerResponse(200, "Success. Returns a username.")]
        [SwaggerResponse(400, "BadRequest. Something went wrong, try again.")]
        [SwaggerResponse(500, "Internal server error. An error occurred while processing your request.")]
        #endregion
        [HttpPost("register")]
        public async Task<ActionResult<UserToken>> Register([FromBody] UserDto user)
        {
            if (UserExists(user.UserName)) 
                return BadRequest("UserName already taken");

            var newUser = new Users { 
                UserName = user.UserName, 
                Email = user.Email, 
                LastName= user.LastName, 
                FirstName = user.FirstName 
            };

            var result = await _userManager.CreateAsync(newUser, user.PasswordHash);

            if (result.Succeeded)
            {
                if (_emailService.IsConfigured())
                    _emailService.SendWelcome(newUser.Email);

                return await BuildToken(newUser.Email);
            }
            else
            {
                return BadRequest(result.Errors);
            }           
        }

        #region Documentation
        [SwaggerOperation(Summary = "Account details", Description = "Requires user or admin privileges.")]
        [SwaggerResponse(200, "Success. Return the account details.")]
        [SwaggerResponse(401, "Unauthenticated user or wrong jwt token.")]
        [SwaggerResponse(500, "Internal server error. An error occurred while processing your request.")]
        #endregion
        [HttpGet("me")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<CurrentUserDto>> Get()
        {
            var currentUserId = _accessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            var user = await _unitOfWork.Context.Users.FirstOrDefaultAsync(u => u.Id.Equals(currentUserId));

            if (user is null)
                return NotFound();
            
            return _mapper.Map<CurrentUserDto>(user);
        }

        private bool UserExists(string userName)
        {
            return _userManager.Users.Any(u => u.UserName == userName);
        }

        #region Token
        //Metodo de creacion del token
        private async Task<UserToken> BuildToken(string email)
        {
            var users = await _userManager.FindByEmailAsync(email);
            var userRoles = await _userManager.GetRolesAsync(users);
            
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.Name, users.UserName)
            };

            foreach (var userRole in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, userRole));
            }
            
            claims.Add(new Claim(ClaimTypes.NameIdentifier, users.Id));

            var claimsDb = await _userManager.GetClaimsAsync(users);
            
            claims.AddRange(claimsDb);

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["jwt:key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expiration = DateTime.UtcNow.AddYears(1);

            JwtSecurityToken token = new JwtSecurityToken(
                issuer: null,
                audience: null,
                claims: claims,
                expires: expiration,
                signingCredentials: creds
            );

            return new UserToken
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = expiration
            };

        }

        #endregion
    }
}
