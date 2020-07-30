using System.Web.Mvc;

namespace Ruanmou.MVC5.Areas.System
{
    public class SystemAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "SystemPlugins";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
               name: "SystemPlugins_default",
               url: "SystemPlugins/{controller}/{action}/{id}",
              defaults: new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}