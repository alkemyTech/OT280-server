using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OngProject.Core.Models;
using OngProject.Core.Models.DTOs.Category;
using OngProject.Repositories.Interfaces;
using OngProject.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OngProject.Core.Helper;
using OngProject.Core.Models.DTOs;

namespace OngProject.Controllers
{
    //[Authorize(Roles="admin")]
    [Route("categories")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CategoryController(ICategoryService categoryService, IUnitOfWork unitOfWork, IMapper mapper)
        {
            this._categoryService = categoryService;
            this._unitOfWork = unitOfWork;
            this._mapper = mapper;
        }

        #region Documentacion

        /// <summary>
        /// Endpoint para listar todas las categorias existentes en el sistema. Rol: ADMIN
        /// </summary>
        /// <returns>Lista de categorias como Categories[]</returns>
        /// <response code="200">Solicitud concretada con exito</response>
        /// <response code="401">Credenciales no válidas</response>

        #endregion
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<CategoryGetAllNamesResponse>))]
        public async Task<IActionResult> GetAll([FromQuery] PaginacionDto paginacionDto)
        {
            //Paginacion
            var queryable = _unitOfWork.Context.Categories.AsQueryable();
            await HttpContext.InsertarParametrosPaginacion(queryable, paginacionDto.CantidadRegistroPorPagina);
            
            var categories = await queryable.Paginar(paginacionDto).ToListAsync();
            var categoriesDTO = _mapper.Map<IEnumerable<CategoryGetAllNamesResponse>>(categories);

            return new OkObjectResult(categoriesDTO);
        }

        #region Documentacion

        /// <summary>
        /// Endpoint para obtener los datos de una categoria por su id. Rol: ADMIN
        /// </summary>
        /// <returns>Objeto de la clase Categories</returns>
        /// <response code="200">Solicitud concretada con exito</response>
        /// <response code="401">Credenciales no válidas</response>

        #endregion
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CategoryDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            var category = await _categoryService.GetById(id);

            if (category == null)
            {
                return NotFound();
            }

            var categoryDTO = _mapper.Map<CategoryDTO>(category);

            return new OkObjectResult(categoryDTO);
        }

        #region Documentacion

        /// <summary>
        /// Endpoint para el manejo de la creacion de Categorias. Rol: ADMIN
        /// </summary>
        /// <returns>Código HTTP con el resultado de la operacion</returns>
        /// <response code="200">Solicitud concretada con exito</response>
        /// <response code="401">Credenciales no válidas</response>

        #endregion
        [HttpPost]
        public async Task<IActionResult> Create(CategoryDTO categoryDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var _category = _mapper.Map<Categories>(categoryDTO);
            var created = await _categoryService.CreateAsync(_category);

            if (created)
                _unitOfWork.Commit();

            return Created("Created", new { Response = StatusCode(201) });
        }
        
        #region Documentacion
        
        /// <summary>
        /// Endpoint para actualizar una category que se consigue al buscarlo por id.Rol: ADMIN
        /// </summary>
        /// <response code="200">Solicitud concretada con exito</response>
        /// <response code="401">Credenciales no validas</response>
        
        #endregion
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CategoryDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(int id, CategoryDTO editCategoryDTO)
        {
            var entity = await _categoryService.GetById(id);

            if (entity == null)
            {
                return NotFound();
            }

            await _categoryService.UpdateCategory(entity, editCategoryDTO);
            var categoryDTO = _mapper.Map<CategoryDTO>(entity);
            _unitOfWork.Commit();

            return new OkObjectResult(categoryDTO);
        }

        #region Documentacion

        /// <summary>
        /// Endpoint que elimina una category del sistema por su id. Rol: ADMIN
        /// </summary>
        /// <response code="200">Solicitud concretada con exito</response>
        /// <response code="401">Credenciales no válidas</response>

        #endregion
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _categoryService.GetById(id);

            if (category == null)
                return BadRequest();

            await _categoryService.DeleteAsync(category);
            _unitOfWork.Commit();

            return Ok(category);
        }
    }
}
