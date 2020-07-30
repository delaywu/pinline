using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ruanmou.MVC5.Controllers
{
    /// <summary>
    /// 1 Route使用和扩展，Area
    /// 2 Razor语法，前后端语法混编
    /// 3 Html扩展控件，后端封装前端
    /// 4 模板页Layout,局部页PartialView 
    /// 
    /// MvcApplication--Application_Start--RegisterRoutes--给RouteCollection添加规则
    /// 请求进到网站--X--请求地址被路由按顺序匹配--遇到一个吻合的结束---就到对应的控制器和action
    /// 
    /// 因为一个Web项目可以非常大非常复杂，多人合作开发，命名就成问题了，Area可以把项目拆分开，方便团队合作；演变到后面可以做成插件式开发：
    /// MvcApplication--Application_Start--AreaRegistration.RegisterAllAreas()---其实就是把SystemAreaRegistration给注册下---添加URL地址规则--请求来了就匹配(area在普通的之前)
    /// 
    /// 众所周知，MVC请求的最后是反射调用的controller+action，信息来自于url+route，路由匹配时，只能找到action和controller，  其实还有个步骤，扫描+存储，在bin里面找Controller的子类，然后把命名空间--类名称+全部方法都存起来
    /// 
    /// a 控制器类可以出现在MVC项目之外，唯一的规则就是继承自Controller
    /// b Area也可以独立开，规则是必须有个继承AreaRegistration
    /// 增加区域后需要指定命名空间
    /// 
    /// Razor语法：cshtml本质是一个类文件，混编了html+cs代码
    /// 写后台代码：行内--单行--多行--关键字
    /// 后台代码写html：@:   闭合的html标签  <text></text>
    /// 
    /// Html扩展控件：封装个方法，自动生成html
    ///               后端一次性完成全部内容，而且html标签闭合
    ///               我们还可以自行封装这种扩展方法
    ///     但是这个已经不流行了，就是UI改动需要重新发布
    ///     更多应该是前后分离，写前端的人是不会懂后端的写法
    ///     
    /// Layout
    /// Masterpage--layout  默认是_layout  可以自行指定
    ///   @Styles.Render("~/Content/css") 使用样式包
    ///   @Scripts.Render("~/bundles/modernizr") 使用js包
    ///   @RenderBody() 就是页面的结合点
    ///   @RenderSection("scripts", required: false)
    ///   
    /// partialPage局部页---ascx控件,是没有自己的ACTION
    /// @{ Html.RenderPartial("PartialPage", "这里是Html.RenderPartial"); }
    ///  @Html.Partial("PartialPage", "这里是Html.Partial")
    /// 
    /// 子请求
    /// @Html.Action("ChildAction", "Second", new { name = "Html.Action" })
    /// @{Html.RenderAction("ChildAction", "Second", new { name = "Html.RenderAction" });}
    /// 有action,也可以传参数
    /// [ChildActionOnly]//只能被子请求访问  不能独立访问
    /// 
    /// </summary>
    public class SecondController : Controller
    {
        public SecondController()
        {

        }

        // GET: Second
        public ActionResult Index()
        {
            return View();
        }

        public string String()
        {
            return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff");
        }

        public string Time(int year, int month, int day)
        {
            return $"当前传入日期：{year}-{month}-{day}";
        }

        public ViewResult RazorExtend()
        {
            return View();
        }

        [ChildActionOnly]//只能被子请求访问  不能独立访问
        public ViewResult ChildAction(string name)
        {
            base.ViewBag.Name = name;
            return View();
        }
    }
}