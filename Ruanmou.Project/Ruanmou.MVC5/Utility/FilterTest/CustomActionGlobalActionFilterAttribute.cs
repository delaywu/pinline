using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ruanmou.MVC5.Utility.FilterTest
{
    public class CustomActionGlobalActionFilterAttribute : ActionFilterAttribute
    {
        private Logger logger = new Logger(typeof(CustomActionGlobalActionFilterAttribute));
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            this.logger.Info($"Global OnActionExecuting {this.Order}");
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            this.logger.Info($"Global OnActionExecuted {this.Order}");
        }

        //public override void OnResultExecuting(ResultExecutingContext filterContext)
        //{
        //    this.logger.Info($"Global OnResultExecuting {this.Order}");
        //}

        //public override void OnResultExecuted(ResultExecutedContext filterContext)
        //{
        //    this.logger.Info($"Global OnResultExecuted {this.Order}");
        //}
    }
}