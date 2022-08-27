using OngProject.Core.Models;
using OngProject.Core.Models.DTOs;

namespace OngProject.Services.Interfaces
{
    public interface IOrganizationService : IGenericService<Organization>
    {
        void UpdateOrganization(Organization organization, EditOrganizationDTO editOrganizationDTO);
    }
}
