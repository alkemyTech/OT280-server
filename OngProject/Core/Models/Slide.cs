using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OngProject.Core.Models
{
    public class Slide
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SlideId { get; set; }

        [Required]
        [MaxLength(255)]
        public string ImageUrl { get; set; }

        [Required]
        [MaxLength(255)]
        public string Text { get; set; }

        public int Order { get; set; }

        [ForeignKey("Organization")]
        public int OrganizationId { get; set; }

        public Organization Organization { get; set; }
    }
}
