using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OngProject.Core.Models.DTOs;
using OngProject.Repositories.Interfaces;
using OngProject.Services.Interfaces;
using System.Threading.Tasks;

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

            var organizationDTO = _mapper.Map<OrganizationDTO>(organization);

            return new OkObjectResult(organizationDTO);
        }
    }
}
