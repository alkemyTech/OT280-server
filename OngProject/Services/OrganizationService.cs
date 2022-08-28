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
            organization.name = editOrganizationDTO.Name;
            organization.image = editOrganizationDTO.Image;
            organization.address = editOrganizationDTO.Address;
            organization.phone = editOrganizationDTO.Phone;
            organization.email = editOrganizationDTO.Email;
            organization.welcomeText = editOrganizationDTO.WelcomeText;
            organization.aboutUsText = editOrganizationDTO.AboutUsText;

            await _organizationRepository.Update(organization);
        }
    }
}
