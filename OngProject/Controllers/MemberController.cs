using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OngProject.Core.Models;
using OngProject.Core.Models.DTOs;
using OngProject.Repositories.Interfaces;
using OngProject.Services.Interfaces;
using System.Collections.Generic;
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

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<MemberDTO>))]
        public async Task<IActionResult> GetAll()
        {
            var members = await _memberService.GetAllAsync();
            var membersDTO = _mapper.Map<IEnumerable<MemberDTO>>(members);

            return new OkObjectResult(membersDTO);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(MemberDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            var member = await _memberService.GetById(id);

            if (member == null)
            {
                return NotFound();
            }

            var memberDTO = _mapper.Map<MemberDTO>(member);

            return new OkObjectResult(memberDTO);
        }

        [HttpPut]
        public async Task<IActionResult> Update(EditMemberDTO editMemberDTO)
        {
            var entity = await _memberService.GetById(editMemberDTO.MemberId);

            if (entity == null)
            {
                return NotFound();
            }

            var member = await _memberService.UpdateMember(entity, editMemberDTO);
            var memberDTO = _mapper.Map<EditMemberDTO>(entity);
            //await _memberService.Update(member);
            _unitOfWork.Commit();

            return new OkObjectResult(memberDTO);

        }
    }
}
