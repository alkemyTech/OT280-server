using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;

namespace OngProject.Core.Models.DTOs
{
    public class CommentDTO
    {
        [Required(ErrorMessage = "news_id is required")]
        public int news_id { get; set; }

        [Required(ErrorMessage = "user_id is required")]
        public string user_id { get; set; }

        [Required(ErrorMessage = "Comment body cannot be empty or null")]
        public string body { get; set; }

    }
}
