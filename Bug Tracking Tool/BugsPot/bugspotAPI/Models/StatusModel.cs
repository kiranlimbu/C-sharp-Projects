using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace bugspotAPI.Models
{
    public class StatusModel
    {
        [Key]
        public int statusId { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string statusName { get; set; }
    }
}