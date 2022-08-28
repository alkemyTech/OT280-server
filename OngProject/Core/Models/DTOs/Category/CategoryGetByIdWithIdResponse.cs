using System.ComponentModel.DataAnnotations;

namespace OngProject.Core.Models.DTOs.Category
{
    public class CategoryGetByIdWithIdResponse
    {
        public int Id { get; set; }

        public string Name { get; set; }

        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        public string Image { get; set; }
    }
}
