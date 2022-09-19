using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
using Swashbuckle.AspNetCore.Annotations;

namespace OngProject.Controllers
{
    [Authorize(Roles = "admin")]
    [Route("news")]
    [ApiController]
    public class NewController : ControllerBase
    {
        private readonly INewService _newService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public NewController(INewService newService, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _newService = newService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        

        #region Documentacion
        [SwaggerOperation(Summary = "List of all News", Description = ".")]
        [SwaggerResponse(200, "Success. Returns a list of existing News.")]
        [SwaggerResponse(401, "Unauthenticated user or wrong jwt token.")]
        [SwaggerResponse(403, "Unauthorized user.")]
        [SwaggerResponse(500, "Internal server error. An error occurred while processing your request.")]
        #endregion
        [HttpGet]        
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<NewsDTO>))]
        public async Task<IActionResult> GetAll([FromQuery] PaginacionDto paginacionDto)
        {
            //Paginacion
            var queryable = _unitOfWork.Context.News.AsQueryable();
            await HttpContext.InsertarParametrosPaginacion(queryable, paginacionDto.CantidadRegistroPorPagina);

            //var news = await _newService.GetAllAsync();
            var news = await queryable.Paginar(paginacionDto).ToListAsync();
            var newsDto = _mapper.Map<IEnumerable<NewsDTO>>(news);

            return new OkObjectResult(newsDto);
        }

        #region Documentacion
        [SwaggerOperation(Summary = "Get News details by id", Description = "Requires admin privileges")]
        [SwaggerResponse(200, "Success. Returns the News details")]
        [SwaggerResponse(401, "Unauthenticated user or wrong jwt token.")]
        [SwaggerResponse(403, "Unauthorized user.")]
        [SwaggerResponse(404, "NotFound. Entity id not found.")]
        [SwaggerResponse(500, "Internal server error. An error occurred while processing your request.")]
        #endregion
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(NewsDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            var news = await _newService.GetById(id);

            if (news == null)
            {
                return NotFound();
            }

            var newsDto = _mapper.Map<NewsDTO>(news);

            return new OkObjectResult(newsDto);
        }

        #region Documentation
        [SwaggerOperation(Summary = "Create News", Description = "Requires admin privileges")]
        [SwaggerResponse(200, "Created. Returns the id of the created object.")]
        [SwaggerResponse(400, "BadRequest. Object not created, try again.")]
        [SwaggerResponse(401, "Unauthenticated or wrong jwt token.")]
        [SwaggerResponse(403, "Unauthorized user.")]
        [SwaggerResponse(500, "Internal server error. An error occurred while processing your request.")]
        #endregion
        [HttpPost]        
        public async Task<IActionResult> Create(CreateNewsDTO createNewsDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            try
            {
                var news = _mapper.Map<News>(createNewsDto);
                var created = await _newService.CreateAsync(news);

                if (created)
                    _unitOfWork.Commit();

                return Created("Created", new { Response = StatusCode(201) });
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }            
        }

        #region Documentation
        [SwaggerOperation(Summary = "Modifies an existing News", Description = "Requires admin privileges")]
        [SwaggerResponse(200, "Updated. Returns the object News updated")]
        [SwaggerResponse(400, "BadRequest. Something went wrong, try again.")]
        [SwaggerResponse(401, "Unauthenticated or wrong jwt token.")]
        [SwaggerResponse(403, "Unauthorized user.")]
        [SwaggerResponse(404, "NotFound. Entity id not found.")]
        [SwaggerResponse(500, "Internal server error. An error occurred while processing your request.")]
        #endregion
        [HttpPut]        
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(EditNewDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Update(EditNewDTO editNewDto)
        {
            var entity = await _newService.GetById(editNewDto.NewId);

            if (ModelState.IsValid && entity != null)
            {
                await _newService.UpdateNews(entity, editNewDto);
                _unitOfWork.Commit();

                return new OkObjectResult(editNewDto);
            }

            return NotFound();
        }   
        
        #region Documentacion
        [SwaggerOperation(Summary = "Soft delete an existing News", Description = "Requires admin privileges")]
        [SwaggerResponse(204, "Deleted. Returns nothing.")]
        [SwaggerResponse(401, "Unauthenticated user or wrong jwt token.")]
        [SwaggerResponse(403, "Unauthorized user.")]
        [SwaggerResponse(404, "NotFound. Entity id not found.")]
        [SwaggerResponse(500, "Internal server error. An error occurred while processing your request.")]
        #endregion
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var news = await _newService.GetById(id);

            if (news == null)
                return BadRequest();

            _newService.DeleteNew(news);
            _unitOfWork.Commit();

            NewDeleteResponseDTO newDto = 
                _mapper.Map<NewDeleteResponseDTO>(news);

            return Ok(newDto);
        }
    }
}
