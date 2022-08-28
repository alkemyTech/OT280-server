using OngProject.Core.Models;
using OngProject.Core.Models.DTOs;
using OngProject.Repositories.Interfaces;
using OngProject.Services.Interfaces;
using System.Threading.Tasks;

namespace OngProject.Services
{
    public class NewService : GenericService<News>, INewService
    {
        private INewRepository _newRepository;
        public NewService(INewRepository newRepository) : base(newRepository)
        {
            this._newRepository = newRepository;
        }

        public async Task<News> UpdateNews(News news, EditNewDTO editNewDTO)
        {
            news.Name = editNewDTO.Name;
            news.Content = editNewDTO.Content;
            news.Image = editNewDTO.Image;

            await _newRepository.Update(news);

            return news;
        }


    }
}
