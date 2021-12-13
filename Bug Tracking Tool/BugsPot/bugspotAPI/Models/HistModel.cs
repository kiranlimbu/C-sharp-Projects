using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace bugspotAPI.Models
{
    public class HistModel
    {
        [Key]
        public int historyId { get; set; }

        public int bugId { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy  HH:mm}")]
        public DateTimeOffset modDate { get; set; }

        [DisplayName("Changed By")]
        public string userId { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string changedItem { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string oldValue { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string newValue { get; set; }


        public virtual BugModel bug { get; set; }
        public virtual UserModel user { get; set; }
    }
}