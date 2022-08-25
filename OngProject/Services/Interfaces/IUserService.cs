using OngProject.Core.Models;
using OngProject.Core.Models.DTOs;
using System.Threading.Tasks;

namespace OngProject.Services.Interfaces
{
    public interface IUserService : IGenericService<Users>
    {
        Task<Users> UpdateUser(Users user, UserUpdateDTO userDTO);
    }
}
