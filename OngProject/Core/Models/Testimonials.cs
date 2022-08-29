using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OngProject.Core.Models
{
    public class Testimonials
    {
        /*
          Campos:
          id: INTEGER NOT NULL AUTO_INCREMENT
          name: VARCHAR NOT NULL
          image: VARCHAR NULLABLE
          content: VARCHAR NULLABLE
          deletedAt: DATETIME 
          */

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TestimonialId { get; set; }

        [Required(ErrorMessage = "El campo Name es requerido")]
        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(100)]
        public string? Image { get; set; }

        public string? Content { get; set; }

        public DateTime? DeletedAt { get; set; }
    }
}
