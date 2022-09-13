using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace OngProject.Core.Models.DTOs.Slide
{
    public class SlideCreateDTO
    {
        [Required]
        public string ImageBase64 { get; set; }

        [Required]
        public string ImageName { get; set; }

        [Required]
        [MaxLength(255)]
        public string Text { get; set; }

        public int Order { get; set; }

        //[ForeignKey("Organization")]
        //public int OrganizationId { get; set; }

        public Organization Organization { get; set; }
    }
}
