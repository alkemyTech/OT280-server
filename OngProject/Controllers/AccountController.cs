using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OngProject.Core.Models;
using OngProject.Core.Models.DTOs;
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

        public AccountController(UserManager<Users> userManager, SignInManager<Users> signInManager, IMapper mapper, IConfiguration configuration, IEmailService emailService,
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

        /// <summary>
        /// Login de usuario registrado al sistema. Rol: admin | standard
        /// </summary>
        /// <returns>Token de acceso</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /auth/login
        ///     {
        ///         "email": "user@example.com",
        ///         "password": "Password@123"
        ///     }
        ///
        /// </remarks>
        /// <response code="200">Solicitud concretada con exito</response>
        /// <response code="401">Credenciales no válidas</response>
        [HttpPost("login")]
        public async Task<ActionResult<UserToken>> Login([FromBody]LoginUserDto user)
        {
            // var user = _userManager.Users.SingleOrDefault(x => x.Email == login.Email);
            //
            // if (user == null) return Unauthorized("Username or password incorrect");
            //
            // var result = await _signInManager.CheckPasswordSignInAsync(user, login.Password, false);
            //
            // if (!result.Succeeded) return Unauthorized();
            //
            // return Ok(user);

            var result = await _signInManager.PasswordSignInAsync(user.Email, user.Password,
                isPersistent: false, lockoutOnFailure: false);
            
            if (result.Succeeded)
            {
                return await BuildToken(user.Email);
            }
            else
            {
                return BadRequest("Inicio de sesión no válido");
            }
        }

        /// <summary>
        /// Registro de nuevo usuario standard al sistema.
        /// </summary>
        /// <returns>JSON del usuario registrado</returns>
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
        /// <response code="200">Solicitud concretada con exito</response>
        /// <response code="401">Credenciales no válidas</response>
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
                _emailService.SendWelcome(newUser.Email);
                return await BuildToken(newUser.Email);
            }
            else
            {
                return BadRequest(result.Errors);
            }           
        }

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
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Email, email)
            };

            var users = await _userManager.FindByEmailAsync(email);

            
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
