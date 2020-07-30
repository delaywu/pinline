using System.Web.Mvc;

namespace Ruanmou.MVC5.Areas.System
{
    public class SystemAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "System";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
               name: "System_default",
               url: "System/{controller}/{action}/{id}",
              defaults: new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}