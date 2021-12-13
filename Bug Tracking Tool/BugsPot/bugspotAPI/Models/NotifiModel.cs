using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace bugspotAPI.Models
{
    public class NotifiModel
    {
        [Key]
        public int notificationId { get; set; }

        public int bugId { get; set; }

        [Required]
        [Column(TypeName = "varchar(100)")]
        public string title { get; set; }

        [Column(TypeName = "varchar(500)")]
        public string message { get; set; }

        [DataType(DataType.Date)]
        public DateTimeOffset created { get; set; } = DateTimeOffset.Now;

        public string senderId { get; set; }

        public string receiverId { get; set; }

        public bool viewed { get; set; }


        // navigation
        public virtual BugModel bug { get; set; }
        public virtual UserModel sender { get; set; }
        public virtual UserModel receiver { get; set; }
    }
}