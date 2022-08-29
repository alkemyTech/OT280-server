using System.ComponentModel.DataAnnotations;

namespace OngProject.Core.Models.DTOs
{
    public class EditNewDTO
    {
        [Required]
        public int NewId { get; set; }

        [Required(ErrorMessage = "El campo Name es requerido")]
        [StringLength(50)]
        public string Name { get; set; }

        [DataType(DataType.MultilineText)]
        public string? Content { get; set; }

        [Required(ErrorMessage = "El campo Image es requerido")]
        [StringLength(100)]
        public string Image { get; set; }
    }
}
