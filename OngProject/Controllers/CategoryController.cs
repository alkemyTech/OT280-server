using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OngProject.Core.Models;
using OngProject.Core.Models.DTOs.Category;
using OngProject.Repositories.Interfaces;
using OngProject.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using OngProject.Core.Helper;
using OngProject.Core.Models.DTOs;
using Swashbuckle.AspNetCore.Annotations;

namespace OngProject.Controllers
{
    [Authorize(Roles = "admin")]
    [Route("categories")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CategoryController(ICategoryService categoryService, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _categoryService = categoryService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        #region Documentation
        [SwaggerOperation(Summary = "List of all Categories", Description = "Require admin privileges")]
        [SwaggerResponse(200, "Success. Returns a list of existing Categories")]
        [SwaggerResponse(401, "Unauthenticated user or wrong jwt token")]
        [SwaggerResponse(403, "Unauthorized user or wrong jwt token")]
        [SwaggerResponse(500, "Internal server error. An error occurred while processing your request.")]
        #endregion
        [Authorize(Roles = "admin, standard")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<CategoryGetAllNamesResponse>))]
        public async Task<IActionResult> GetAll([FromQuery] PaginacionDto paginacionDto)
        {
            //Paginacion
            var queryable = _unitOfWork.Context.Categories.AsQueryable();
            await HttpContext.InsertarParametrosPaginacion(queryable, paginacionDto.CantidadRegistroPorPagina);
            
            var categories = await queryable.Paginar(paginacionDto).ToListAsync();
            var categoriesDto = _mapper.Map<IEnumerable<CategoryGetAllNamesResponse>>(categories);

            return new OkObjectResult(categoriesDto);
        }

        #region Documentation
        [SwaggerOperation(Summary = "Get Category details by Id", Description = "Requires admin privileges")]
        [SwaggerResponse(200, "Success. Returns Category details")]
        [SwaggerResponse(401, "Unauthenticated user or wrong jwt token")]
        [SwaggerResponse(403, "Unauthorized user")]
        [SwaggerResponse(404, "NotFound. Entity id not found.")]
        [SwaggerResponse(500, "Internal server error. An error occurred while processing your request.")]
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

            var categoryDto = _mapper.Map<CategoryDTO>(category);

            return new OkObjectResult(categoryDto);
        }

        #region Documentacion
        [SwaggerOperation(Summary = "Creates a new Category", Description = "Requires admin privileges")]
        [SwaggerResponse(200, "Success.")]
        [SwaggerResponse(400, "BadRequest. Object not created, try again")]
        [SwaggerResponse(401, "Unauthenticated or wrong jwt token")]
        [SwaggerResponse(403, "Unauthorized user")]
        [SwaggerResponse(500, "Internal server error. An error occurred while processing your request.")]
        #endregion
        [HttpPost]
        public async Task<IActionResult> Create(CategoryDTO categoryDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var category = _mapper.Map<Categories>(categoryDto);
            var created = await _categoryService.CreateAsync(category);

            if (created)
                _unitOfWork.Commit();

            return Created("Created", new { Response = StatusCode(201) });
        }
        
        #region Documentacion
        [SwaggerOperation(Summary = "Modifies an existing Category", Description = "Requires admin privileges")]
        [SwaggerResponse(200, "Success.")]
        [SwaggerResponse(400, "BadRequest. Object not modified, try again")]
        [SwaggerResponse(401, "Unauthenticated or wrong jwt token")]
        [SwaggerResponse(403, "Unauthorized user")]
        [SwaggerResponse(404, "NotFound. Entity id not found.")]
        [SwaggerResponse(500, "Internal server error. An error occurred while processing your request.")]
        #endregion
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CategoryDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(int id, CategoryDTO editCategoryDto)
        {
            var entity = await _categoryService.GetById(id);

            if (entity == null)
            {
                return NotFound();
            }

            await _categoryService.UpdateCategory(entity, editCategoryDto);
            var categoryDto = _mapper.Map<CategoryDTO>(entity);
            _unitOfWork.Commit();

            return new OkObjectResult(categoryDto);
        }

        #region Documentacion
        [SwaggerOperation(Summary = "Soft delete an existing Category", Description = "Requires admin privileges")]
        [SwaggerResponse(204, "Deleted.")]
        [SwaggerResponse(401, "Unauthenticated or wrong jwt token")]
        [SwaggerResponse(403, "Unauthorized user")]
        [SwaggerResponse(404, "NotFound. Entity id not found.")]
        [SwaggerResponse(500, "Internal server error. An error occurred while processing your request.")]
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
