using OngProject.Core.Models;
using OngProject.Core.Models.DTOs;
using System.Threading.Tasks;

namespace OngProject.Services.Interfaces
{
    public interface ICategoryService : IGenericService<Categories>
    {
        Task<Categories> UpdateCategory(Categories category, EditCategoryDTO editCategoryDTO);
    }
}
