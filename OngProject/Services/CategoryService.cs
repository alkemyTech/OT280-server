using OngProject.Core.Models;
using OngProject.Core.Models.DTOs;
using OngProject.Repositories.Interfaces;
using OngProject.Services.Interfaces;
using System.Threading.Tasks;

namespace OngProject.Services
{
    public class CategoryService : GenericService<Categories>, ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        public CategoryService(ICategoryRepository categoryRepository) : base(categoryRepository)
        {
            this._categoryRepository = categoryRepository;
        }

        public async Task<Categories> UpdateCategory(Categories category, EditCategoryDTO editCategoryDTO)
        {
            category.Name = editCategoryDTO.Name;
            category.Description = editCategoryDTO.Description;
            category.Image = editCategoryDTO.Image;

            await _categoryRepository.Update(category);

            return category;
        }
    }
}
