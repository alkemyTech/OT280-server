﻿using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OngProject.Core.Models.DTOs;
using OngProject.Repositories.Interfaces;
using OngProject.Services.Interfaces;
using SendGrid.Helpers.Mail;
using System.Linq;
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
            var organization = await _organizationService.GetByIdSlides(id);

            organization.Slides.OrderByDescending(o => o.Order);

            if (organization == null)
                return NotFound();

            var organizationDTO = _mapper.Map<OrganizationDTO>(organization);

            return new OkObjectResult(organizationDTO);
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost("public")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(EditOrganizationDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(EditOrganizationDTO editOrganizationDTO)
        {
            var entity = await _organizationService.GetById(editOrganizationDTO.OrganizationId);

            if (ModelState.IsValid && entity != null)
            {
                _organizationService.UpdateOrganization(entity, editOrganizationDTO);
                _unitOfWork.Commit();

                return new OkObjectResult(editOrganizationDTO);
            }

            return NotFound();
        }
    }
}
