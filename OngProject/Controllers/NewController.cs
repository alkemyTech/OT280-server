﻿using System;
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
using OngProject.Services;
using OngProject.Services.Interfaces;

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
            this._newService = newService;
            this._unitOfWork = unitOfWork;
            this._mapper = mapper;
        }

        #region Documentacion

        /// <summary>
        /// Endpoint para eliminar una novedad por su id.
        /// </summary>
        /// <returns>Objeto de la clase Novedades</returns>
        /// <response code="200">Solicitud concretada con exito</response>
        /// <response code="401">Credenciales no válidas</response>

        #endregion
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var _new = await _newService.GetById(id);

            if (_new == null)
                return BadRequest();

            _newService.DeleteNew(_new);
            _unitOfWork.Commit();

            NewDeleteResponseDTO _newDTO = 
                _mapper.Map<NewDeleteResponseDTO>(_new);

            return Ok(_newDTO);
        }

        #region Documentacion

        /// <summary>
        /// Endpoint para listar todas las novedades existentes en el sistema
        /// </summary>
        /// <returns>Lista de novedades como News[]</returns>
        /// <response code="200">Solicitud concretada con exito</response>
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
            var newsDTO = _mapper.Map<IEnumerable<NewsDTO>>(news);

            return new OkObjectResult(newsDTO);
        }

        #region Documentacion

        /// <summary>
        /// Endpoint para obtener los datos de una novedad por su id.
        /// </summary>
        /// <returns>Objeto de la clase Novedades</returns>
        /// <response code="200">Solicitud concretada con exito</response>
        /// <response code="401">Credenciales no válidas</response>

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

            var newsDTO = _mapper.Map<NewsDTO>(news);

            return new OkObjectResult(newsDTO);
        }

        #region Documentation
        /// <summary>
        /// Alta de novedad. 
        /// </summary>
        /// <response code="200">Solicitud concretada con exito</response>
        /// <response code="401">Credenciales no válidas</response>
        #endregion
        [HttpPost]        
        public async Task<IActionResult> Create(CreateNewsDTO createNewsDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            try
            {
                var news = _mapper.Map<News>(createNewsDTO);
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
        /// <summary>
        /// Actualización de novedad existente
        /// </summary>
        /// <response code="200">Solicitud concretada con exito</response>
        /// <response code="404">No encontrado</response>
        #endregion
        [HttpPut]        
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(EditNewDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Update(EditNewDTO editNewDTO)
        {
            var entity = await _newService.GetById(editNewDTO.NewId);

            if (ModelState.IsValid && entity != null)
            {
                await _newService.UpdateNews(entity, editNewDTO);
                _unitOfWork.Commit();

                return new OkObjectResult(editNewDTO);
            }

            return NotFound();
        }                 
    }
}
