using OngProject.Core.Models;
using System.Threading.Tasks;

namespace OngProject.Repositories.Interfaces
{
    public interface IOrganizationRepository : IGenericRepository<Organization>
    {
        Task<Organization> GetByIdSlides(int id);
    }
}
