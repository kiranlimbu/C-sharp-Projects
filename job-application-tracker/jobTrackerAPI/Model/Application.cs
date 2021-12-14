using System;
using System.ComponentModel.DataAnnotations;

namespace jobTrackerAPI.Model
{
    public class Application
    {
        [Key]
        public int id {get;set;}

        [StringLength(100)]
        public string company {get; set;}
        
        [StringLength(50)]
        public string position {get;set;}

        [StringLength(100)]
        public string website {get;set;}

        [StringLength(150)]
        public string address {get;set;}

        [StringLength(50)]
        public string contact {get;set;}

        [StringLength(11)]
        public string phone {get;set;}


        public DateTime dateApplied {get;set;}
        public string notes {get;set;}

        public Application()
        {
            dateApplied = DateTime.Now.Date;
        }
    }
}