using System.ComponentModel.DataAnnotations;

namespace OngProject.Core.Models.DTOs
{
    public class EditCategoryDTO
    {
        [Required]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "El campo Name es requerido")]
        [StringLength(50)]
        public string Name { get; set; }

        [DataType(DataType.MultilineText)]
        public string? Description { get; set; }

        [StringLength(100)]
        public string? Image { get; set; }
    }
}
