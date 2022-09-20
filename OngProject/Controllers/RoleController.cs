using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OngProject.Core.Models;
using OngProject.Repositories.Interfaces;
using OngProject.Core.Models.DTOs;
using OngProject.Services.Interfaces;
using AutoMapper;
using Swashbuckle.AspNetCore.Annotations;

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
            _roleService = roleService;
            _genericRepository = genericRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        #region Documentation
        [SwaggerOperation(Summary = "List of all Roles.", Description = "Requires admin privileges.")]
        [SwaggerResponse(200, "Success. Returns a list of existing Roles.")]
        [SwaggerResponse(401, "Unauthenticated user or wrong jwt token.")]
        [SwaggerResponse(403, "Unauthorized user.")]
        [SwaggerResponse(500, "Internal server error. An error occurred while processing your request.")]
        #endregion
        [HttpGet]
        public async Task<IEnumerable<Roles>> Get()
        {
            return await _genericRepository.GetAllAsync();
        }
        
        #region Documentation
        [SwaggerOperation(Summary = "Get roles details by id.", Description = "Requires admin privileges.")]
        [SwaggerResponse(200, "Success. Returns the roles details.")]
        [SwaggerResponse(401, "Unauthenticated user or wrong jwt token.")]
        [SwaggerResponse(403, "Unauthorized user.")]
        [SwaggerResponse(404, "NotFound. Entity id not found.")]
        [SwaggerResponse(500, "Internal server error. An error occurred while processing your request.")]
        #endregion
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
        
        #region Documentation
        [SwaggerOperation(Summary = "Create Role", Description = "Requires admin privileges")]
        [SwaggerResponse(200, "Created. Returns the id of the created object.")]
        [SwaggerResponse(400, "BadRequest. Object not created, try again.")]
        [SwaggerResponse(401, "Unauthenticated or wrong jwt token.")]
        [SwaggerResponse(403, "Unauthorized user.")]
        [SwaggerResponse(500, "Internal server error. An error occurred while processing your request.")]
        #endregion
        [HttpPost]
        public async Task<IActionResult> Post(CreateRoleDTO roleDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var role = _mapper.Map<Roles>(roleDto);
           
            if (roleDto.RoleId != null) role.Id = roleDto.RoleId;
            role.NormalizedName = roleDto.Name;
            
            var created = await _genericRepository.CreateAsync(role);

            if (created)
                _unitOfWork.Commit();

            return Created("Created", new { Response = StatusCode(201) });
        }
        
        #region Documentation
        [SwaggerOperation(Summary = "Soft Delete of an existing Role", Description = "Requires admin privileges")]
        [SwaggerResponse(200, "Success. Returns nothing")]
        [SwaggerResponse(401, "Unauthenticated user or wrong jwt token")]
        [SwaggerResponse(403, "Unauthorized user")]
        [SwaggerResponse(404, "NotFound. Entity id not found.")]
        [SwaggerResponse(500, "Internal server error. An error occurred while processing your request.")]
        #endregion
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
        
        #region Documentation
        [SwaggerOperation(Summary = "Modifies an existing Roles.", Description = "Requires user privileges.")]
        [SwaggerResponse(204, "Updated. Returns nothing.")]
        [SwaggerResponse(400, "BadRequest. Something went wrong, try again.")]
        [SwaggerResponse(401, "Unauthenticated or wrong jwt token.")]
        [SwaggerResponse(404, "NotFound. Entity id not found.")]
        [SwaggerResponse(500, "Internal server error. An error occurred while processing your request.")]
        #endregion
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(EditRoleDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(EditRoleDTO editRoleDto)
        {
            var entity = await _roleService.GetById(editRoleDto.RoleId);

            if (entity == null)
            {
                return NotFound();
            }

            await _roleService.UpdateRole(entity, editRoleDto);
            var roleDto = _mapper.Map<EditRoleDTO>(entity);
            _unitOfWork.Commit();

            return new OkObjectResult(roleDto);
        }
    }
}
