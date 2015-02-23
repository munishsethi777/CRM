using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;


    public abstract class BaseController : Controller
    {
        protected const string FAIL = "fail";
        protected const string SUCCESS = "success";
        protected const string ERROR_OCCURED_PLEASE_TRY_AGAIN = "Error occured please try again";
        protected const string SERVER_VALIDATION_FAILED = "Server validation failed, Please fill the required field";
        protected const string IS_NOT_AJAX_REQUEST = "Not an ajax Request, Not processed the request";


        protected const string ADD_FAIL = "Add operation failed, Please try again";
        protected const string ADD_SUCCESS = "Record Inserted Successfully";

        protected const string UPDATE_FAIL = "Update operation failed, Please try again";
        protected const string UPDATE_SUCCESS = "Record updated successfully";

        protected const string DELETE_FAIL = "Delete operation failed, Please try again";
        protected const string DELETE_SUCCESS = "Record deleted successfully";
        protected const string DEACTIVATED_SUCCESS = "Deactivated successfully";

        protected const string MAIL_FAIL = "Mail sending failed";
        protected const string MAIL_SUCCESS = "Mail sent.";
        protected const string COMMA = ", ";


        protected string sJsonResult = FAIL;
        protected string sJsonMsg = ERROR_OCCURED_PLEASE_TRY_AGAIN;
        protected string sView = "";
        protected bool sIsUpdate = false;
        protected JsonResult jSONResult
        {
            get
            {

                var json = new
                {
                    result = sJsonResult,
                    message = sJsonMsg,
                    view = sView,
                    isUpdate = sIsUpdate
                };
                return Json(json, JsonRequestBehavior.AllowGet);
                //return " [{'result':'" + sJsonResult + "', 'message':'" + sJsonMsg + "'}]"; 

            }
        }
        protected void NotAjaxJsonResult()
        {
            sJsonResult = FAIL;
            sJsonMsg = IS_NOT_AJAX_REQUEST;
        }
        protected void ExcetionJsonResult(String exMessage)
        {
            sJsonResult = FAIL;
            sJsonMsg = exMessage;
        }
        protected void JsonResult(Boolean isUpdate, Boolean isSuccess)
        {

            if (isSuccess)
            {
                sJsonResult = SUCCESS;
                ModelState.Clear();
                ViewData.Clear();
                if (isUpdate)
                {
                    sJsonMsg = UPDATE_SUCCESS;
                }
                else
                {
                    sJsonMsg = ADD_SUCCESS;
                }
            }
            else
            {
                sJsonResult = FAIL;
                if (isUpdate)
                {
                    sJsonMsg = ADD_FAIL;
                }
                else
                {
                    sJsonMsg = UPDATE_FAIL;
                }
            }
           
        }

        protected void JsonResult(Boolean isSuccess)
        {
            if (isSuccess)
            {
                sJsonResult = SUCCESS;
                sJsonMsg = DELETE_SUCCESS;

                ModelState.Clear();
                ViewData.Clear();
            }
            else
            {
                sJsonResult = FAIL;
                sJsonMsg = DELETE_FAIL;
            }
            
        }

       
        /// <summary>
        /// Gets the current site session.
        /// </summary>
        protected string RenderPartialViewToString()
        {
            return RenderPartialViewToString(null, null);
        }

        protected string RenderPartialViewToString(string viewName)
        {
            return RenderPartialViewToString(viewName, null);
        }

        protected string RenderPartialViewToString(object model)
        {
            return RenderPartialViewToString(null, model);
        }

        protected string RenderPartialViewToString(string viewName, object model)
        {
            if (string.IsNullOrEmpty(viewName))
                viewName = ControllerContext.RouteData.GetRequiredString("action");

            ViewData.Model = model;

            using (StringWriter sw = new StringWriter())
            {
                ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
                ViewContext viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);

                return sw.GetStringBuilder().ToString();
            }
        }  

    }
