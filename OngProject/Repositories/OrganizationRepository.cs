using Microsoft.EntityFrameworkCore;
using OngProject.Core.Models;
using OngProject.Repositories.Interfaces;
using System.Threading.Tasks;

namespace OngProject.Repositories
{
    public class OrganizationRepository : GenericRepository<Organization>, IOrganizationRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        public OrganizationRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Organization> GetByIdSlides(int id)
        {
            return await _unitOfWork.Context.Set<Organization>()
                .Include(e => e.Slides)
                .FirstOrDefaultAsync(e => e.OrganizationId == id);
        }
    }
}
