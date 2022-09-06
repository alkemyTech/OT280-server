using OngProject.Core.Models;
using OngProject.Repositories.Interfaces;
using OngProject.Services.Interfaces;

namespace OngProject.Services
{
    public class UserService : GenericService<Users>, IUserService
    {
        private IUserRepository _userRepository;
        public UserService(IUserRepository userRepository) : base(userRepository)
        {
            this._userRepository = userRepository;
        }

    }
}
