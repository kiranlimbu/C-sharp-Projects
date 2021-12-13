using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace bugspotAPI.Models
{
    public class PriorityModel
    {
        [Key]
        public int priorityId { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string priorityName { get; set; }
    }
}