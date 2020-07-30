using Ruanmou.Bussiness.Interface;
using Ruanmou.EF.Model;
using Ruanmou.Framework.Encrypt;
using Ruanmou.Framework.Extension;
using Ruanmou.Framework.Log;
using Ruanmou.Web.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Unity;

namespace Ruanmou.MVC5.Utility
{
    /// <summary>
    /// UserManager在UI层，准备放入基础设施层Ruanmou.Framework
    /// Ruanmou.Framework 依赖Ruanmou.Bussiness.Interface
    /// 但是Ruanmou.Bussiness.Interface 是依赖Ruanmou.Framework
    /// 循环依赖...  
    /// 可以通过委托传递，UI引用了Interface,调用Framework里面的UserManager,
    /// 可以传递委托进去，该委托完成对数据库的查询，
    /// UserManager只需要执行委托，不需要依赖了
    /// </summary>
    public static class UserManager
    {
        private static Logger logger = new Logger(typeof(UserManager)); //Logger.CreateLogger(typeof(UserManager));
        /// <summary>
        /// 用户登录验证
        /// </summary>
        /// <param name="name"></param>
        /// <param name="password"></param>
        /// <param name="verifyCode"></param>
        /// <returns></returns>
        public static LoginResult Login(this HttpContextBase context, string name, string password, string verifyCode)
        {
            if (context.Session["CheckCode"] != null
                && !string.IsNullOrWhiteSpace(context.Session["CheckCode"].ToString())
                && context.Session["CheckCode"].ToString().Equals(verifyCode, StringComparison.CurrentCultureIgnoreCase))
            {
                using (IUserCompanyService servcie = DIFactory.GetContainer().Resolve<IUserCompanyService>())
                {
                    User user = servcie.Set<User>().FirstOrDefault(u => u.Name.Equals(name) || u.Account.Equals(name) || u.Mobile.Equals(name) || u.Email.Equals(name));//账号查找
                    if (user == null)
                    {
                        return LoginResult.NoUser;
                    }
                    else if (!user.Password.Equals(MD5Encrypt.Encrypt(password)))
                    {
                        return LoginResult.WrongPwd;
                    }
                    else if (user.State == 1)
                    {
                        return LoginResult.Frozen;
                    }
                    else
                    {
                        //登录成功  写cookie session
                        CurrentUser currentUser = new CurrentUser()
                        {
                            Id = user.Id,
                            Name = user.Name,
                            Account = user.Account,
                            Email = user.Email,
                            Password = user.Password,
                            LoginTime = DateTime.Now
                        };
                        //都是asp.net解析的
                        #region Request
                        //context.Request
                        //Request 获取请求个各种参数，
                        //Header里面的各种信息
                        //InputStream上传的文件
                        #endregion

                        #region Response
                        //context.Response
                        //Response响应的 跨域、压缩、缓存、cookie、output + contentType
                        #endregion

                        #region Server
                        //辅助类 Server
                        string encode = context.Server.HtmlEncode("<我爱我家>");
                        string decode = context.Server.HtmlDecode(encode);
                        string physicalPath = context.Server.MapPath("/home/index");//只能做物理文件的映射
                        string encodeUrl = context.Server.UrlEncode("<我爱我家>");
                        string decodeUrl = context.Server.UrlDecode(encodeUrl);
                        #endregion

                        #region Application
                        context.Application.Lock();//ASP.NET 应用程序内的多个会话和请求之间共享信息
                        context.Application.Lock();
                        context.Application.Add("try", "die");
                        context.Application.UnLock();
                        object aValue = context.Application.Get("try");
                        aValue = context.Application["try"];
                        context.Application.Remove("命名对象");
                        context.Application.RemoveAt(0);
                        context.Application.RemoveAll();
                        context.Application.Clear();

                        context.Items["123"] = "123";//单一会话，不同环境都可以用
                        #endregion

                        #region Cookie
                        //context.Request.Cookies

                        //HttpCookie cookie = context.Request.Cookies.Get("CurrentUser");
                        //if (cookie == null)
                        //{
                        HttpCookie myCookie = new HttpCookie("CurrentUser");
                        myCookie.Value = JsonHelper.ObjectToString<CurrentUser>(currentUser);
                        myCookie.Expires = DateTime.Now.AddMinutes(5);
                        //5分钟后  硬盘cookie
                        //不设置就是内存cookie--关闭浏览器就丢失
                        //改成过期 -1 过期
                        //修改cookie：不能修改，只能起个同名的cookie

                        //myCookie.Domain//设置cookie共享域名
                        //myCookie.Path//指定路径能享有cookie
                        context.Response.Cookies.Add(myCookie);//一定要输出
                        //}
                        //前端只能获取name-value
                        #endregion Cookie

                        #region Session
                        //context.Session.RemoveAll();
                        var sessionUser = context.Session["CurrentUser"];
                        context.Session["CurrentUser"] = currentUser;
                        context.Session.Timeout = 3;//minute  session过期等于Abandon
                        #endregion Session

                        logger.Debug(string.Format("用户id={0} Name={1}登录系统", currentUser.Id, currentUser.Name));
                        return LoginResult.Success;
                    }
                }
                //服务端是只靠session--安全
                //cookie一直做登陆
                //cookie+session：验证用session，没有session就看cookie(cookie写个时间)
            }
            else
            {
                return LoginResult.WrongVerify;
            }


        }
        public enum LoginResult
        {
            /// <summary>
            /// 登录成功
            /// </summary>
            [RemarkAttribute("登录成功")]
            Success = 0,
            /// <summary>
            /// 用户不存在
            /// </summary>
            [RemarkAttribute("用户不存在")]
            NoUser = 1,
            /// <summary>
            /// 密码错误
            /// </summary>
            [RemarkAttribute("密码错误")]
            WrongPwd = 2,
            /// <summary>
            /// 验证码错误
            /// </summary>
            [RemarkAttribute("验证码错误")]
            WrongVerify = 3,
            /// <summary>
            /// 账号被冻结
            /// </summary>
            [RemarkAttribute("账号被冻结")]
            Frozen = 4
        }

        public static void UserLogout(this HttpContextBase context)
        {
            #region Cookie
            HttpCookie myCookie = context.Request.Cookies["CurrentUser"];
            if (myCookie != null)
            {
                myCookie.Expires = DateTime.Now.AddMinutes(-1);//设置过过期
                context.Response.Cookies.Add(myCookie);
            }

            #endregion Cookie

            #region Session
            var sessionUser = context.Session["CurrentUser"];
            if (sessionUser != null && sessionUser is CurrentUser)
            {
                CurrentUser currentUser = (CurrentUser)context.Session["CurrentUser"];
                logger.Debug(string.Format("用户id={0} Name={1}退出系统", currentUser.Id, currentUser.Name));
            }
            context.Session["CurrentUser"] = null;//表示将制定的键的值清空，并释放掉，
            context.Session.Remove("CurrentUser");
            context.Session.Clear();//表示将会话中所有的session的键值都清空，但是session还是依然存在，
            context.Session.RemoveAll();//
            context.Session.Abandon();//就是把当前Session对象删除了，下一次就是新的Session了   
            #endregion Session
        }
    }
}