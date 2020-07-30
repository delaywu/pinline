using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ruanmou.MVC5.Utility.Pipeline
{
    /// <summary>
    /// 直播平台--网页播放--jwplayer--需要一个配置文件.rtmp
    /// 在临时文件夹生成一个文件.rtmp  然后配置一下文件mine，当成物理文件访问---临时生成---还得删除
    /// 
    /// 客户端要的是内容---先保存硬盘---返回文件流
    /// 如果能直接动态响应  .rtmp
    /// 我们可以从请求级出发，避开默认机制
    /// </summary>
    public class CustomRTMPHandler : IHttpHandler
    {
        public bool IsReusable => true;

        public void ProcessRequest(HttpContext context)
        {
            //Console.WriteLine("This is AAAA");
            context.Response.Write("This is AAAA");
            context.Response.ContentType = "text/html";
        }
    }
}