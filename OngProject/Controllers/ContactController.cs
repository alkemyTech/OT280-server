using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OngProject.Core.Models;
using OngProject.Core.Models.DTOs.Contact;
using OngProject.Repositories.Interfaces;
using OngProject.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Swashbuckle.AspNetCore.Annotations;

namespace OngProject.Controllers
{
    [Route("contacts")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ContactController : Controller
    {
        private readonly IContactService _contactService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;

        public ContactController(IContactService contactService)
        {
            _contactService = contactService;
        }

        public ContactController(IContactService contactService, IUnitOfWork unitOfWork, IMapper mapper, IEmailService emailService)
        {
            _contactService = contactService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _emailService = emailService;
        }

        #region Documentation
        [SwaggerOperation(Summary = "Create a Contact.", Description = ".")]
        [SwaggerResponse(200, "Created.")]
        [SwaggerResponse(400, "BadRequest. Object not created, try again.")]
        [SwaggerResponse(401, "Unauthenticated or wrong jwt token.")]
        [SwaggerResponse(500, "Internal server error. An error occurred while processing your request.")]
        #endregion
        [HttpPost]
        public async Task<IActionResult> Create(ContactCreateDTO contactDto)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var newContact = _mapper.Map<Contact>(contactDto);
            var created = await _contactService.CreateAsync(newContact);

            if (created && _emailService.IsConfigured())
            {
                _emailService.SendSuccessContact(contactDto.Email);
                _unitOfWork.Commit();                                  
            }

            return Created("Created", new { Response = StatusCode(201) });
        }

        #region Documentation
        [SwaggerOperation(Summary = "List of all Contacts",Description = "Require admin privileges")]
        [SwaggerResponse(200, "Success. Returns a list of existing Contacts")]
        [SwaggerResponse(400, "BadRequest. Something went wrong, try again")]
        [SwaggerResponse(401, "Unauthenticated user or wrong jwt token")]
        #endregion
        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<IEnumerable<Contact>>> GetsAll()
        {
            var contacts=await _contactService.GetAllAsync();
            if (contacts == null)
            {
                return NotFound();
            }
            return Ok(contacts);            

        }
    }
}
