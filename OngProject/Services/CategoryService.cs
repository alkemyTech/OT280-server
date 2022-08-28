using OngProject.Core.Models;
using OngProject.Core.Models.DTOs.Category;
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

        public async Task<Categories> UpdateCategory(Categories category, CategoryDTO categoryDTO)
        {
            category.Name = categoryDTO.Name;
            category.Description = categoryDTO.Description;
            category.Image = categoryDTO.Image;

            await _categoryRepository.Update(category);

            return category;
        }
    }
}
