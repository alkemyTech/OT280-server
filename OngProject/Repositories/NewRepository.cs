using OngProject.Core.Models;
using OngProject.Repositories.Interfaces;

namespace OngProject.Repositories
{
    public class NewRepository : GenericRepository<News>, INewRepository
    {
        public NewRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}
