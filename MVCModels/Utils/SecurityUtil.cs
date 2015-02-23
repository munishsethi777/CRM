using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Security;
using System.Web.Mvc;
using System.Collections;
namespace MVCModels
{
    public class SecurityUtil
    {
        static List<SelectListItem> roleslist = null;
        static MultiSelectList roles = null;
        
        public static SelectList getRolesSelectList(object selectedValue = null)
        {
            if (roleslist == null)
            {
                string[] roles = Roles.GetAllRoles();
                roleslist = new List<SelectListItem>();
                for (int i = 0; i <= roles.Length-1; i++)
                {
                    roleslist.Add(new SelectListItem { Text = roles[i], Value = roles[i] });
                }
            }
            return new SelectList(roleslist, "Value", "Text", selectedValue);
        }

        public static MultiSelectList getAllRolesMultiSelect(string[] selectedIds)
        {
            if (roles == null)
            {
                var roleList = SecurityUtil.getRolesSelectList();
                roles =  new MultiSelectList(roleList, "Value", "Text", selectedIds);
            }
            return roles;
        }
    }

    
}
