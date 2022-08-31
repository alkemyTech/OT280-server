using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OngProject.Core.Models
{
    public class Categories
    {
        /// <summary>
        /// 
        /// </summary>
        /*
         Campos:
         id: INTEGER NOT NULL AUTO_INCREMENT
         name: VARCHAR NOT NULL
         description: VARCHAR NULLABLE
         image: VARCHAR NULLABLE
         timestamps y softDeletes
         */

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "El campo Name es requerido")]
        [StringLength(50)]
        public string Name { get; set; }

        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [StringLength(100)]
        public string Image { get; set; }

        [Timestamp]
        public byte[] ChangeCheck { get; set; }
        public bool IsDeleted { get; set; }

        //Para FK con tabla News
        public ICollection<News> News { get; set; }
    }

}
