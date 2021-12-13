using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace bugspotAPI.Models
{
    public class ProjectModel
    {
        [Key]
        public int projectId { get; set; }

        public int? companyId { get; set; }

        [Required]
        [Column(TypeName = "varchar(50)")]
        public string projName { get; set; }

        [Column(TypeName = "varchar(500)")]
        public string projDescription { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTimeOffset startDate { get; set; } = System.DateTimeOffset.Now.Date;

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTimeOffset? endDate { get; set; }

        public bool archived { get; set; }
        public string UserModelId { get; set; }


        // navigational purpose
        public virtual CompanyModel company { get; set; }

        public virtual ICollection<BugModel> bugs { get; set; } = new HashSet<BugModel>();
        [NotMapped]
        public virtual ICollection<UserModel> members { get; set; } = new HashSet<UserModel>();

    }
}