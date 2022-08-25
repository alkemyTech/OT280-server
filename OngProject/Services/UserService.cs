using OngProject.Core.Models;
using OngProject.Core.Models.DTOs;
using System.Threading.Tasks;
using OngProject.Services.Interfaces;
using OngProject.Repositories.Interfaces;

namespace OngProject.Services
{
    public class UserService : GenericService<Users>, IUserService
    {
        private IUserRepository _userRepository;

        public UserService(IUserRepository userRepository) : base(userRepository)
        {
            this._userRepository = userRepository;
        }

        public async Task<Users> UpdateUser(Users user, UserUpdateDTO userDTO)
        {
            user.FirstName = userDTO.FirstName;
            user.LastName = userDTO.LastName;

            await _userRepository.Update(user);

            return user;
        }
    }
}