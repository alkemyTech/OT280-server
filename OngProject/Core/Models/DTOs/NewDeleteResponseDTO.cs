using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;

namespace OngProject.Core.Models.DTOs
{
    public class NewDeleteResponseDTO
    {
        public int NewId { get; set; }

        public string Name { get; set; }

        [DataType(DataType.MultilineText)]
        public string Content { get; set; }

        [StringLength(100)]
        public string Image { get; set; }

        [Timestamp]
        public byte[] ChangeCheck { get; set; }
        public bool IsDeleted { get; set; }

        public DateTime? DeletedAt { get; set; }

        //[ForeignKey("Categories")]
        public int CategoryId { get; set; }

    }
}
