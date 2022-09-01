using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OngProject.Core.Models;
using OngProject.Core.Models.DTOs;
using OngProject.Repositories.Interfaces;
using OngProject.Services.Interfaces;
using System.Threading.Tasks;

namespace OngProject.Controllers
{
    [ApiController]
    public class TestimonialController : ControllerBase
    {
        private readonly ITestimonialService _testimonialService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TestimonialController(ITestimonialService testimonialService, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _testimonialService = testimonialService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpPost]
        [Route("/testimonials")]
        public async Task<IActionResult> Create(TestimonialDTO testimonial)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var _testimonial = _mapper.Map<Testimonials>(testimonial);
            var created = await _testimonialService.CreateAsync(_testimonial);

            if (created)
                _unitOfWork.Commit();

            return Created("Created", new { Response = StatusCode(201) });
        }
    }

    
}
