using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;

namespace OngProject.Core.Models.DTOs
{
    public class CommentDTO
    {
        [Required(ErrorMessage = "Comment body is required")]
        [RequiredNotEmpty(ErrorMessage = "Comment body cannot be empty")]
        public string body { get; set; }

    }

    public class RequiredNotEmptyAttribute : RequiredAttribute
    {
        public override bool IsValid(object value)
        {
            if (value is string) 
                return !String.IsNullOrEmpty((string)value);

            return base.IsValid(value);
        }
    }
}
