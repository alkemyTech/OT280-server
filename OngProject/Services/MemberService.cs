using OngProject.Core.Models;
using OngProject.Core.Models.DTOs;
using OngProject.Repositories.Interfaces;
using OngProject.Services.Interfaces;
using System.Threading.Tasks;

namespace OngProject.Services
{
    public class MemberService : GenericService<Members>, IMemberService
    {
        private IMemberRepository _memberRepository;
        public MemberService(IMemberRepository memberRepository) : base(memberRepository)
        {
            this._memberRepository = memberRepository;
        }

        public async Task<Members> UpdateMember(Members member, EditMemberDTO editMemberDTO)
        {            
            member.name = editMemberDTO.name;
            member.facebookUrl = editMemberDTO.facebookUrl;
            member.instagramUrl = editMemberDTO.instagramUrl;
            member.linkedinUrl = editMemberDTO.linkedinUrl;
            member.image = editMemberDTO.image;
            member.description = editMemberDTO.description;

            await _memberRepository.Update(member);

            return member;
        }
    }
}
