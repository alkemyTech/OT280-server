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

namespace OngProject.Controllers
{
    [Authorize(Roles = "Administrator")]
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

        #region Documentacion
        /// <summary>
        /// Endpoint para el manejo de la creacion de Testimonios.Se debe ser ADMINISTRADOR
        /// </summary>
        /// <response code="200">Solicitud concretada con exito</response>
        /// <response code="401">Credenciales no validas</response>
        #endregion
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

        #region Documentacion
        /// <summary>
        /// Endpoint para actualizar un testimonio que se consigue al buscarlo por id.Se debe ser ADMINISTRADOR 
        /// </summary>
        /// <response code="200">Solicitud concretada con exito</response>
        /// <response code="401">Credenciales no validas</response>
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

        #region Documentacion
        /// <summary>
        /// Endpoint para obtener la lista de los Testimonios existentes.Se debe ser usuario ADMINISTRADOR
        /// </summary>
        /// <response code="200">Solicitud concretada con exito</response>
        /// <response code="401">Credenciales no validas</response>
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
            var testimonialsDTO = _mapper.Map<IEnumerable<TestimonialDTO>>(testimonials);

            return new OkObjectResult(testimonialsDTO);
        }

        #region Documentacion
        /// <summary>
        /// Endpoint que borra un testimonio que se busca por su id.Se debe ser ADMINISTRADOR 
        /// </summary>
        /// <response code="200">Solicitud concretada con exito</response>
        /// <response code="401">Credenciales no validas</response>
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
