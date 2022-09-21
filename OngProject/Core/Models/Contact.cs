using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace OngProject.Core.Models
{
    public class Contact
    {
        /// <summary>
        /// Criterios de aceptación:
        /// Al completar el Formulario de Contacto, se almacenará en la base de datos.
        /// Nombre de tabla: contacts.
        /// Contendrá los campos 
        /// name, 
        /// phone, 
        /// email, 
        /// message, 
        /// deletedAt (utilizada para soft delete)
        /// </summary>

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ContactId { get; set; }

        [Required(ErrorMessage = "El campo Name es requerido")]
        [StringLength(50)]
        public string Name { get; set; }

        [Required(ErrorMessage = "El campo Phone es requerido")]
        public int Phone { get; set; }

        [Required(ErrorMessage = "El campo Email es requerido")]
        [StringLength(50)]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "El campo Message es requerido")]
        [StringLength(200)]
        public string Message { get; set; }

        public bool IsDeleted { get; set; }
    }
}
