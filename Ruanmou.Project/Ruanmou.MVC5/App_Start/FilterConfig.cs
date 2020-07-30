using Ruanmou.MVC5.Utility.Filter;
using Ruanmou.MVC5.Utility.FilterTest;
using System.Web;
using System.Web.Mvc;

namespace Ruanmou.MVC5
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            //filters.Add(new HandleErrorAttribute());
            filters.Add(new CustomHandleErrorAttribute());//全部的控制器全部的action都生效
            //filters.Add(new CustomAuthorizeAttribute("~/Fifth/Login"));
            //filters.Add(new CustomActionGlobalActionFilterAttribute());
            filters.Add(new CompressActionFilterAttribute());
        }
    }
}
