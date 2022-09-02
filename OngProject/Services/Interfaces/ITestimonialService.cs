using OngProject.Core.Models;
using OngProject.Core.Models.DTOs;

namespace OngProject.Services.Interfaces
{
    public interface ITestimonialService : IGenericService<Testimonials>
    {
        void UpdateTestimonial(Testimonials testimonial, TestimonialDTO editTestimonialDTO);
    }
}