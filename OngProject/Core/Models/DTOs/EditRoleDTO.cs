using System.ComponentModel.DataAnnotations;

namespace OngProject.Core.Models.DTOs
{
    public class EditRoleDTO
    {
        [Required(ErrorMessage = "Id is required")]
        public string RoleId { get; set; }
        
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        public string Description { get; set; }

    }
}
