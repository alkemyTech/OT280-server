using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace OngProject.Entities.Domain
{
    public class Roles : IdentityRole
    {
        [Required(ErrorMessage = "Name is required")]
        override public string Name { get; set; }

        public string Description { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }
        public bool IsDeleted { get; set; }

    }
}
