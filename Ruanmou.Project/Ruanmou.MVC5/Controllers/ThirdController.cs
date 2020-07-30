using Ruanmou.Bussiness.Interface;
using Ruanmou.Bussiness.Service;
using Ruanmou.EF.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ruanmou.MVC5.Controllers
{
    /// <summary>
    /// 1 IOC和MVC的结合，工厂的创建和Bussiness初始化
    /// 2 WCF搜索引擎的封装调用和AOP的整合
    /// 3 HTTP请求的本质，各种ActionResult扩展订制
    /// 
    /// MVC使用EF6：
    /// 完成EF6引入和数据访问
    /// 依赖的EF需要引入--数据库连接需要配置
    /// 
    /// 当然不是的，一直推崇IOC，
    /// 
    /// 
    /// MVC请求进来---路由匹配---找到控制器和Action---控制器是个普通的类，Action是个普通的实例方法
    /// ---是不是一定有个过程，叫实例化控制器---但是现在希望通过容器来实例化这个控制器
    /// 
    /// 路由匹配后得到控制器名称---MVCHandler---ControllerBuilder.GetControllerFactory()---然后创建的控制器的实例
    /// 
    /// DefaultControllerFactory默认的控制器工厂---把工厂换成自己实现的不就可以了？---ControllerBuilder有个SetControllerFactory
    /// 
    /// 1 继承DefaultControllerFactory
    /// 2 SetFactory----实例化控制器会进到这里
    /// 3 引入第三方容器--将控制器的实例化换成容器操作
    /// 完成了MVC+IOC+ORM的结合
    /// 
    /// 这个适合全部的控制器吗？ 控制器不是用的抽象-实例的配置，
    /// 是直接构造类型的实例
    /// 
    /// </summary>
    public class ThirdController : Controller
    {
        private IUserService _iUserService = null;
        private ICompanyService _iCompanyService = null;
        private IUserCompanyService _iUserCompanyService = null;
        /// <summary>
        /// 构造函数注入---控制器得是由容器来实例化
        /// </summary>
        /// <param name="userService"></param>
        /// <param name="companyService"></param>
        /// <param name="userCompanyService"></param>
        public ThirdController(IUserService userService, ICompanyService companyService, IUserCompanyService userCompanyService)
        {
            this._iUserService = userService;
            this._iCompanyService = companyService;
            this._iUserCompanyService = userCompanyService;
        }

        // GET: Third
        public ActionResult Index()
        {
            //JDDbContext context = new JDDbContext();
            //IUserService service = new UserService(context);
            IUserService service = this._iUserService;
            User user = service.Find<User>(2);
            return View();
        }
    }
}