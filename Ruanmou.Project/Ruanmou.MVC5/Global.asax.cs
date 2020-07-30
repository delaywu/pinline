using Ruanmou.MVC5.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Ruanmou.MVC5
{
    /// <summary>
    /// 部署IIS，端口2017，发布部署---直接项目文件部署
    /// 程序池--CLR4.0---集成模式
    /// 
    /// System.Web.Mvc  在nuget 搜索Microsoft.AspNet.Mvc
    /// 
    /// 今天是12最后一次课？当然不是！
    /// 开了一个新连接给大家上课，
    /// 从明天开始到下周，会给开通新链接的直播权限
    /// 
    /// </summary>
    public class MvcApplication : System.Web.HttpApplication
    {
        private Logger logger = new Logger(typeof(MvcApplication));
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();//注册区域
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);//注册全局的Filter
            RouteConfig.RegisterRoutes(RouteTable.Routes);//注册路由
            BundleConfig.RegisterBundles(BundleTable.Bundles);//合并压缩 ，打包工具 Combres
            ControllerBuilder.Current.SetControllerFactory(new ElevenControllerFactory());

            this.logger.Info("网站启动了。。。");
        }
        /// <summary>
        /// 全局式的异常处理，可以抓住漏网之鱼
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Application_Error(object sender, EventArgs e)
        {
            //Exception excetion = Server.GetLastError();
            //this.logger.Error($"{base.Context.Request.Url.AbsoluteUri}出现异常");
            //Response.Write("System is Error....");
            //Server.ClearError();

            //Response.Redirect
            //base.Context.RewritePath("/Home/Error?msg=")
        }

        protected void CustomHttpModuleEleven_CustomHttpModuleHandler(object sender, EventArgs e)
        {
            this.logger.Info("this is CustomHttpModuleEleven_CustomHttpModuleHandler");
        }


        /// <summary>
        /// 会在系统新增一个session时候触发
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Session_Start(object sender, EventArgs e)
        {
            HttpContext.Current.Application.Lock();
            object oValue = HttpContext.Current.Application.Get("TotalCount");
            if (oValue == null)
            {
                HttpContext.Current.Application.Add("TotalCount", 1);
            }
            else
            {
                HttpContext.Current.Application.Add("TotalCount", (int)oValue + 1);
            }
            HttpContext.Current.Application.UnLock();
            this.logger.Debug("这里执行了Session_Start");
        }

        /// <summary>
        /// 系统释放一个session的时候
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Session_End(object sender, EventArgs e)
        {
            this.logger.Debug("这里执行了Session_End");
        }

    }
}
