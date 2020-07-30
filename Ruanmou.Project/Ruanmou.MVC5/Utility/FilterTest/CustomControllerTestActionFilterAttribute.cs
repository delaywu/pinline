using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ruanmou.MVC5.Utility.FilterTest
{
    public class CustomControllerTestActionFilterAttribute : ActionFilterAttribute
    {
        private Logger logger = new Logger(typeof(CustomControllerTestActionFilterAttribute));
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            this.logger.Info($"Controller OnActionExecuting {this.Order}");
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            this.logger.Info($"Controller OnActionExecuted {this.Order}");
        }

        //public override void OnResultExecuting(ResultExecutingContext filterContext)
        //{
        //    this.logger.Info($"Controller OnResultExecuting {this.Order}");
        //}

        //public override void OnResultExecuted(ResultExecutedContext filterContext)
        //{
        //    this.logger.Info($"Controller OnResultExecuted {this.Order}");
        //}
    }
}