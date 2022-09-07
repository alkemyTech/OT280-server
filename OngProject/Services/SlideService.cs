using System.Threading.Tasks;
using OngProject.Core.Models.DTOs;
using OngProject.Core.Models;
using OngProject.Core.Models.DTOs.Slide;
using OngProject.Repositories.Interfaces;
using OngProject.Services.Interfaces;

namespace OngProject.Services
{
    public class SlideService : GenericService<Slide>, ISlideService
    {
        private readonly ISlideRepository _slideRepository;

        public SlideService(ISlideRepository slideRepositor) : base(slideRepositor)
        {
            _slideRepository = slideRepositor;
        }

        public async Task<Slide> UpdateSlide(Slide slide, SlideCreateDTO slideDto)
        {
            slide.ImageUrl = slideDto.ImageBase64;
            slide.Text = slideDto.Text;
            slide.Order = slideDto.Order;
            slide.Organization = slideDto.Organization;

            await _slideRepository.Update(slide);

            return slide;
        }
    }
}
