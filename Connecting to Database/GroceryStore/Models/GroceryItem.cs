using System;
using System.ComponentModel.DataAnnotations;


namespace GroceryStore.Models
{
    public class GroceryItem
    {
        [Key]
        public int ItemID { get; set; }
        public string ItemName { get; set; }
        public decimal ItemPrice { get; set; }
    }
}
