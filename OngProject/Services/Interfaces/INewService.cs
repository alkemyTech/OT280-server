using OngProject.Core.Models;
using OngProject.Core.Models.DTOs;
using System.Threading.Tasks;

namespace OngProject.Services.Interfaces
{
    public interface INewService : IGenericService<News>
    {
        Task<News> UpdateNews(News news, EditNewDTO editNewDTO);
    }
}