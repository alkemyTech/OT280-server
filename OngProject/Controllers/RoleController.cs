using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OngProject.Core.Models;
using OngProject.Repositories.Interfaces;
using OngProject.Core.Models.DTOs;
using OngProject.Services.Interfaces;
using AutoMapper;
using AutoMapper.Execution;

namespace OngProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;
        private readonly IGenericRepository<Roles> _genericRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;


        public RoleController(IRoleService roleService, IGenericRepository<Roles> genericRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            this._roleService = roleService;
            this._genericRepository = genericRepository;
            this._unitOfWork = unitOfWork;
            this._mapper = mapper;
        }

        [HttpGet]
        public async Task<IEnumerable<Roles>> Get()
        {
            return await _genericRepository.GetAllAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Roles>> GetById(string id)
        {
            var role = await _genericRepository.GetById(id);

            if (role == null)
            {
                return NotFound();
            }

            return role;
        }

        [HttpPost]
        public async Task<IActionResult> Post(CreateRoleDTO role)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var _role = _mapper.Map<Roles>(role);
           
            if (role.RoleId != null) _role.Id = role.RoleId;
            _role.NormalizedName = role.Name;
            
            var created = await _genericRepository.CreateAsync(_role);

            if (created)
                _unitOfWork.Commit();

            return Created("Created", new { Response = StatusCode(201) });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var role = await _genericRepository.GetById(id);

            if (role == null)
            {
                return NotFound();
            }

            var deleted = _genericRepository.Delete_(role);

            if (deleted)
                _unitOfWork.Commit();

            return Ok(role);
        }
        
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(EditRoleDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(EditRoleDTO editRoleDTO)
        {
            var entity = await _roleService.GetById(editRoleDTO.RoleId);

            if (entity == null)
            {
                return NotFound();
            }

            var role = await _roleService.UpdateRole(entity, editRoleDTO);
            var roleDTO = _mapper.Map<EditRoleDTO>(entity);
            _unitOfWork.Commit();

            return new OkObjectResult(roleDTO);
        }
    }
}
