using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OngProject.Core.Models.DTOs;
using OngProject.Repositories.Interfaces;
using OngProject.Services.Interfaces;
using System.Threading.Tasks;
using Swashbuckle.AspNetCore.Annotations;

namespace OngProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrganizationController : Controller
    {
        private readonly IOrganizationService _organizationService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public OrganizationController(IOrganizationService organizationService, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _organizationService = organizationService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        #region Documentation
        [SwaggerOperation(Summary = "Get details of the organization by id", Description = "Requires admin privileges")]
        [SwaggerResponse(200, "Success. Returns the organization details.")]
        [SwaggerResponse(401, "Unauthenticated user or wrong jwt token.")]
        [SwaggerResponse(403, "Unauthorized user.")]
        [SwaggerResponse(404, "NotFound. Entity id not found.")]
        [SwaggerResponse(500, "Internal server error. An error occurred while processing your request.")]
        #endregion
        [HttpGet("public/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OrganizationDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            var organization = await _organizationService.GetById(id);

            if (organization == null)
            {
                return NotFound();
            }

            var organizationDto = _mapper.Map<OrganizationDTO>(organization);

            return new OkObjectResult(organizationDto);
        }
        
        #region Documentation
        [SwaggerOperation(Summary = "Creates a new Organization", Description = "Requires admin privileges")]
        [SwaggerResponse(200, "Success. Returns nothing")]
        [SwaggerResponse(400, "BadRequest. Object not created, try again.")]
        [SwaggerResponse(401, "Unauthenticated or wrong jwt token.")]
        [SwaggerResponse(403, "Unauthorized user.")]
        [SwaggerResponse(500, "Internal server error. An error occurred while processing your request.")]
        #endregion
        [Authorize(Roles = "admin")]
        [HttpPost("public")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(EditOrganizationDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(EditOrganizationDTO editOrganizationDto)
        {
            var entity = await _organizationService.GetById(editOrganizationDto.OrganizationId);

            if (ModelState.IsValid && entity != null)
            {
                _organizationService.UpdateOrganization(entity, editOrganizationDto);
                _unitOfWork.Commit();

                return new OkObjectResult(editOrganizationDto);
            }

            return NotFound();
        }
    }
}
