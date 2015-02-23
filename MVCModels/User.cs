//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MVCModels
{
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;
    using System.ComponentModel.DataAnnotations;
    [MetadataType(typeof(MVCModels.metadata.UserMetaData))]
    
    public partial class User
    {
        public User()
        {
            RoleList = SecurityUtil.getAllRolesMultiSelect(this.Roles);
        }
        public int id { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public string mobile { get; set; }
        public string address1 { get; set; }
        public string address2 { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string country { get; set; }
        public string zip { get; set; }
        public string phone { get; set; }
        public string usertype { get; set; }
        public Nullable<byte> isactive { get; set; }
        public Nullable<System.DateTime> createdon { get; set; }
        public Nullable<long> createdby { get; set; }
        public Nullable<System.DateTime> lastmodifiedon { get; set; }
        public string[] Roles { get; set; }
        
       
        public IEnumerable<SelectListItem> RoleList { get; set; }
    }
}
