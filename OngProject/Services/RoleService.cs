using OngProject.Core.Models.DTOs;
using OngProject.Core.Models;
using OngProject.Repositories.Interfaces;
using System.Threading.Tasks;
using OngProject.Services.Interfaces;

namespace OngProject.Services
{
    public class RoleService : GenericService<Roles>, IRoleService
    {
        private IRoleRepository _roleRepository;
        public RoleService(IRoleRepository roleRepository) : base(roleRepository)
        {
            this._roleRepository = roleRepository;
        }

        public async Task<Roles> UpdateRole(Roles role, EditRoleDTO editRoleDTO)
        {
            /*
            role.name = editRoleDTO.name;
            role.facebookUrl = editRoleDTO.facebookUrl;
            role.instagramUrl = editRoleDTO.instagramUrl;
            role.linkedinUrl = editRoleDTO.linkedinUrl;
            role.image = editRoleDTO.image;
            role.description = editRoleDTO.description;
            */

            role.Name = editRoleDTO.Name;
            role.Description = editRoleDTO.Description;
            role.NormalizedName = editRoleDTO.Name;

            await _roleRepository.Update(role);

            return role;
        }
    }
}
