using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OngProject.Core.Models
{
    public class News
    {
        /*
         Campos:
         id: INTEGER NOT NULL AUTO_INCREMENT
         name: VARCHAR NOT NULL
         content: TEXT NOT NULL
         image: VARCHAR NOT NULL
         categoryId: Clave foranea hacia ID de Categories
         timestamps y softDeletes
         */

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int NewId { get; set; }

        [Required(ErrorMessage = "El campo Name es requerido")]
        [StringLength(50)]
        public string Name { get; set; }

        [DataType(DataType.MultilineText)]
        public string Content { get; set; }

        [Required(ErrorMessage = "El campo Image es requerido")]
        [StringLength(100)]
        public string Image { get; set; }

        [Timestamp]
        public byte[] ChangeCheck { get; set; }
        public bool IsDeleted { get; set; }


        //Clave foranea hacia ID de Categories
        //categoryId
        //uno a muchos (Categories a News)

        [ForeignKey("Categories")]
        public int CategoryId { get; set; }
        public Categories Categories { get; set; }
    }
}
