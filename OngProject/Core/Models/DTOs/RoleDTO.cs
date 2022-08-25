using System.ComponentModel.DataAnnotations;

namespace OngProject.Core.Models.DTOs
{
    public class RoleDTO
    {
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        public string Description { get; set; }
        public string NormalizedName { get; set; }

    }
}
