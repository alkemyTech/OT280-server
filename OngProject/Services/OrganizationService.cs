using OngProject.Core.Models;
using OngProject.Core.Models.DTOs;
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

        public async void UpdateOrganization(Organization organization, EditOrganizationDTO editOrganizationDTO)
        {
            organization.Name = editOrganizationDTO.Name;
            organization.Image = editOrganizationDTO.Image;
            organization.Address = editOrganizationDTO.Address;
            organization.Phone = editOrganizationDTO.Phone;
            organization.Email = editOrganizationDTO.Email;
            organization.WelcomeText = editOrganizationDTO.WelcomeText;
            organization.AboutUsText = editOrganizationDTO.AboutUsText;

            await _organizationRepository.Update(organization);
        }
    }
}
