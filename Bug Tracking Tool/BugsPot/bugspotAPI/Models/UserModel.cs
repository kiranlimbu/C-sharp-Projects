using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace bugspotAPI.Models
{
    public class UserModel : IdentityUser
    {
        [Required]
        [Column(TypeName = "varchar(50)")]
        public string fName { get; set; }

        [Required]
        [Column(TypeName = "varchar(50)")]
        public string lName { get; set; }

        [NotMapped]
        public string fullName { get {return $"{fName} {lName}"; }}

        [Required]
        [EmailAddress]
        [Column(TypeName = "nvarchar(100)")]
        public override string Email { get; set; }
        
        [DataType(DataType.Password)]
        [JsonIgnore]
        public string password { get; set; }

        [NotMapped]
        [DataType(DataType.Password)]
        [Compare("password", ErrorMessage = "The password did not match!")]
        public string confirmPassword { get; set; }

        public int? companyId { get; set; }


        // Profile Image
        [NotMapped]
        [DataType(DataType.Upload)]
        public IFormFile profileImage { get; set; }
        public string imageName { get; set; }
        public byte[] imageData { get; set; }
        public string fileExtension { get; set; }

        // navigation
        public virtual CompanyModel company { get; set; }

        public virtual ICollection<ProjectModel> projects { get; set; } = new HashSet<ProjectModel>();
    }
}