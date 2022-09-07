using System.Threading.Tasks;
using OngProject.Core.Models.DTOs;
using OngProject.Core.Models;
using OngProject.Core.Models.DTOs.Slide;

namespace OngProject.Services.Interfaces
{
    public interface ISlideService : IGenericService<Slide>
    {
        Task<Slide> UpdateSlide(Slide slide, SlideCreateDTO slideDto);
    }
}