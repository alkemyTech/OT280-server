using AutoMapper;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using OngProject.Core.Models;
using OngProject.Core.Models.DTOs;
using OngProject.Services.Interfaces;

using System.Linq;
using System.Threading.Tasks;
using System.Text;
using System.Security.Cryptography;

namespace OngProject.Controllers
{
    [Route("auth/")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        public readonly ITokenService _tokenService;
        private readonly UserManager<Users> _userManager;
        private readonly SignInManager<Users> _signInManager;
        private readonly IMapper _mapper;


        public AccountController(ITokenService tokenService, UserManager<Users> userManager,
            SignInManager<Users> signInManager,
            IMapper mapper)
        {
            _tokenService = tokenService;
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
        }

        [HttpPost("login")]
        public async Task<ActionResult<Users>> Login(LoginUserDTO login)
        {
            var user = _userManager.Users.SingleOrDefault(x => x.Email == login.Email);

            if (user == null) return Unauthorized("Username or password incorrect");

            var result = await _signInManager.CheckPasswordSignInAsync(user, login.Password, false);

            if (!result.Succeeded) return Unauthorized();

            return Ok(user);
        }

        //Criterios de aceptación:
        //La contraseña debe ser encriptada.
        //Deberá devolver como respuesta el usuario generado.
        //Encriptar contraseña
        [HttpPost("register")]
        public async Task<ActionResult<RegisterUserDTO>> Register(Users user)
        {
            if (UserExists(user.Email)) return BadRequest("Email already taken");

            // Ver hmac.ComputeHash
            // PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(user.Password)),
            // PasswordSalt = hmac.Key

            var newUser = _mapper.Map<Users>(user);

            string hashedPassword = ComputeSha256Hash(user.Password);
            newUser.UserName = user.Email;

            var result = await _userManager.CreateAsync(newUser, user.Password);
            //var result = await _userManager.CreateAsync(newUser, hashedPassword);

            if (!result.Succeeded) return BadRequest(result.Errors);

            //Ask for the roles
            //var roleResult = await _userManager.AddToRoleAsync(newUser, "User");

            //if (!roleResult.Succeeded) return BadRequest(roleResult.Errors);

            var token = await _tokenService.CreateToken(newUser);

            var userDTO = _mapper.Map<RegisterUserDTO>(newUser);
            userDTO.Token = token;

            return Ok(userDTO);
        }

        private bool UserExists(string email)
        {
            return _userManager.Users.Any(u => u.Email == email);
        }
        private string ComputeSha256Hash(string rawData)
        {
            // Create a SHA256   
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array  
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                // Convert byte array to a string   
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}
