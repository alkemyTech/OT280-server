using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net.Mail;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace OngProject.Core.Models
{
    [Index(nameof(Email), IsUnique = true)]
    public class Users : IdentityUser
    {
        [Required(ErrorMessage = "First Name is required")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is required")]
        public string LastName { get; set; }

        [Column(TypeName = "varchar(max)")]
        public string Photo { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }

        public bool IsDeleted { get; set; }
    }
}
