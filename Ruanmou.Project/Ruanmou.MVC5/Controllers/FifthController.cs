using Ruanmou.Framework.Extension;
using Ruanmou.Framework.ImageHelper;
using Ruanmou.MVC5.Utility;
using Ruanmou.MVC5.Utility.Filter;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ruanmou.MVC5.Controllers
{
    /// <summary>
    /// 1 用户登录/退出功能实现
    /// 2 AuthorizeAttribute权限验证
    /// 3 Filter注册和匿名支持
    /// 4 解读Filter生效机制
    /// 
    /// 登陆后有权限控制，有的页面的是需要用户登录后才能访问的
    /// 需要在访问页面时增加登陆验证
    /// 也不能每个action都来一遍
    /// 
    /// 自定义CustomAuthorizeFilter，
    /// 1 方法注册----单个方法生效
    /// 2 控制器注册--控制器全部方法生效
    /// 3 全局注册--全部控制器的全部方法生效
    /// 
    /// AllowAnonymous匿名，单独加特性是没用的
    /// 其实需要验证时支持，甚至说可以自定义一些特性一样可以生效
    /// 
    /// a 用户访问A页面--没有权限--去登陆--成功跳回A页面
    ///   前端更多加个参数    
    ///   用session,验证失败记录url，登陆成功使用url
    ///   
    /// b 如果是ajax请求时没登录，需要返回规定格式的Ajax数据
    /// 
    /// c 特性使用范围
    ///   希望特性通用在不同的系统，不同的登陆地址
    ///   
    /// Filter生效机制：
    /// 控制器已经实例化了-- ExecuteCore--找到方法名字--ControllerActionInvoker.InvokeAction
    /// ---找到全部的Filter特性---InvokeAuthorize--result不为空，直接InvokeActionResult
    /// --为空就正常执行Action
    /// 
    /// 有了一个类型实例，有一个方法名称，希望你反射执行
    /// 在找到方法后，执行方法前---可以检测下特性
    /// （1 来自全局  2 找控制器 3 找方法的）
    /// ---特性是我预定义--只找这三类---按类执行
    /// ---定个标识，为空正常，不为空就跳转--正常就继续执行方法
    /// 
    /// 
    /// 
    /// </summary>
    [CustomAuthorize("~/Fifth/Login")]
    [CustomAllowAnonymous]
    public class FifthController : Controller
    {
        public FifthController()
        {
            throw new Exception("FifthController ctor 异常");
        }


        //[Authorize]
        //[CustomAuthorize]
        [CustomAllowAnonymous]
        public ActionResult Index()
        {
            //if (base.HttpContext.Session["CurrentUser"] == null)
            //{
            //    //Redirect()
            //}
            //else
            //{

            //}
            return View();
        }

        [HttpGet]//响应get请求
        [CustomAllowAnonymous]
        public ViewResult Login()
        {
            return View();
        }

        [HttpPost]
        [CustomAllowAnonymous]
        public ActionResult Login(string name, string password, string verify)
        {
            string formName = base.HttpContext.Request.Form["Name"];

            var result = base.HttpContext.Login(name, password, verify);
            if (result == UserManager.LoginResult.Success)
            {
                if (base.HttpContext.Session["CurrentUrl"] != null)
                {
                    string url = base.HttpContext.Session["CurrentUrl"].ToString();
                    base.HttpContext.Session.Remove("CurrentUrl");
                    return base.Redirect(url);
                }
                else
                    return base.Redirect("/Home/Index");
            }
            else
            {
                ModelState.AddModelError("failed", result.GetRemark());
                return View();
            }
        }

        /// <summary>
        /// 验证码 FileContentResult
        /// </summary>
        /// <returns></returns>
        [CustomAllowAnonymous]
        public ActionResult VerifyCode()
        {
            string code = "";
            Bitmap bitmap = VerifyCodeHelper.CreateVerifyCode(out code);
            base.HttpContext.Session["CheckCode"] = code;//Session识别用户，为单一用户有效
            MemoryStream stream = new MemoryStream();
            bitmap.Save(stream, ImageFormat.Gif);
            return File(stream.ToArray(), "image/gif");//返回FileContentResult图片
        }

        /// <summary>
        /// 验证码  直接写入Response
        /// </summary>
        public void Verify()
        {
            string code = "";
            Bitmap bitmap = VerifyCodeHelper.CreateVerifyCode(out code);
            base.HttpContext.Session["CheckCode"] = code;
            bitmap.Save(base.Response.OutputStream, ImageFormat.Gif);
            base.Response.ContentType = "image/gif";
        }

        [HttpPost]
        [CustomAllowAnonymous]
        public ActionResult Logout()
        {
            this.HttpContext.UserLogout();
            return RedirectToAction("Index", "Home"); ;
        }
    }
}