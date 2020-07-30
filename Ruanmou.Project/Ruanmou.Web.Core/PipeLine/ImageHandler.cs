using System;
using System.Web;

namespace Ruanmou.Web.Core.PipeLine
{
    /// <summary>
    /// 盗链：A网站去通过B网站资源展示图片
    /// 防盗链:不允许盗链，请求页面时会检测一下urlrerfer（浏览器行为）
    ///        在白名单里面就正常返回，否则就不正常返回(返回一个授权图片)
    /// </summary>
    public class ImageHandler : IHttpHandler
    {
        #region IHttpHandler Members

        public bool IsReusable
        {
            get { return true; }
        }

        public void ProcessRequest(HttpContext context)
        {
            // 如果UrlReferrer为空，则显示一张默认的禁止盗链的图片
            if (context.Request.UrlReferrer == null || context.Request.UrlReferrer.Host == null)
            {    //大部分都是爬虫
                context.Response.ContentType = "image/JPEG";
                context.Response.WriteFile("/Content/Image/Forbidden.jpg");
            }
            else
            {
                // 如果 UrlReferrer中不包含自己站点主机域名，则显示一张默认的禁止盗链的图片
                if (context.Request.UrlReferrer.Host.Contains("localhost"))
                {
                    // 获取文件服务器端物理路径
                    string FileName = context.Server.MapPath(context.Request.FilePath);
                    context.Response.ContentType = "image/JPEG";
                    context.Response.WriteFile(FileName);
                }
                else
                {
                    context.Response.ContentType = "image/JPEG";
                    context.Response.WriteFile("/Content/Image/Forbidden.jpg");
                }
            }
        }

        #endregion
    }
}
