using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InterviewPrepMVC.Models
{
    public class HandleException:FilterAttribute,IExceptionFilter
    {
        public void OnException(ExceptionContext filterContext)
        {
            filterContext.Result = new RedirectResult("~/Content/Error.html");
            filterContext.ExceptionHandled = true;
        }
    }
}