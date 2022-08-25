using OngProject.Core.Models;
using OngProject.Repositories.Interfaces;

namespace OngProject.Repositories
{
    public class ActivityRepository : GenericRepository<Activities>, IActivityRepository
    {
        public ActivityRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }
    }
}
