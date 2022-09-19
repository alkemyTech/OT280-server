using OngProject.Core.Models;
using OngProject.Core.Models.DTOs;
using System.Threading.Tasks;

namespace OngProject.Services.Interfaces
{
    public interface IOrganizationService : IGenericService<Organization>
    {
        void UpdateOrganization(Organization organization, EditOrganizationDTO editOrganizationDTO);
        Task<Organization> GetByIdSlides(int id);
    }
}
