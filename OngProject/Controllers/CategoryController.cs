using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OngProject.Core.Models;
using OngProject.Core.Models.DTOs;
using OngProject.Repositories.Interfaces;
using OngProject.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OngProject.Controllers
{
    [Route("api/[controller]")]
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

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<CategoryDTO>))]
        public async Task<IActionResult> GetAll()
        {
            var categories = await _categoryService.GetAllAsync();
            var categoriesDTO = _mapper.Map<IEnumerable<CategoryDTO>>(categories);

            return new OkObjectResult(categoriesDTO);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(MemberDTO))]
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

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(EditCategoryDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(EditCategoryDTO editCategoryDTO)
        {
            var entity = await _categoryService.GetById(editCategoryDTO.CategoryId);

            if (entity == null)
            {
                return NotFound();
            }

            await _categoryService.UpdateCategory(entity, editCategoryDTO);
            var categoryDTO = _mapper.Map<EditCategoryDTO>(entity);
            _unitOfWork.Commit();

            return new OkObjectResult(categoryDTO);

        }
    }
}
