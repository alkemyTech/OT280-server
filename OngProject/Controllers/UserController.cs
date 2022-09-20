using Microsoft.AspNetCore.Mvc;
using OngProject.Core.Models;
using OngProject.Repositories.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using OngProject.Core.Models.DTOs.Account;
using Swashbuckle.AspNetCore.Annotations;

namespace OngProject.Controllers
{
    [Authorize(Roles = "admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IGenericRepository<Users> _genericRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UserController(IGenericRepository<Users> genericRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _genericRepository = genericRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        #region Documentation
        [SwaggerOperation(Summary = "List of all Users.", Description = "Requires admin privileges.")]
        [SwaggerResponse(200, "Success. Returns a list of existing Users.")]
        [SwaggerResponse(401, "Unauthenticated user or wrong jwt token.")]
        [SwaggerResponse(403, "Unauthorized user.")]
        [SwaggerResponse(500, "Internal server error. An error occurred while processing your request.")]
        #endregion
        [HttpGet]
        public async Task<IEnumerable<Users>> Get()
        {
            return await _genericRepository.GetAllAsync();
        }
        

        #region Documentation
        [SwaggerOperation(Summary = "Get user details by id.", Description = "Requires admin privileges.")]
        [SwaggerResponse(200, "Success. Returns the user details.")]
        [SwaggerResponse(401, "Unauthenticated user or wrong jwt token.")]
        [SwaggerResponse(403, "Unauthorized user.")]
        [SwaggerResponse(404, "NotFound. Entity id not found.")]
        [SwaggerResponse(500, "Internal server error. An error occurred while processing your request.")]
        #endregion
        [HttpGet("{id}")]
        public async Task<IEnumerable<Users>> GetById(string id)
        {
            var user = await _genericRepository.GetById(id);
            if (user == null)
                return (IEnumerable<Users>)NotFound();
            return (IEnumerable<Users>)Ok(user);
        }
        
        #region Documentation
        [SwaggerOperation(Summary = "Create Users", Description = "Requires admin privileges")]
        [SwaggerResponse(200, "Created. Returns the id of the created object.")]
        [SwaggerResponse(400, "BadRequest. Object not created, try again.")]
        [SwaggerResponse(401, "Unauthenticated or wrong jwt token.")]
        [SwaggerResponse(403, "Unauthorized user.")]
        [SwaggerResponse(500, "Internal server error. An error occurred while processing your request.")]
        #endregion
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Users users)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var created = await _genericRepository.CreateAsync(users);

            if (created)
                _unitOfWork.Commit();

            return Created("Created", new { Response = StatusCode(201) });
        }
        
        #region Documentation
        [SwaggerOperation(Summary = "Modifies an existing Slide.", Description = "Requires user privileges.")]
        [SwaggerResponse(204, "Updated. Returns nothing.")]
        [SwaggerResponse(400, "BadRequest. Something went wrong, try again.")]
        [SwaggerResponse(401, "Unauthenticated or wrong jwt token.")]
        [SwaggerResponse(404, "NotFound. Entity id not found.")]
        [SwaggerResponse(500, "Internal server error. An error occurred while processing your request.")]
        #endregion
        [Authorize(Roles = "admin, standard")]
        [HttpPatch("{id}")]
        public async Task<ActionResult> Patch(string id, [FromBody] JsonPatchDocument<UpdateUserDto> patchDocument)
        {
            var user = await _genericRepository.GetById(id);
        
            if (user is null )
                return NotFound("Usuario no encontrado");

            var userDto = _mapper.Map<UpdateUserDto>(user);
            patchDocument.ApplyTo(userDto);

            await _genericRepository.Update(_mapper.Map(userDto, user));
            _unitOfWork.Commit();

            return NoContent();
        }
        
        #region Documentation
        [SwaggerOperation(Summary = "Soft Delete of an existing User", Description = "Requires admin privileges")]
        [SwaggerResponse(200, "Success. Returns nothing")]
        [SwaggerResponse(401, "Unauthenticated user or wrong jwt token")]
        [SwaggerResponse(403, "Unauthorized user")]
        [SwaggerResponse(404, "NotFound. Entity id not found.")]
        [SwaggerResponse(500, "Internal server error. An error occurred while processing your request.")]
        #endregion
        [Authorize(Roles = "admin, standard")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _genericRepository.GetById(id);

            if (user == null)
                return BadRequest();

            await _genericRepository.DeleteAsync(user);
            _unitOfWork.Commit();

            return Ok(user);
        }
    }
}
