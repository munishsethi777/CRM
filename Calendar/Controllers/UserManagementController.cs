using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVCModels;
namespace Calendar.Controllers
{
    public class UserManagementController : Controller
    {

        private static AccountsEntities db = new AccountsEntities();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Create()
        {
            return View();
        }
        public ActionResult ConfirmCreate(MVCModels.User user)
        {
            try
            {
                //user.id = 1;
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
    }
}
