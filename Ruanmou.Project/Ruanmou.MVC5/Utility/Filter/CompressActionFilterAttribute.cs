using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO.Compression;
using Ruanmou.Framework.Caching;

namespace Ruanmou.MVC5.Utility.Filter
{
    /// <summary>
    /// 浏览器请求时---声明支持的格式---默认IIS是没有压缩
    /// ---检测了支持的格式---响应时将数据压缩(IIS服务器)---响应头里加上Content-Encoding
    /// ---浏览器先查看数据格式---按照压缩格式解压--(无论你是什么东西，都可以压缩解压)
    /// 
    /// 压缩是IIS  解压是浏览器
    /// 就像lucene分词，重复的单元可以节约空间
    /// </summary>
    public class CompressActionFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //foreach (var item in filterContext.ActionParameters)
            //{
            //    //参数检测  敏感词过滤
            //}  
            var request = filterContext.HttpContext.Request;
            var respose = filterContext.HttpContext.Response;
            string acceptEncoding = request.Headers["Accept-Encoding"];//检测支持格式
            if (!string.IsNullOrWhiteSpace(acceptEncoding) && acceptEncoding.ToUpper().Contains("GZIP"))
            {
                respose.AddHeader("Content-Encoding", "gzip");//响应头指定类型
                respose.Filter = new GZipStream(respose.Filter, CompressionMode.Compress);//压缩类型指定
            }
        }
    }

    public class LimitActionFilterAttribute : ActionFilterAttribute
    {
        private int _Max = 0;
        public LimitActionFilterAttribute(int max = 1000)
        {
            this._Max = max;
        }
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string key = $"{filterContext.RouteData.Values["Controller"]}_{filterContext.RouteData.Values["Action"]}";
            //CacheManager.Add(key,) 存到缓存key 集合 时间  
            filterContext.Result = new JsonResult()
            {
                Data = new { Msg = "超出频率" }
            };
        }
    }
}