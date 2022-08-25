using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OngProject.Core.Models
{
    public class Activities
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ActivityId { get; set; }

        [Required(ErrorMessage = "El campo Name es requerido")]
        [StringLength(50)]
        public string Name { get; set; }

        [Required(ErrorMessage = "El campo Content es requerido")]
        [DataType(DataType.MultilineText)]
        public string Content { get; set; }

        [Required(ErrorMessage = "El campo Image es requerido")]
        [StringLength(100)]
        public string Image { get; set; }

        public DateTime? DeletedAt { get; set; }

    }
}

