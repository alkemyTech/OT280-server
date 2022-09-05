using System.ComponentModel.DataAnnotations;

namespace OngProject.Core.Models.DTOs.Slide
{
    public class SlideDTO
    {
        [Required]
        [MaxLength(255)]
        public string ImageUrl { get; set; }

        public int Order { get; set; }
    }
}
