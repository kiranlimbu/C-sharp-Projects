using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace bugspotAPI.Models
{
    public class SeverityModel
    {
        [Key]
        public int severityId { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string severityName { get; set; }
    }
}