using Ruanmou.MVC5.Utility.RouteExtend;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Ruanmou.MVC5
{
    /// <summary>
    /// 路由：映射，按照规则转发
    /// http://localhost:2017/Second/index
    /// 从Second/index开始匹配
    /// 
    /// 路由是按照注册顺序进行匹配，遇到第一个吻合的就结束匹配；每个请求只会被一个路由匹配上
    /// </summary>
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            //忽略路由  正则表达式  {resource}表示变量   a.axd/xxxx   resource=a   pathInfo=xxxx
            //.axd是历史原因，最开始都是webform，请求都是.aspx后缀，IIS根据后缀转发请求；MVC出现了，没有后缀，IIS6以及更早版本，打了个补丁，把mvc的请求加上个.axd的后缀，然后这种都转发到网站----新版本的IIS已经不需要了，遇到了就直接忽略，还是走原始流程
            routes.IgnoreRoute("CustomService/{*pathInfo}");//以CustomService开头，都不走路由

            //routes.MapRoute(
            //    name: "About",
            //    url: "About",
            //    defaults: new { controller = "Home", action = "About", id = UrlParameter.Optional }
            //    );//固定路由，/Home/About----About

            //routes.MapRoute(
            //   name: "Test",
            //   url: "Test/{action}/{id}",
            //   defaults: new { controller = "Second", action = "Index", id = UrlParameter.Optional }
            //   );//修改控制器，

            //routes.MapRoute(
            //  name: "Regex",
            //  url: "{controller}/{action}_{year}_{month}_{day}",
            //  defaults: new { controller = "Second", action = "Index", id = UrlParameter.Optional },
            //  constraints: new { year = @"\d{4}", month = @"\d{2}", day = @"\d{2}" }
            //  );
            //http://localhost:2017/second/Time_2019_06_13    Regex
            //http://localhost:2017/second/Time_2019_6_13 失败 
            //http://localhost:2017/second/Time?year=2019&month=6&day=13  Default
            //http://localhost:2017/test/Time?year=2019&month=6&day=13    Test
            //http://localhost:2017/test/Time_2019_06_13  失败的，只会被一个路由匹配

            routes.Add("chrome", new CustomRoute());

            routes.Add("config", new Route("Eleven/{path}", new CustomMvcRouteHandler()));

            //常规路由,一般来说，我们不怎么扩展这个路由
            routes.MapRoute(
                name: "Default",//路由名称，RouteCollection是key-value，key 避免重复
                url: "{controller}/{action}/{id}",//正则规则：两个斜线 3个变量
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
                //默认值 没有id变量 就是UrlParameter.Optional  没有action就是index  没有controller是home
                , namespaces: new string[] { "Ruanmou.MVC5.Controllers", "Ruanmou.MVC5.Plugins" }
                //上周漏掉了，增加区域后需要指定命名空间
            );

        }
    }
}
