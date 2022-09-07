using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using OngProject.Core.Models;
using OngProject.Core.Models.DTOs.Contact;
using OngProject.Repositories.Interfaces;
using OngProject.Services.Interfaces;
using System.Threading.Tasks;

namespace OngProject.Controllers
{
    [Route("contacts")]
    [ApiController]
    public class ContactController : Controller
    {
        private readonly IContactService _contactService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ContactController(IContactService contactService, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _contactService = contactService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> Create(ContactCreateDTO contactDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var newContact = _mapper.Map<Contact>(contactDTO);
            var created = await _contactService.CreateAsync(newContact);

            if (created)
                _unitOfWork.Commit();

            return Created("Created", new { Response = StatusCode(201) });
        }
    }
}
