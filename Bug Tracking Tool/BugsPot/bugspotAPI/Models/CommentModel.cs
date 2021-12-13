using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace bugspotAPI.Models
{
    public class CommentModel
    {
        [Key]
        public int commentId { get; set; }

        public int bugId { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy HH:mm}")]
        public DateTimeOffset writenDate { get; set; } = DateTimeOffset.Now;

        [DisplayName("Writen By")]
        public string userId { get; set; }

        public string content { get; set; }


        // navigational purpose
        public virtual BugModel bug { get; set; }
        public virtual UserModel user { get; set; }

        
    }
}