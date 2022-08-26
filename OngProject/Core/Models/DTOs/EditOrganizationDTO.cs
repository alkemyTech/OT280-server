using System.ComponentModel.DataAnnotations;

namespace OngProject.Core.Models.DTOs
{
    public class EditOrganizationDTO
    {
        public int OrganizationId { get; set; }


        [Required(ErrorMessage = "El campo Name es requerido")]
        [StringLength(50)]
        public string Name { get; set; }


        [Required(ErrorMessage = "El campo Image es requerido")]
        [StringLength(100)]
        public string Image { get; set; }


        [StringLength(100)]
        public string? Address { get; set; }

        [Phone(ErrorMessage = "El formato del campo debe corresponder al de un número telefonico")]
        [Range(0, 20, ErrorMessage = " El valor del campo debe ser un número entre 0 y 20")]
        public int? Phone { get; set; }


        [Required(ErrorMessage = "El campo email es requerido")]
        [StringLength(100)]
        [EmailAddress(ErrorMessage = "El campo debe contener un formato de email")]
        public string Email { get; set; }


        [Required(ErrorMessage = "El campo welcomeText es requerido")]
        [StringLength(100)]
        public string WelcomeText { get; set; }


        [Required(ErrorMessage = "El campo aboutUsText es requerido")]
        [StringLength(100)]
        public string AboutUsText { get; set; }
    }
}
