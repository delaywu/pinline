using Ruanmou.Bussiness.Interface;
using Ruanmou.EF.Model;
using Ruanmou.MVC5.Utility;
using Ruanmou.MVC5.Utility.Filter;
using Ruanmou.MVC5.Utility.FilterTest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ruanmou.MVC5.Controllers
{
    /// <summary>
    /// 1 Filter原理和AOP面向切面编程
    /// 2 全局异常处理：HandleErrorAttribute
    /// 3 IActionFilter IResultFilter扩展订制
    /// 4 Filter全总结，实战框架中AOP解决方案
    /// 
    /// AOP面向切面编程
    /// Filter原理：控制器实例化之后---ActionInvoke前后---通过检测预定义Filter并且执行它---达到AOP的目的
    /// 
    /// 开发日常工作：查bug 解决bug 写bug
    /// 
    /// HandleErrorAttribute+Application_Error(粒度不一样)
    /// 
    /// 
    /// IActionFilter&IResultFilter
    /// 
    /// 
    /// a 避免UI直接看到异常
    /// 
    /// 
    /// 
    /// Global OnActionExecuting
    /// Controller OnActionExecuting
    /// Action OnActionExecuting
    /// Action真实执行
    /// Action OnActionExecuted
    /// Controller OnActionExecuted
    /// Global OnActionExecuted
    /// 
    /// 不同注册位置生效顺序--全局/控制器/Action
    /// 同一位置按照先后顺序生效
    /// (不设置Order默认是1) 设置后是按照从小到大执行
    /// 俄罗斯套娃
    /// 
    /// ActionFilter能干啥？
    /// 日志  参数检测-过滤参数  缓存  重写视图 压缩 
    /// 防盗链  统计访问量--限流
    /// 不同的客户端跳转不同的页面
    /// 异常--权限：当然可以做，但是不合适，专业的对口
    /// 
    /// filter真的这么厉害，有没有什么局限性？？！！
    /// 虽然很丰富，但是只能是以Action为单位
    /// Action内部调用别的类库，加操作，做不到！
    /// 这种就得靠IOC+AOP扩展
    /// 
    /// ActionFilter 即使Action返回string 甚至Null  
    /// 4个方法都是会生效的
    /// </summary>    
    //[CustomControllerTestActionFilterAttribute]
    public class SixthController : Controller
    {
        #region Identity
        private Logger logger = new Logger(typeof(SixthController));

        private IUserService _iUserService = null;
        private ICompanyService _iCompanyService = null;
        private IUserCompanyService _iUserCompanyService = null;
        /// <summary>
        /// 构造函数注入---控制器得是由容器来实例化
        /// </summary>
        /// <param name="userService"></param>
        /// <param name="companyService"></param>
        /// <param name="userCompanyService"></param>
        public SixthController(IUserService userService, ICompanyService companyService, IUserCompanyService userCompanyService)
        {
            this._iUserService = userService;
            this._iCompanyService = companyService;
            this._iUserCompanyService = userCompanyService;
        }
        #endregion 

        public ActionResult Index()
        {
            return View();
        }
        #region 多异常情况
        //1 Action异常,没被catch                       T 
        //2 Action异常,被catch                         F
        //3 Action调用Service异常                      T  异常冒泡 
        //4 Action正常视图出现异常                     T  ExecuteResult是包裹在try里面的
        //5 控制器构造出现异常                         F  控制器构造后才有Filter
        //6 Action名称错误                             F  因为请求其实都没进mvc流程
        //7 任意错误地址                               F
        //8 权限Filter异常                             T  权限fileter也是在try里面的
        //全局注册，能不能进入自定义的异常Filter
        //TFTFTFTF
        //不卡的刷个1
        #endregion

        //[CustomHandleErrorAttribute]
        public ActionResult Exception()
        {
            int i = 0;
            int k = 10 / i;
            return View();
        }
        public ActionResult ExceptionCatch()
        {
            try
            {
                int i = 0;
                int k = 10 / i;
            }
            catch (Exception ex)
            {
                this.logger.Error(ex.Message);
            }
            return View();
        }

        public ActionResult ExceptionService()
        {
            this._iCompanyService.TestError();
            return View();
        }

        public ActionResult ExceptionView()
        {
            return View();
        }
        [CustomTestErrorAuthorizeAttribute]
        public ActionResult ExceptionAuthorize()
        {
            return View();
        }


        //[CustomActionFilter]

        [CustomActionGlobalActionFilter(Order = 12)]
        [CustomControllerTestActionFilter(Order = 24)]
        [CustomActionTestActionFilterAttribute(Order = 6)]
        [CompressActionFilterAttribute]

        public ActionResult Show()
        {
            int i = 1;

            int k = 2;

            int m = i + k;

            this._iCompanyService.Find<Company>(1);

            return View();
        }
        //var result=Action.Invoke();

        //result.ExecuteResult();
        //浏览器没有权限获取mac
        //子线程异常--1 WaitAll--主线程异常--当然可以呀  2没有的话线程自己的事儿，主线程不知道，当然不行呀

    }
}