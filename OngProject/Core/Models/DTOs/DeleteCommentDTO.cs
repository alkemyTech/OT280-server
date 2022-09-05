using System.ComponentModel.DataAnnotations;

namespace OngProject.Core.Models.DTOs
{
    public class DeleteCommentDTO
    {
        [Required(ErrorMessage = "user_id is required")]
        public string user_id { get; set; }
    }
}
