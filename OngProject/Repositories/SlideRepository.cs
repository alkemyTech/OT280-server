using OngProject.Core.Models;
using OngProject.Repositories.Interfaces;

namespace OngProject.Repositories
{
    public class SlideRepository : GenericRepository<Slide>,ISlideRepository
    {
        public SlideRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }
    }
}
