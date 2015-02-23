using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVCModels;
using MvcJqGrid;
using System.Web.Script.Serialization;
using MVCBusiness;
using WebMatrix.WebData;
using System.Web.Security;
namespace CRM.Controllers
{
    public class UserManagementController : BaseController
    {

        private static AccountsEntities db = new AccountsEntities();
        String Delete_SQL = "Delete from Users where id in ({0})";
        IHomeRepository objHomeRep = new HomeRepository();
        public ActionResult Index()
        {
            return View();
        }
        
        public ActionResult GetAllUsers(GridSettings gridSettings)
        {
            try
            {
                object objJsonResult = objHomeRep.GridDataForUsers(gridSettings);
                JsonResult result = Json(objJsonResult, JsonRequestBehavior.AllowGet);
                return result;
            }
            catch (Exception ex)
            {
            }
            return null;
        }

        [HttpPost]
        public ActionResult Delete(string sIds)
        {
            try
            {
                if (Request.IsAjaxRequest())
                {
                    int iCheck = objHomeRep.Delete(sIds, Delete_SQL);
                    if (iCheck > 0)
                    {
                        //TODO -- Remove User roles
                    }
                    JsonResult(iCheck > 0);
                }
                else
                {
                    NotAjaxJsonResult();
                }
            }
            catch (Exception Ex)
            {
                ExcetionJsonResult(Ex.Message);
            }
            return jSONResult;
        }



        public ActionResult Edit(int id)
        {
            User user = db.Users.Find(id);
            return View("Create", user);
        }

        public ActionResult Create()
        {
            User user = new User();
            return View(user);
        }

        public ActionResult ConfirmCreate(MVCModels.User user,string[] roles)
        {
            try
            {
                user.lastmodifiedon = DateTime.Now;
                int id = user.id;
                if (id > 0)
                {
                    user = db.Users.Find(id); 
                }
                else
                {
                    WebSecurity.CreateUserAndAccount(user.email, user.password);
                    id = WebSecurity.GetUserId(user.email);
                    user.id = id;
                    user.createdon = DateTime.Now;
                    user.isactive = 1;
                    user.usertype = "user";
                }
                UpdateModel(user);
                user.id = id;
                db.Entry(user).State = System.Data.EntityState.Modified;
                string[] exstingRoles = Roles.GetRolesForUser(user.email);
                if (exstingRoles.Length > 0)
                {
                    Roles.RemoveUserFromRoles(user.email, exstingRoles);
                }
                Roles.AddUserToRoles(user.email, roles);
                db.SaveChanges();
                ModelState.Clear();
                return View("Index");
            }
            catch (Exception e)
            {
                ModelState.AddModelError(string.Empty, "Exception: - " + e.Message);
                return View("Create", user);
            }
        }
       
        public ActionResult CreateRole()
        {
            return View();
        }
        public ActionResult ConfirmCreateRole(MVCModels.Role role)
        {
            try
            {
                role.createdon = DateTime.Now;
                role.lastmodifiedon = DateTime.Now;
                role.isactive = 1;
                role.createdby = 1;
                db.Roles.Add(role);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                ModelState.AddModelError(string.Empty, "Exception: - " + e.Message);
                return View("CreateRole", role);
            }
        }
    }
}
