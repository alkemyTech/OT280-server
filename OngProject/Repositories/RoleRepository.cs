using OngProject.Core.Models;
using OngProject.Repositories.Interfaces;

namespace OngProject.Repositories
{
    public class RoleRepository : GenericRepository<Roles>, IRoleRepository
    {
        public RoleRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }
    }
}
