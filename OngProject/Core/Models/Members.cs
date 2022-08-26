using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OngProject.Core.Models
{
    public class Members
    {
        /*
         Campos:
         id: INTEGER NOT NULL AUTO_INCREMENT
         name: VARCHAR NOT NULL
         facebookUrl: VARCHAR NULLABLE
         instagramUrl: VARCHAR NULLABLE
         linkedinUrl: VARCHAR NULLABLE
         image: VARCHAR NOT NULL
         description: VARCHAR NULLABLE
         timestamps y softDeletes
         */

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MemberId { get; set; }

        [Required(ErrorMessage = "El campo Name es requerido")]
        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(100)]
        public string FacebookUrl { get; set; }

        [StringLength(100)]
        public string InstagramUrl { get; set; }

        [StringLength(100)]
        public string LinkedinUrl { get; set; }

        [Required(ErrorMessage = "El campo Image es requerido")]
        [StringLength(100)]
        public string Image { get; set; }

        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Timestamp]
        public byte[] ChangeCheck { get; set; }
        public bool IsDeleted { get; set; }

    }
}
