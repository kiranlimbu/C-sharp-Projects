using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace bugspotAPI.Models
{
    public class AttachmentModel
    {
        [Key]
        public int attachmentId { get; set; }

        public int bugId { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTimeOffset uploaded { get; set; } = DateTimeOffset.Now;

        [DisplayName("Uploaded By")]
        public string userId { get; set; }

        // attachment file info
        [Column(TypeName = "varchar(50)")]
        public string imageName { get; set; }
        public byte[] imageData { get; set; }

        [NotMapped]
        [DataType(DataType.Upload)]
        public IFormFile formFile { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string fileExtension { get; set; }

        // navigational purpose
        public virtual BugModel bug { get; set; }
        public virtual UserModel user { get; set; }
    }
}