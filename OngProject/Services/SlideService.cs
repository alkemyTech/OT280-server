using OngProject.Core.Models.DTOs;
using OngProject.Core.Models;
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

        //public async void UpdateSlide(Slide slide, SlideDTO slideDTO)
        //{
        //    slide.ImageUrl = activityDTO.Name;
        //    slide.Text = activityDTO.Content;
        //    slide.Order = activityDTO.Content;
        //    slide.Organization = activityDTO.Content;

        //    await _slideRepository.Update(slide);
        //}
    }
}
