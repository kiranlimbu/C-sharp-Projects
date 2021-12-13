using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace bugspotAPI.Models
{
    public class BugModel
    {
        [Key]
        public int bugId { get; set; }

        public int projectId { get; set; }

        [Required]
        [Column(TypeName = "varchar(200)")]
        public string title { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTimeOffset openDate { get; set; } = DateTimeOffset.Now;
        
        [DataType(DataType.Date)]
        public DateTimeOffset? lastMod { get; set; } // ? = nulls are acceptable

        public int typeId { get; set; }
        public int statusId { get; set; }
        public int priorityId { get; set; }
        public int severityId { get; set; }

        public bool archived { get; set; }

        public string reporterId { get; set; }

        public string developerId { get; set; }

        public string description { get; set; }

        public string stepsToProd { get; set; }

        public string actualRes { get; set; }

        public string expectedRes { get; set; }

        

        // navigational purpose (not stored in database)
        public virtual ProjectModel project { get; set; }
        public virtual UserModel reporter { get; set; }
        public virtual UserModel developer { get; set; }
        public virtual SeverityModel severity { get; set; }
        public virtual PriorityModel priority { get; set; }
        public virtual StatusModel status { get; set; }
        public virtual TypeModel type { get; set; }


        public ICollection<AttachmentModel> attachments { get; set; } = new HashSet<AttachmentModel>();
        public ICollection<CommentModel> comments { get; set; } = new HashSet<CommentModel>();
        public ICollection<HistModel> history { get; set; } = new HashSet<HistModel>();
        public ICollection<NotifiModel> notifications { get; set; } = new HashSet<NotifiModel>();
    }
}