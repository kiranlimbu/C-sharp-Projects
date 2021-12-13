using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace bugspotAPI.Models
{
    public class InviteModel
    {
        [Key]
        public int inviteId { get; set; }

        [DataType(DataType.Date)]
        public DateTimeOffset inviteDate { get; set; } = DateTimeOffset.Now;

        [DataType(DataType.Date)]
        public DateTimeOffset joinDate { get; set; }

        public Guid companyToken { get; set; }

        public int companyId { get; set; }
        public int projectId { get; set; }
        public string invitorId { get; set; }
        public string inviteeId { get; set; }

        [Column(TypeName = "varchar(100)")]
        public string inviteeEmail { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string inviteeFName { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string inviteeLName { get; set; }
        public bool IsValid { get; set; }


        // navigation
        public virtual CompanyModel company { get; set; }
        public virtual ProjectModel project { get; set; }
        public virtual UserModel invitor { get; set; }
        public virtual UserModel invitee { get; set; }
    }
}