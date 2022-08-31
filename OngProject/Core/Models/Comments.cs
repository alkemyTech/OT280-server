using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;

namespace OngProject.Core.Models
{
    public class Comments
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public int CommentId { get; set; }

        [ForeignKey("Users")]
        [Required]
        public string UserId { get; set; }
        public Users Users { get; set; }

        [Required(ErrorMessage = "Comment body is required")]
        [RequiredNotEmpty]
        public string Body { get; set; }

        [ForeignKey("News")]
        public int NewId { get; set; }
        public News News { get; set; }

    }
    public class RequiredNotEmptyAttribute : RequiredAttribute
    {
        public override bool IsValid(object value)
        {
            if (value is string) return !String.IsNullOrEmpty((string)value);

            return base.IsValid(value);
        }
    }
}
