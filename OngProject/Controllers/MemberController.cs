using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OngProject.Core.Models;
using OngProject.Core.Models.DTOs;
using OngProject.Repositories.Interfaces;
using OngProject.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OngProject.Core.Helper;
using Swashbuckle.AspNetCore.Annotations;
using Microsoft.AspNetCore.Authorization;

namespace OngProject.Controllers
{
    [Authorize(Roles = "admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class MemberController : ControllerBase
    {
        private readonly IMemberService _memberService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public MemberController(IMemberService memberService, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _memberService = memberService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        
        #region Documentacion
        [SwaggerOperation(Summary = "List of all Members.", Description = "Requires admin privileges.")]
        [SwaggerResponse(200, "Success. Returns a list of existing Members.")]
        [SwaggerResponse(401, "Unauthenticated user or wrong jwt token.")]
        [SwaggerResponse(403, "Unauthorized user.")]
        [SwaggerResponse(500, "Internal server error. An error occurred while processing your request.")]
        #endregion
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<MemberDTO>))]
        public async Task<IActionResult> GetAll([FromQuery] PaginacionDto paginacionDto)
        {
            //Paginacion
            var queryable = _unitOfWork.Context.Members.AsQueryable();
            await HttpContext.InsertarParametrosPaginacion(queryable, paginacionDto.CantidadRegistroPorPagina);
                
            //var members = await _memberService.GetAllAsync();
            var members = await queryable.Paginar(paginacionDto).ToListAsync();
            var membersDto = _mapper.Map<IEnumerable<MemberDTO>>(members);

            return new OkObjectResult(membersDto);
        }
        
        #region Documentacion
        [SwaggerOperation(Summary = "Get member details by id.", Description = ".")]
        [SwaggerResponse(200, "Success. Returns the slide details.")]
        [SwaggerResponse(401, "Unauthenticated user or wrong jwt token.")]
        [SwaggerResponse(404, "NotFound. Entity id not found.")]
        [SwaggerResponse(500, "Internal server error. An error occurred while processing your request.")]
        #endregion
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

            var memberDto = _mapper.Map<MemberDTO>(member);

            return new OkObjectResult(memberDto);
        }

        #region Documentacion
        [SwaggerOperation(Summary = "Create Member.",Description = "Requires user or admin privileges.")]
        [SwaggerResponse(200, "Created. Returns the id of the created object.")]
        [SwaggerResponse(400, "BadRequest. Object not created, try again.")]
        [SwaggerResponse(401, "Unauthenticated or wrong jwt token.")]
        [SwaggerResponse(403, "Unauthorized user.")]
        [SwaggerResponse(500, "Internal server error. An error occurred while processing your request.")]
        #endregion
        [HttpPost]
        public async Task<IActionResult> Create(MemberDTO memberDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var member = _mapper.Map<Members>(memberDto);
            var created = await _memberService.CreateAsync(member);

            if (created)
                _unitOfWork.Commit();

            return Created("Created", new { Response = StatusCode(201) });
        }
        
        #region Documentacion
        [SwaggerOperation(Summary = "Soft delete an existing Member.", Description = "Requires admin privileges.")]
        [SwaggerResponse(204, "Deleted. Returns nothing.")]
        [SwaggerResponse(401, "Unauthenticated user or wrong jwt token.")]
        [SwaggerResponse(403, "Unauthorized user.")]
        [SwaggerResponse(404, "NotFound. Entity id not found.")]
        [SwaggerResponse(500, "Internal server error. An error occurred while processing your request.")]
        #endregion
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

        #region Documentacion
        [SwaggerOperation(Summary = "Modifies an existing Member.", Description = ".")]
        [SwaggerResponse(204, "Updated. Returns nothing.")]
        [SwaggerResponse(400, "BadRequest. Something went wrong, try again.")]
        [SwaggerResponse(401, "Unauthenticated or wrong jwt token.")]
        [SwaggerResponse(404, "NotFound. Entity id not found.")]
        [SwaggerResponse(500, "Internal server error. An error occurred while processing your request.")]
        #endregion
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(EditMemberDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(EditMemberDTO editMemberDto)
        {
            var entity = await _memberService.GetById(editMemberDto.MemberId);

            if (entity == null)
            {
                return NotFound();
            }

            await _memberService.UpdateMember(entity, editMemberDto);
            var memberDto = _mapper.Map<EditMemberDTO>(entity);
            _unitOfWork.Commit();

            return new OkObjectResult(memberDto);

        }
    }
}
