using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OngProject.Core.Models.DTOs.Slide
{
    public class SlideDTO
    {
        public string ImageUrl { get; set; }

        public string Text { get; set; }

        public int Order { get; set; }

        [ForeignKey("Organization")]
        public int OrganizationId { get; set; }
    }
}
