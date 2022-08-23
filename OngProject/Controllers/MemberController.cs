using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OngProject.Core.Models;
using OngProject.Core.Models.DTOs;
using OngProject.Repositories.Interfaces;
using OngProject.Services.Interfaces;
using System.Threading.Tasks;

namespace OngProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MemberController : ControllerBase
    {
        private readonly IMemberService _memberService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public MemberController(IMemberService memberService, IUnitOfWork unitOfWork, IMapper mapper)
        {
            this._memberService = memberService;
            this._unitOfWork = unitOfWork;
            this._mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> Create(MemberDTO member)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var _member = _mapper.Map<Members>(member);
            var created = await _memberService.CreateAsync(_member);

            if (created)
                _unitOfWork.Commit();

            return Created("Created", new { Response = StatusCode(201) });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var member = await _memberService.GetById(id);

            if (member == null)
                return BadRequest();

            await _memberService.DeleteAsync(member);
            _unitOfWork.Commit();

            return Ok(member);
        }

    }
}
