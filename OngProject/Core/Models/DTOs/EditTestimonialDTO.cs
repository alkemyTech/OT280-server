using System.ComponentModel.DataAnnotations;

namespace OngProject.Core.Models.DTOs
{
    public class EditTestimonialDTO
    {
        [Required]
        public int TestimonialId { get; set; }

        [Required(ErrorMessage = "El campo Name es requerido")]
        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(100)]
        public string? Image { get; set; }

        public string? Content { get; set; }
    }
}
