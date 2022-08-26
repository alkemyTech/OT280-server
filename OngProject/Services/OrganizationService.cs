using OngProject.Core.Models;
using OngProject.Repositories.Interfaces;
using OngProject.Services.Interfaces;

namespace OngProject.Services
{
    public class OrganizationService : GenericService<Organization>, IOrganizationService
    {
        private IOrganizationRepository _organizationRepository;
        public OrganizationService(IOrganizationRepository organizationRepository) : base(organizationRepository)
        {
            this._organizationRepository = organizationRepository;
        }
    }
}
