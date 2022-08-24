using OngProject.Core.Models.DTOs;
using OngProject.Core.Models;
using System.Threading.Tasks;

namespace OngProject.Services.Interfaces
{
    public interface IRoleService : IGenericService<Roles>
    {
        Task<Roles> UpdateRole(Roles role, EditRoleDTO editRoleDTO);
    }
}
