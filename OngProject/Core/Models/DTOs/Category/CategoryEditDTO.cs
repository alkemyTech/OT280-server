using System.ComponentModel.DataAnnotations;

namespace OngProject.Core.Models.DTOs.Category
{
    public class CategoryEditDTO
    {
        [Required(ErrorMessage = "Id is required")]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(50)]
        public string Name { get; set; }

        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [StringLength(100)]
        public string Image { get; set; }
    }
}
