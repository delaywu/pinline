using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ruanmou.MVC5.Utility.Pipeline
{
    public class CustomHttpModule : IHttpModule
    {
        public void Dispose()
        {
            Console.WriteLine();
        }

        public event EventHandler CustomHttpModuleHandler;

        /// <summary>
        /// 注册动作
        /// </summary>
        /// <param name="context"></param>
        public void Init(HttpApplication application)
        {
            application.BeginRequest += (s, e) =>
              {
                  this.CustomHttpModuleHandler?.Invoke(application, null);
              };
            //application.EndRequest += (s, e) =>
            //{
            //    HttpContext.Current.Response.Write("CustomHttpModule.EndRequest");
            //};
            #region 为每一个事件，都注册了一个动作，向客户端输出信息
            application.AcquireRequestState += (s, e) => application.Response.Write(string.Format("<h1 style='color:#00f'>来自MyCustomModule 的处理，{0}请求到达 {1}</h1><hr>", DateTime.Now.ToString(), "AcquireRequestState        "));
            application.AuthenticateRequest += (s, e) => application.Response.Write(string.Format("<h1 style='color:#00f'>来自MyCustomModule 的处理，{0}请求到达 {1}</h1><hr>", DateTime.Now.ToString(), "AuthenticateRequest        "));
            application.AuthorizeRequest += (s, e) => application.Response.Write(string.Format("<h1 style='color:#00f'>来自MyCustomModule 的处理，{0}请求到达 {1}</h1><hr>", DateTime.Now.ToString(), "AuthorizeRequest           "));
            application.BeginRequest += (s, e) => application.Response.Write(string.Format("<h1 style='color:#00f'>来自MyCustomModule 的处理，{0}请求到达 {1}</h1><hr>", DateTime.Now.ToString(), "BeginRequest               "));
            application.Disposed += (s, e) => application.Response.Write(string.Format("<h1 style='color:#00f'>来自MyCustomModule 的处理，{0}请求到达 {1}</h1><hr>", DateTime.Now.ToString(), "Disposed                   "));
            application.EndRequest += (s, e) => application.Response.Write(string.Format("<h1 style='color:#00f'>来自MyCustomModule 的处理，{0}请求到达 {1}</h1><hr>", DateTime.Now.ToString(), "EndRequest                 "));
            application.Error += (s, e) => application.Response.Write(string.Format("<h1 style='color:#00f'>来自MyCustomModule 的处理，{0}请求到达 {1}</h1><hr>", DateTime.Now.ToString(), "Error                      "));
            application.LogRequest += (s, e) => application.Response.Write(string.Format("<h1 style='color:#00f'>来自MyCustomModule 的处理，{0}请求到达 {1}</h1><hr>", DateTime.Now.ToString(), "LogRequest                 "));
            application.MapRequestHandler += (s, e) => application.Response.Write(string.Format("<h1 style='color:#00f'>来自MyCustomModule 的处理，{0}请求到达 {1}</h1><hr>", DateTime.Now.ToString(), "MapRequestHandler          "));
            application.PostAcquireRequestState += (s, e) => application.Response.Write(string.Format("<h1 style='color:#00f'>来自MyCustomModule 的处理，{0}请求到达 {1}</h1><hr>", DateTime.Now.ToString(), "PostAcquireRequestState    "));
            application.PostAuthenticateRequest += (s, e) => application.Response.Write(string.Format("<h1 style='color:#00f'>来自MyCustomModule 的处理，{0}请求到达 {1}</h1><hr>", DateTime.Now.ToString(), "PostAuthenticateRequest    "));
            application.PostAuthorizeRequest += (s, e) => application.Response.Write(string.Format("<h1 style='color:#00f'>来自MyCustomModule 的处理，{0}请求到达 {1}</h1><hr>", DateTime.Now.ToString(), "PostAuthorizeRequest       "));
            application.PostLogRequest += (s, e) => application.Response.Write(string.Format("<h1 style='color:#00f'>来自MyCustomModule 的处理，{0}请求到达 {1}</h1><hr>", DateTime.Now.ToString(), "PostLogRequest             "));
            application.PostMapRequestHandler += (s, e) => application.Response.Write(string.Format("<h1 style='color:#00f'>来自MyCustomModule 的处理，{0}请求到达 {1}</h1><hr>", DateTime.Now.ToString(), "PostMapRequestHandler      "));
            application.PostReleaseRequestState += (s, e) => application.Response.Write(string.Format("<h1 style='color:#00f'>来自MyCustomModule 的处理，{0}请求到达 {1}</h1><hr>", DateTime.Now.ToString(), "PostReleaseRequestState    "));
            application.PostRequestHandlerExecute += (s, e) => application.Response.Write(string.Format("<h1 style='color:#00f'>来自MyCustomModule 的处理，{0}请求到达 {1}</h1><hr>", DateTime.Now.ToString(), "PostRequestHandlerExecute  "));
            application.PostResolveRequestCache += (s, e) => application.Response.Write(string.Format("<h1 style='color:#00f'>来自MyCustomModule 的处理，{0}请求到达 {1}</h1><hr>", DateTime.Now.ToString(), "PostResolveRequestCache    "));
            application.PostUpdateRequestCache += (s, e) => application.Response.Write(string.Format("<h1 style='color:#00f'>来自MyCustomModule 的处理，{0}请求到达 {1}</h1><hr>", DateTime.Now.ToString(), "PostUpdateRequestCache     "));
            application.PreRequestHandlerExecute += (s, e) => application.Response.Write(string.Format("<h1 style='color:#00f'>来自MyCustomModule 的处理，{0}请求到达 {1}</h1><hr>", DateTime.Now.ToString(), "PreRequestHandlerExecute   "));
            application.PreSendRequestContent += (s, e) => application.Response.Write(string.Format("<h1 style='color:#00f'>来自MyCustomModule 的处理，{0}请求到达 {1}</h1><hr>", DateTime.Now.ToString(), "PreSendRequestContent      "));
            application.PreSendRequestHeaders += (s, e) => application.Response.Write(string.Format("<h1 style='color:#00f'>来自MyCustomModule 的处理，{0}请求到达 {1}</h1><hr>", DateTime.Now.ToString(), "PreSendRequestHeaders      "));
            application.ReleaseRequestState += (s, e) => application.Response.Write(string.Format("<h1 style='color:#00f'>来自MyCustomModule 的处理，{0}请求到达 {1}</h1><hr>", DateTime.Now.ToString(), "ReleaseRequestState        "));
            application.RequestCompleted += (s, e) => application.Response.Write(string.Format("<h1 style='color:#00f'>来自MyCustomModule 的处理，{0}请求到达 {1}</h1><hr>", DateTime.Now.ToString(), "RequestCompleted           "));
            application.ResolveRequestCache += (s, e) => application.Response.Write(string.Format("<h1 style='color:#00f'>来自MyCustomModule 的处理，{0}请求到达 {1}</h1><hr>", DateTime.Now.ToString(), "ResolveRequestCache        "));
            application.UpdateRequestCache += (s, e) => application.Response.Write(string.Format("<h1 style='color:#00f'>来自MyCustomModule 的处理，{0}请求到达 {1}</h1><hr>", DateTime.Now.ToString(), "UpdateRequestCache         "));
            #endregion
        }
    }
}