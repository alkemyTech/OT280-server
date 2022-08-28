using OngProject.Core.Models;
using OngProject.Core.Models.DTOs.Category;
using OngProject.Repositories.Interfaces;
using OngProject.Services.Interfaces;
using System.Threading.Tasks;

namespace OngProject.Services
{
    public class CategoryService : GenericService<Categories>, ICategoryService
    {
        private ICategoryRepository _categoryRepository;
        public CategoryService(ICategoryRepository categoryRepository) : base(categoryRepository)
        {
            this._categoryRepository = categoryRepository;
        }

        public async Task<Categories> UpdateCategory(Categories category, CategoryDTO editCategoryDTO)
        {
            category.name = editCategoryDTO.Name;
            category.description = editCategoryDTO.Description;
            category.image = editCategoryDTO.Image;

            await _categoryRepository.Update(category);

            return category;
        }
    }
}
