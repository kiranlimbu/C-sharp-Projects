using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace bugspotAPI.Models
{
    public class TypeModel
    {
        [Key]
        public int typeId { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string typeName { get; set; }
    }
}