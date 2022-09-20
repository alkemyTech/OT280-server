using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OngProject.Core.Helper;
using OngProject.Core.Models;
using OngProject.Core.Models.DTOs;
using OngProject.Repositories.Interfaces;
using OngProject.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using Swashbuckle.AspNetCore.Annotations;

namespace OngProject.Controllers
{
    [Authorize(Roles = "admin")]
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
        
        #region Documentation
        [SwaggerOperation(Summary = "List of all Testimonials.", Description = ".")]
        [SwaggerResponse(200, "Success. Returns a list of existing Slides.")]
        [SwaggerResponse(401, "Unauthenticated user or wrong jwt token.")]
        [SwaggerResponse(500, "Internal server error. An error occurred while processing your request.")]
        #endregion
        [HttpGet]
        [Route("api/testimonials")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<TestimonialDTO>))]
        public async Task<IActionResult> GetAll([FromQuery] PaginacionDto paginacionDto)
        {
            //Paginacion
            var queryable = _unitOfWork.Context.Testimonials.AsQueryable();
            await HttpContext.InsertarParametrosPaginacion(queryable, paginacionDto.CantidadRegistroPorPagina);

            //var testimonials = await _testimonialService.GetAllAsync();
            var testimonials = await queryable.Paginar(paginacionDto).ToListAsync();
            var testimonialsDto = _mapper.Map<IEnumerable<TestimonialDTO>>(testimonials);

            return new OkObjectResult(testimonialsDto);
        }

        #region Documentation
        [SwaggerOperation(Summary = "Create a Testimony.",Description = "Requires admin privileges.")]
        [SwaggerResponse(200, "Created. Returns the id of the created object.")]
        [SwaggerResponse(400, "BadRequest. Object not created, try again.")]
        [SwaggerResponse(401, "Unauthenticated or wrong jwt token.")]
        [SwaggerResponse(403, "Unauthorized user.")]
        [SwaggerResponse(500, "Internal server error. An error occurred while processing your request.")]
        #endregion
        [HttpPost]
        [Route("/testimonials")]
        public async Task<IActionResult> Create(TestimonialDTO testimonialDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var testimonial = _mapper.Map<Testimonials>(testimonialDto);
            var created = await _testimonialService.CreateAsync(testimonial);

            if (created)
                _unitOfWork.Commit();

            return Created("Created", new { Response = StatusCode(201) });
        }

        #region Documentation
        [SwaggerOperation(Summary = "Modifies an existing Testimony",Description = "Requires admin privileges")]
        [SwaggerResponse(204, "Updated. Returns nothing.")]
        [SwaggerResponse(400, "BadRequest. Something went wrong, try again.")]
        [SwaggerResponse(401, "Unauthenticated or wrong jwt token.")]
        [SwaggerResponse(403, "Unauthorized user.")]
        [SwaggerResponse(404, "NotFound. Entity id not found.")]
        [SwaggerResponse(500, "Internal server error. An error occurred while processing your request.")]
        #endregion
        [HttpPut]
        [Route("/testimonials/{id}")]
        public async Task<ActionResult> Update(int id, TestimonialDTO testimonial)
        {
            var entity = await _testimonialService.GetById(id);

            if (ModelState.IsValid && entity != null)
            {
                _testimonialService.UpdateTestimonial(entity, testimonial);
                _unitOfWork.Commit();

                return new OkObjectResult(testimonial);
            }

            return NotFound();
        }

        #region Documentation
        [SwaggerOperation(Summary = "Soft delete an existing Testimony", Description = "Requires admin privileges")]
        [SwaggerResponse(204, "Deleted. Returns nothing.")]
        [SwaggerResponse(401, "Unauthenticated user or wrong jwt token.")]
        [SwaggerResponse(403, "Unauthorized user.")]
        [SwaggerResponse(404, "NotFound. Entity id not found.")]
        [SwaggerResponse(500, "Internal server error. An error occurred while processing your request.")]
        #endregion
        [HttpDelete]
        [Route("/testimonials/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var testimonials = await _testimonialService.GetById(id);

            if (testimonials == null)
                return BadRequest();

            _testimonialService.DeleteTestimonial(testimonials);
            _unitOfWork.Commit();

            return Ok(testimonials);
        }
    }

    
}
