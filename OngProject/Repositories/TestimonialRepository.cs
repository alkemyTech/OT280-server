using OngProject.Core.Models;
using OngProject.Repositories.Interfaces;

namespace OngProject.Repositories
{
    public class TestimonialRepository : GenericRepository<Testimonials>, ITestimonialRepository
    {
        public TestimonialRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}
