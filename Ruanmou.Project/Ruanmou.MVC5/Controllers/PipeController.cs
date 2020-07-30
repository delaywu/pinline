using Ruanmou.Web.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace Ruanmou.MVC5.Controllers
{
    /// <summary>
    /// 1 Http请求处理流程
    /// 2 HttpApplication的事件
    /// 3 HttpModule
    /// 4 Global事件
    /// 
    /// 管道处理模式，大家平时开发都是在关注功能实现，
    /// 有没有从请求级层面来考虑程序，是怎么托管在IIS，又是怎么样响应的请求
    /// 
    /// Runtime--运行时
    /// Context--上下文
    /// 任何一个Http请求一定是有一个IHttpHandler来处理的 ashx aspx.cs  mvchttphandler 
    /// 任何一个Http请求就是一个HttpApplication对象来处理
    /// 然后处理过程固定包含：权限认证/缓存处理/Session处理/Cookie处理/生成html/输出客户端
    ///  与此同时，千千万万的开发者，又有各种各样的扩展诉求，任何一个环节都有可能要扩展，
    ///  我们来设计，该怎么设计？
    ///  这里用的是观察者模式，把固定的步骤直接写在handler里面，在步骤前&后分别放一个事件，
    ///  然后开发者可以对事件注册动作，等着请求进来了，然后就可以按顺序执行一下
    ///  详见 HttpProcessDemo
    ///  
    ///  对HttpApplication里面的事件进行动作注册的，就叫IHttpModule
    ///  自定义一个HttpModule--配置文件注册--然后任何一个请求都会执行Init里面注册给Application事件的动作
    ///  正常流程下，会按顺序执行19个事件
    ///  学习完HttpModule，我们可以做点什么有用的扩展？
    ///  1 日志-性能监控-后台统计数据
    ///  2 权限
    ///  3 缓存
    ///  4 页面加点东西
    ///  5 请求过滤--黑名单
    ///  6 MVC--就是一个Module扩展
    ///  
    ///  不适合的(不是全部请求的，就不太适合用module，因为有性能损耗)
    ///  1 多语言--根据cookie信息去查询不同的数据做不同的展示
    ///            如果是全部一套处理，最后httpmodule拦截+翻译，适合httpmodule
    ///  2 跳转到不同界面--也不适合
    ///  3 防盗链--针对一类的后缀来处理，而不是全部请求--判断--再防盗链
    ///  
    ///  HttpModule里面发布一个事件CustomHttpModuleHandler,在Global增加一个动作，
    ///  CustomHttpModuleEleven_CustomHttpModuleHandler(配置文件module名称_module里面事件名称)
    ///  请求响应时，该事件会执行
    ///  
    ///  HttpModule是对HttpApplication的事件注册动作
    ///  Global是对httpmodule里面的事件注册动作
    /// 
    /// 
    /// C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Config\web.config
    /// .NetFramework安装路径，是一个全局的配置，是当前电脑上任何一个网站的默认配置
    /// 
    /// 
    /// 1 HttpHandler及扩展，自定义后缀，图片防盗链等
    /// 2 RoutingModule,IRouteHandler、IHttpHandler
    /// 3 MVC扩展Route，扩展HttpHandle
    /// 
    /// 配置文件指定映射关系：后缀名与处理程序的关系(IHttpHandler---IHttpHandlerFactory)
    /// Http任何一个请求一定是由某一个具体的Handler来处理的，不管是成功还是失败
    /// 以前写aspx，请求访问的是物理地址，其实不然，请求的处理是框架设置的
    /// 
    /// 所谓管道处理模型，其实就是后台如何处理一个Http请求
    /// 定义多个事件完成处理步骤，每个事件可以扩展动作(httpmodule)，
    /// 最后有个httphandler完成请求的处理，这个过程就是管道处理模型
    /// 还有一个全局的上下文环境httpcontext，无论参数，中间结果 最终结果，都保存在其中
    /// 
    /// 自定义handler处理，就是可以处理各种后缀请求，可以加入自己的逻辑
    /// 如果没有--请求都到某个页面--传参数---返回图片
    /// 防盗链---加水印---伪静态---RSS--robot--trace.axd
    /// 
    /// MVC里面不是controller action？其实是由 MvcHandler来处理请求，期间完成对action调用的
    /// 
    /// 网站启动时---对RouteCollection进行配置
    /// 把正则规则和RouteHandler（提供httphandler）绑定，放入RouteCollection
    /// 
    /// 请求来临时---用RouteCollection进行匹配
    /// 所谓MVC框架，其实就是在Asp.Net管道上扩展的，在PostResolveCache事件扩展了UrlRoutingModule，会在任何请求进来后，先进行路由匹配，如果匹配上了，就指定httphandler；没有匹配就还是走原始流程
    /// 
    /// 扩展自己的route，写入routecollection，可以自定义规则完成路由
    /// 扩展httphandle，就可以为所欲为，跳出MVC框架
    /// </summary>
    public class PipeController : Controller
    {
        // GET: Pipe
        public ActionResult Index()
        {
            //HttpRuntime.ProcessRequest(null);
            //HttpApplication
            return View();
        }

        #region HttpModule
        public ActionResult Module()
        {
            HttpApplication app = base.HttpContext.ApplicationInstance;

            List<SysEvent> sysEventsList = new List<SysEvent>();
            int i = 1;
            foreach (EventInfo e in app.GetType().GetEvents())
            {
                sysEventsList.Add(new SysEvent()
                {
                    Id = i++,
                    Name = e.Name,
                    TypeName = e.GetType().Name
                });
            }

            List<string> list = new List<string>();
            foreach (string item in app.Modules.Keys)
            {
                list.Add($"{item}: {app.Modules.Get(item)}");
            }
            ViewBag.Modules = string.Join(",", list);

            return View(sysEventsList);
        }
        #endregion

        #region MyRegion
        public ActionResult Handler()
        {
            base.ViewBag.HttpHandler = base.HttpContext.CurrentHandler.GetType().FullName;
            //base.RouteData.Values //路由匹配后，获取的信息
            return View();
        }

        public ActionResult Refuse()
        {
            return View();
        }
        #endregion
    }



    public class HttpProcessDemo
    {
        public class HttpApplicationDemo : IHttpHandler
        {
            public bool IsReusable => true;

            public event Action BeginRequest;
            public event Action EndRequest;
            public event Action PreSomething1Handler;
            public event Action PostSomething1Handler;
            public event Action PreSomething2Handler;
            public event Action PostSomething2Handler;
            public event Action PreSomething3Handler;
            public event Action PostSomething3Handler;
            public event Action PreSomething4Handler;
            public event Action PostSomething4Handler;
            public event Action PreSomething5Handler;
            public event Action PostSomething5Handler;
            public event Action PreSomething6Handler;
            public event Action PostSomething6Handler;
            public void ProcessRequest(HttpContext context)
            {
                this.BeginRequest?.Invoke();

                this.PreSomething1Handler?.Invoke();
                Console.WriteLine("Something 1");
                this.PostSomething1Handler?.Invoke();

                this.PreSomething2Handler?.Invoke();
                Console.WriteLine("Something 2");
                this.PostSomething2Handler?.Invoke();
                this.PreSomething3Handler?.Invoke();
                Console.WriteLine("Something 3");
                this.PostSomething3Handler?.Invoke();
                this.PreSomething4Handler?.Invoke();
                Console.WriteLine("Something 4");
                this.PostSomething4Handler?.Invoke();

                this.PreSomething5Handler?.Invoke();
                Console.WriteLine("Something 5");
                this.PostSomething5Handler?.Invoke();
                this.PreSomething6Handler?.Invoke();
                Console.WriteLine("Something 6");
                this.PostSomething6Handler?.Invoke();

                this.EndRequest?.Invoke();
            }
            //任何请求进来，只能是 123456
            //事件升级后，可以在程序启动时，实例化HttpApplicationDemo后，可以给事件注册动作，请求再进来时，处理不仅是123456了，还有多个事件里面的动作
        }


    }

}

/*
 module执行的顺序是什么
V
悟丶
构造函数之后吗
V
民工甲
老师，能不能让某些类型的请求只让我们自定义的module处理，不走其他的module
     */
