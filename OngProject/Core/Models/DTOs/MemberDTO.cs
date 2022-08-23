using System.ComponentModel.DataAnnotations;

namespace OngProject.Core.Models.DTOs
{
    public class MemberDTO
    {
        [Required(ErrorMessage = "El campo Name es requerido")]
        [StringLength(50)]
        public string name { get; set; }

        [StringLength(100)]
        public string? facebookUrl { get; set; }

        [StringLength(100)]
        public string? instagramUrl { get; set; }

        [StringLength(100)]
        public string? linkedinUrl { get; set; }

        [Required(ErrorMessage = "El campo Image es requerido")]
        [StringLength(100)]
        public string image { get; set; }

        [DataType(DataType.MultilineText)]
        public string? description { get; set; }

    }
}
