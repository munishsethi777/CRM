using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using MVCModels;

namespace CRM.Controllers
{
    public class EventController : Controller
    {
        private static AccountsEntities db = new AccountsEntities();
        //
        // GET: /Event/

        public ActionResult Index()
        {
            var db = new AccountsEntities();
            var events = from calEvents in db.CalendarEvents
                         select calEvents;
            return View(events.ToList());
        }

        public ActionResult ShowEvents()
        {
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }
       
        public ActionResult ConfirmCreate(MVCModels.CalendarEvent calEvent)
        {
            try
            {
                calEvent.eventtype = "calendar";
                calEvent.deparmentid = 0;
                calEvent.enddate = calEvent.startdate;
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
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                ModelState.AddModelError(string.Empty,"Exception: - " + e.Message);
                return View("Create", calEvent);
            }
            
        }

        public ActionResult Edit(int id)
        {
            CalendarEvent calEevent = db.CalendarEvents.Find(id);
            return View("Create", calEevent);
        }

        //public JsonResult Delete(int id)
        //{
        //    CalendarEvent calEvent = db.CalendarEvents.Find(id);
        //    return UserView("Delete", user);
        //}

        //
        // POST: /tblUser/Delete/5

     
        public ActionResult ConfirmDelete(int id)
        {
            try
            {
                CalendarEvent calEvents = db.CalendarEvents.Find(id);
                DeleteChildEvents(calEvents);
                db.CalendarEvents.Remove(calEvents);
                db.SaveChanges();                
            }
            catch (Exception e)
            {
                ModelState.AddModelError(string.Empty, "Exception: - " + e.Message);
            }
            return RedirectToAction("Index");
            
        }


        private void DeleteChildEvents(CalendarEvent calEvent)
        {
           int i =  db.Database.ExecuteSqlCommand("delete from CalendarEvents where parenteventseq = " + calEvent.id);
        }

        private void CreateChildEvents(CalendarEvent calEvent)
        {
            if (calEvent.isrepeated)
            {
                int? repCount = calEvent.repeatedweekly;
                for (int i = 1; i < repCount; i++)
                {
                    CalendarEvent childEvent = db.CalendarEvents.Create();
                    db.CalendarEvents.Add(childEvent);
                    db.Entry<CalendarEvent>(childEvent).CurrentValues.SetValues(calEvent);
                    childEvent.id = 0;
                    childEvent.parenteventseq = calEvent.id;
                    string durationOption = calEvent.durationoption;
                    if (durationOption.Equals(DurationOptions.Untill))
                    {

                    }
                    else if (durationOption.Equals(DurationOptions.DurationInMunites))
                    {

                    }
                    db.SaveChanges();
                }
            }
        }

       

    }
}
