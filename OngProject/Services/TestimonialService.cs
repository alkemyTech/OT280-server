using OngProject.Core.Models;
using OngProject.Core.Models.DTOs;
using OngProject.Repositories.Interfaces;
using OngProject.Services.Interfaces;
using System;

namespace OngProject.Services
{
    public class TestimonialService : GenericService<Testimonials>, ITestimonialService
    {
        private ITestimonialRepository _testimonialRepository;
        public TestimonialService(ITestimonialRepository testimonialRepository) : base(testimonialRepository)
        {
            this._testimonialRepository = testimonialRepository;
        }

        public async void UpdateTestimonial(Testimonials testimonial, NewsDTO editTestimonialDTO)
        {
            testimonial.Name = editTestimonialDTO.Name;
            testimonial.Image = editTestimonialDTO.Image;
            testimonial.Content = editTestimonialDTO.Content;

            await _testimonialRepository.Update(testimonial);
        }

        public async void DeleteTestimonial(Testimonials testimonial)
        {
            testimonial.DeletedAt = DateTime.Now;
            await _testimonialRepository.Update(testimonial);
        }
    }
}