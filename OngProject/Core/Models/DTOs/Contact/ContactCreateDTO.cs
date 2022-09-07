using System.ComponentModel.DataAnnotations;

namespace OngProject.Core.Models.DTOs.Contact
{
    public class ContactCreateDTO
    {
        [Required(ErrorMessage = "El campo Name es requerido")]
        [StringLength(50)]
        public string Name { get; set; }

        public int Phone { get; set; }

        [Required(ErrorMessage = "El campo Email es requerido")]
        [StringLength(50)]
        [EmailAddress]
        public string Email { get; set; }

        [StringLength(200)]
        public string Message { get; set; }
    }
}
