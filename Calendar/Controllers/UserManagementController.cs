using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVCModels;
using MvcJqGrid;
using System.Web.Script.Serialization;
namespace CRM.Controllers
{
    public class UserManagementController : Controller
    {

        private static AccountsEntities db = new AccountsEntities();
        public ActionResult Index()
        {
            return View();
        }
        
        public ActionResult GetAllUsers(GridSettings gridSettings)
        {
            try
            {
                IEnumerable<User> users = db.Users;
                int objtot = users.Count();

                //int64 objtot = convert.toint64(objorderdashboard.tables[1].rows[0]["cnt"]);

                if (users == null)
                    return null;

                var jsondata = new
                {
                    total = objtot / gridSettings.PageSize + 1,
                    page = gridSettings.PageIndex,
                    records = objtot,
                    rows = new JavaScriptSerializer().Serialize(users)
                };
                JsonResult result = Json(jsondata);
                return result;

            }
            catch (Exception ex)
            {
            }
            return null;
        }
        public ActionResult Create()
        {
            return View();
        }
        public ActionResult ConfirmCreate(MVCModels.User user)
        {
            try
            {
                user.createdon = new DateTime();
                user.lastmodifiedon = new DateTime();
                user.isactive = 1;
                user.usertype = "user";
                db.Users.Add(user);
                db.SaveChanges();
                return RedirectToAction("Index");
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
