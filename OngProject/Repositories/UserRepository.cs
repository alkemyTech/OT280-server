using OngProject.Core.Models;
using OngProject.Repositories.Interfaces;

namespace OngProject.Repositories
{
    public class UserRepository : GenericRepository<Users>, IUserRepository
    {
        public UserRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}