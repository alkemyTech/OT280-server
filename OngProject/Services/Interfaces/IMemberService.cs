using OngProject.Core.Models;
using OngProject.Core.Models.DTOs;
using System.Threading.Tasks;

namespace OngProject.Services.Interfaces
{
    public interface IMemberService: IGenericService<Members>
    {
        Task<Members> UpdateMember(Members member, EditMemberDTO editMemberDTO);
    }
}
