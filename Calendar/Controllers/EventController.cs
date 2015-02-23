using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using MVCModels;
using MvcJqGrid;
using System.Data;
using MVCBusiness;
using System.Collections;
using Newtonsoft.Json;


namespace CRM.Controllers
{
    public class EventController : BaseController
    {
        AccountsEntities db = new AccountsEntities();
        IHomeRepository objHomeRep = new HomeRepository();
        String Delete_SQL = "Delete from CalendarEvents where id in ({0})";
      
        public ActionResult Index()
        {
            return View("Events");
        }

        [HttpPost]
        public JsonResult EventDetails(int id)
        {
            ViewBag.repeatingEventOn = null;
            CalendarEvent calEvent = db.CalendarEvents.Find(id);
            IEnumerable<CalendarEvent> repeatingEventOn = (IEnumerable<CalendarEvent>)db.CalendarEvents.Where(x => x.parenteventseq == calEvent.id);
            if (repeatingEventOn.Any())
            {
                ViewBag.repeatingEventOn = repeatingEventOn;
            }            
            var jsonResult = new
            {
                Data = RenderPartialViewToString("EventDetails", calEvent)
            };

            if (Request.IsAjaxRequest())
            {
                JsonResult result = Json(jsonResult, JsonRequestBehavior.AllowGet);
                return result;
            }
            return null;
        }
            
        public ActionResult ShowCalendar()
        {
            return View();
        }
        public ActionResult LoadCalendar()
        {
            try
            {
                IEnumerable objJsonResult = objHomeRep.CalendarDataForEvents();
                JsonResult result = Json(objJsonResult, JsonRequestBehavior.AllowGet);
                return result;
            }
            catch (Exception Ex)
            {
                //log.Error(Ex);
            }
            return null;
        }
        public ActionResult Create()
        {
            CalendarEvent calEvent = new CalendarEvent();
            calEvent.durationoption = DurationOptions.WithoutDuration.ToString();
            return View(calEvent);
        }

        public ActionResult GridDataForEvents(GridSettings objGrdSettings)
        {
            try
            {
                object objJsonResult = objHomeRep.GridDataForEvents(objGrdSettings);

                JsonResult result = Json(objJsonResult, JsonRequestBehavior.AllowGet);
                return result;
            }
            catch (Exception Ex)
            {
                //log.Error(Ex);
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
            CalendarEvent calEevent = db.CalendarEvents.Find(id);
            return View("Create", calEevent);
        }
       
        public ActionResult ConfirmCreate(MVCModels.CalendarEvent calEvent)
        {
            try
            {
                calEvent.eventtype = "calendar";
                calEvent.deparmentid = 0;
                calEvent.enddate = calEvent.startdate;
                calculateDuration(calEvent);
                if (calEvent.id > 0)
                {
                    DeleteChildEvents(calEvent);
                    calEvent = db.CalendarEvents.Find(calEvent.id);
                    UpdateModel(calEvent);
                    db.Entry(calEvent).State = System.Data.EntityState.Modified;
                }
                else
                {
                    db.CalendarEvents.Add(calEvent);
                }
                db.SaveChanges();
                db.Entry(calEvent).GetDatabaseValues();
                CreateChildEvents(calEvent);
                return View("Events");
            }
            catch (Exception e)
            {
                ModelState.AddModelError(string.Empty,"Exception: - " + e.Message);
                return View("Create", calEvent);
            }
            
        }

        private void calculateDuration(CalendarEvent calEvent){
            string durationOption = calEvent.durationoption;
            if (durationOption.Equals(DurationOptions.Untill.ToString()))
            {
                calEvent.enddate = calEvent.durationuntill ?? DateTime.Now; 
            }
            else if (durationOption.Equals(DurationOptions.DurationInMunites.ToString()))
            {
                double minutes = (double)calEvent.durationinminutes;
                DateTime enddate = calEvent.startdate;
                calEvent.enddate = enddate.AddMinutes(minutes);
            }
            
        }

        
        //Private API'S
        private void DeleteChildEvents(CalendarEvent calEvent)
        {
           int i =  db.Database.ExecuteSqlCommand("delete from CalendarEvents where parenteventseq = " + calEvent.id);
        }

        private void CreateChildEvents(CalendarEvent calEvent)
        {
            if (calEvent.isrepeated)
            {
                int? repCount = calEvent.repeatedweekly;
                DateTime startDate = calEvent.startdate;
                DateTime endDate = calEvent.enddate;
                 
                for (int i = 1; i < repCount; i++)
                {
                    CalendarEvent childEvent = db.CalendarEvents.Create();
                    db.CalendarEvents.Add(childEvent);
                    db.Entry<CalendarEvent>(childEvent).CurrentValues.SetValues(calEvent);
                    childEvent.id = 0;
                    childEvent.parenteventseq = calEvent.id;
                    string durationOption = calEvent.durationoption;
                    startDate = startDate.AddDays(7);
                    endDate = endDate.AddDays(7);
                    childEvent.startdate = startDate;
                    childEvent.enddate = endDate;                                 
                    db.SaveChanges();
                }
            }
        }

       

    }
}
