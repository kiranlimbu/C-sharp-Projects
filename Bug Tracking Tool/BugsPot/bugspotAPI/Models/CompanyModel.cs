using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace bugspotAPI.Models
{
    public class CompanyModel
    {
        [Key]
        public int companyId { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string coName { get; set; }

        [Column(TypeName = "varchar(200)")]
        public string coDescription { get; set; }


        public virtual ICollection<UserModel> members { get; set; }
        public virtual ICollection<ProjectModel> projects { get; set; }
        public virtual ICollection<InviteModel> invites { get; set; }
    }
}