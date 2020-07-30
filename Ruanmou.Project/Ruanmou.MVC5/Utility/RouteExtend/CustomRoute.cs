using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Ruanmou.MVC5.Utility.RouteExtend
{
    public class CustomRoute : RouteBase
    {
        /// <summary>
        /// 如果是Chrome/74.0.3729.169 版本，允许正常访问
        /// 否则 跳转提示页
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        public override RouteData GetRouteData(HttpContextBase httpContext)
        {
            //httpContext.Request.Url.AbsoluteUri
            if (httpContext.Request.UserAgent.Contains("Chrome/74.0.3729.169"))
            {
                return null;//继续后面的
            }
            else
            {
                RouteData routeData = new RouteData(this, new MvcRouteHandler());//还是走mvc流程
                routeData.Values.Add("controller", "pipe");
                routeData.Values.Add("action", "refuse");
                return routeData;//中断路由匹配
            }
        }
        public override VirtualPathData GetVirtualPath(RequestContext requestContext, RouteValueDictionary values)
        {
            return null;
        }
    }

    public class CustomMvcRouteHandler : MvcRouteHandler
    {
        protected override IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            return new CustomHttpHandler();
        }
    }

    public class CustomHttpHandler : IHttpHandler
    {
        public bool IsReusable => true;

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/html";
            context.Response.WriteFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Web.config"));
            //看日志
        }
    }
}