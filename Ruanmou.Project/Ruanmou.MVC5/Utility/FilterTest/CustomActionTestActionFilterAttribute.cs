using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ruanmou.MVC5.Utility.FilterTest
{
    public class CustomActionTestActionFilterAttribute : ActionFilterAttribute
    {
        private Logger logger = new Logger(typeof(CustomActionTestActionFilterAttribute));
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            this.logger.Info($"Action OnActionExecuting {this.Order}");
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            this.logger.Info($"Action OnActionExecuted {this.Order}");
        }

        //public override void OnResultExecuting(ResultExecutingContext filterContext)
        //{
        //    this.logger.Info($"Action OnResultExecuting {this.Order}");
        //}

        //public override void OnResultExecuted(ResultExecutedContext filterContext)
        //{
        //    this.logger.Info($"Action OnResultExecuted {this.Order}");
        //}
    }
}