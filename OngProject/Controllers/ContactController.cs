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

        public ContactController(IContactService contactService, IUnitOfWork unitOfWork, IMapper mapper, IEmailService emailService)
        {
            _contactService = contactService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _emailService = emailService;
        }

        [HttpPost]
        public async Task<IActionResult> Create(ContactCreateDTO contactDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var newContact = _mapper.Map<Contact>(contactDTO);
            var created = await _contactService.CreateAsync(newContact);

            if (created && _emailService.IsConfigured())
            {
                _emailService.SendSuccessContact(contactDTO.Email);
                _unitOfWork.Commit();                                  
            }

            return Created("Created", new { Response = StatusCode(201) });
        }

        
        [HttpGet]
        //[Authorize(Roles="Admin")]
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
