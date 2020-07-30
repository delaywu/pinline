using Ruanmou.MVC5.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ruanmou.MVC5.Controllers
{
    /// <summary>
    /// 1 MVC5：Controller、Action、View
    /// 2 IIS部署，Global、Log4
    /// 3 数据传值的多种方式
    /// 
    /// 
    /// 广义MVC(Model--View-Controller)，
    /// V是界面  M是数据和逻辑  C是控制，把M和V链接起来
    /// 程序设计模式，一种设计理念，可以有效的分离界面和业务
    /// 
    /// 狭义MVC，是web开发框架， 
    /// V--Views   用户看到的视图内容
    /// C---Controllers  决定用户使用哪个视图，还能调用逻辑计算 
    /// 方法Action
    /// M--Models  数据传递模型，普通的实体
    /// 
    /// MVC就是返回页面？ 不是的
    /// 返回html--string---json--xml--file--图片
    /// 
    /// WebApi是返回数据的，为啥不都用MVC算了
    /// 其实不管是aspx/ashx/webapi/mvc,都是使用Http协议
    /// 所以一切的请求都可以实现的！ 
    /// Aspx：属于比较重的，默认有页面的生命周期---前后融合，viewstate---跟cs是一一对应
    /// Ashx: 属于轻量级的，没有页面的概念
    /// MVC：前后分离的，C可以任意指定视图，可以一套后台多套UI
    /// WepApi:专人做专事儿，管道都是独立的；RESTful，没有action
    ///         .net core二者又融合管道了
    ///         
    /// ViewData字典传值，里面是object，需要类型转换
    /// ViewBag dynamic传值，可以随便属性访问，运行时检测
    /// 以上二者是会覆盖的，后者为准
    /// model--适合复杂数据的传递,强类型
    /// TempData--临时数据，可以跨action后台传递，存在session里面，用一次就清理掉
    /// 
    /// Views--Web.Config是配置视图文件
    /// 
    /// masterpage--layout  默认是_layout  可以自行指定
    /// Global.asax--全局式 
    /// Application_Start 全局启动时执行，且只执行一次，非常适合做初始化---也可以静态构造函数
    /// 还可以有很多别的，下回分解
    /// 
    /// dynamic是个动态类型--运行时检测--编译时随便你写
    /// 利用委托的，性能比反射高，可以提供便利
    /// 弱类型语言的特点，方便做一些特殊处理
    /// 
    /// </summary>
    public class FirstController : Controller
    {
        public FirstController()
        {

        }


        private List<CurrentUser> _UserList = new List<CurrentUser>()
        {
            new CurrentUser()
            {
                Id=1,
                Name="Z",
                Account="Administrator",
                Email="57265177@qq.com",
                LoginTime=DateTime.Now,
                Password="123456"
            },
            new CurrentUser()
            {
                Id=2,
                Name="白天搬砖",
                Account="Administrator",
                Email="57265177@qq.com",
                LoginTime=DateTime.Now,
                Password="123456"
            },
            new CurrentUser()
            {
                Id=3,
                Name="晚上做梦",
                Account="Administrator",
                Email="57265177@qq.com",
                LoginTime=DateTime.Now,
                Password="123456"
            },
        };

        // GET: First
        /// <summary>
        ///  123  
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Index(int id = 3)
        {

            //base.HttpContext.Session
            //System.Web.Mvc.WebViewPage
            CurrentUser currentUser = this._UserList.FirstOrDefault(u => u.Id == id)
                ?? this._UserList[0];

            base.ViewData["CurrentUserViewData"] = this._UserList[0];
            base.ViewBag.CurrentUserViewBag = this._UserList[1];

            base.ViewData["TestProp"] = "cx";
            base.ViewBag.TestProp = "Tenk";
            base.TempData["TestProp"] = "Spider";//独立存储

            base.TempData["CurrentUserTempData"] = currentUser;

            if (id == 1 || id == 2 || id == 3)
                return View(this._UserList[2]);
            else if (id < 10)
                return View("~/Views/First/Index1.cshtml");
            else
                return base.RedirectToAction("TempDataShow");
        }
        //dynamic

        public ActionResult TempDataShow()
        {
            return View();
        }


        /// <summary>
        /// /First/IndexId/4 id是路由解析出来的，只有id参数可以这样
        /// /First/IndexId?id=3 url地址传递参数
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ViewResultBase IndexId(int id)
        {
            //各种的数据库增删改查
            return View();
        }
        public ViewResult IndexIdNull(int? id)
        {
            return View();
        }
        /// <summary>
        /// string可以为空
        /// /First/stringname?name=小白
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public string StringName(string name)
        {
            return $"This is {name}";
        }

        public string StringJson(string name)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(new
            {
                Id = 123,
                Name = name
            });
        }

        public JsonResult Json(int id, string name, string remark)
        {
            return new JsonResult()
            {
                Data = new
                {
                    Id = id,
                    Name = name ?? "X",
                    Remark = remark ?? "这里是默认的"
                },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
        //public FilePathResult FileTest()
        //{
        //    return File(@"D:\ruanmou\online12\20190612Advanced12Course51MVC5\about.txt", "");
        //}
    }
}
