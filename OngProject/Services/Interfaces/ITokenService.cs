using OngProject.Core.Models;
using OngProject.Core.Models.DTOs;
using System.Threading.Tasks;

namespace OngProject.Services.Interfaces
{
    public interface ITokenService
    {
        Task<string> CreateToken(Users user);
        //Task<string> CreateToken(RegisterUserDTO newUser);
    }
}

