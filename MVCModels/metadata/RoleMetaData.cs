using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
namespace MVCModels.metadata
{
    class RoleMetaData
    {
        public int id { get; set; }
        [Display(Name = "Role Name")]
        [Required]
        public string name { get; set; }
        [Display(Name = "Description")]
        public string description { get; set; }
        [Display(Name = "Active")]
        public Nullable<byte> isactive { get; set; }
        public Nullable<System.DateTime> createdon { get; set; }
        public Nullable<long> createdby { get; set; }
        public Nullable<System.DateTime> lastmodifiedon { get; set; }
    }
}
