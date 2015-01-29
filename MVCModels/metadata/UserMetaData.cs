using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
namespace MVCModels.metadata
{
    class UserMetaData
    {
        public int id { get; set; }
        [Display(Name = "Full Name")]
        [Required]
        public string name { get; set; }
        [Display(Name = "Email Id")]
        [Required]
        public string email { get; set; }
        [Display(Name = "Password")]
        [Required]
        public string password { get; set; }
        [Display(Name = "Mobile number")]
        [Required]
        public string mobile { get; set; }
        [Display(Name = "Address 1")]
        public string address1 { get; set; }
        [Display(Name = "Address 2")]
        public string address2 { get; set; }
        [Display(Name = "City")]        
        public string city { get; set; }
        [Display(Name = "State")]
        public string state { get; set; }
        [Display(Name = "Country")]
        public string country { get; set; }
        [Display(Name = "Zip")]
        public string zip { get; set; }
        [Display(Name = "Phone number")]
        public string phone { get; set; }
        public string usertype { get; set; }
    }
}
