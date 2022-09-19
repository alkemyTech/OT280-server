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
        
        /// <summary>
        /// Endpoint para obtener la lista de los Members existentes.Se debe ser usuario ADMINISTRADOR/STANDARD 
        /// </summary>
        /// <response code="200">Solicitud concretada con exito</response>
        /// <response code="401">Credenciales no validas</response>
        
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
            var membersDTO = _mapper.Map<IEnumerable<MemberDTO>>(members);

            return new OkObjectResult(membersDTO);
        }
        
        #region Documentacion
        
        /// <summary>
        /// Endpoint para obtener un member por id.Se debe ser ADMINISTRADOR 
        /// </summary>
        /// <response code="200">Solicitud concretada con exito</response>
        /// <response code="401">Credenciales no validas</response>
        
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

            var memberDTO = _mapper.Map<MemberDTO>(member);

            return new OkObjectResult(memberDTO);
        }

        #region Documentacion
        
        /// <summary>
        /// Endpoint para el manejo de la creacion de Members.Se debe ser ADMINISTRADOR 
        /// </summary>
        /// <response code="200">Solicitud concretada con exito</response>
        /// <response code="401">Credenciales no validas</response>
        
        #endregion
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
        
        #region Documentacion
        
        /// <summary>
        /// Endpoint que borra un member que se busca por su id.Se debe ser ADMINISTRADOR 
        /// </summary>
        /// <response code="200">Solicitud concretada con exito</response>
        /// <response code="401">Credenciales no validas</response>
        
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
        
        /// <summary>
        /// Endpoint para actualizar un member que se consigue al buscarlo por id.Se debe ser ADMINISTRADOR 
        /// </summary>
        /// <response code="200">Solicitud concretada con exito</response>
        /// <response code="401">Credenciales no validas</response>
        
        #endregion
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(EditMemberDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(EditMemberDTO editMemberDTO)
        {
            var entity = await _memberService.GetById(editMemberDTO.MemberId);

            if (entity == null)
            {
                return NotFound();
            }

            var member = await _memberService.UpdateMember(entity, editMemberDTO);
            var memberDTO = _mapper.Map<EditMemberDTO>(entity);
            _unitOfWork.Commit();

            return new OkObjectResult(memberDTO);

        }
    }
}
