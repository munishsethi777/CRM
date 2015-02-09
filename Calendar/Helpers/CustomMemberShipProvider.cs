using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebMatrix.WebData;
using MVCModels;
using System.Web.Security;
namespace CRM.Helpers
{
    public class CustomMembershipProvider : SimpleMembershipProvider 
    {

        
        public override bool ValidateUser(string username, string password)
        {
            return true; // base.ValidateUser(username, password);
        }
    }
}