using OngProject.Core.Models;
using OngProject.Repositories.Interfaces;
using OngProject.Services.Interfaces;

namespace OngProject.Services
{
    public class MemberService : GenericService<Members>, IMemberService
    {
        private IMemberRepository _memberRepository;
        public MemberService(IMemberRepository memberRepository) : base(memberRepository)
        {
            this._memberRepository = memberRepository;
        }
    }
}
