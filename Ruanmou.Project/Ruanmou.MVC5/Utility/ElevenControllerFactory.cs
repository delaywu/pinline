using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Unity;

namespace Ruanmou.MVC5.Utility
{
    public class ElevenControllerFactory : DefaultControllerFactory
    {
        private Logger logger = new Logger(typeof(ElevenControllerFactory));

        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            this.logger.Warn($"{controllerType.Name}被构造...");

            IUnityContainer container = DIFactory.GetContainer();
            //return base.GetControllerInstance(requestContext, controllerType);
            return (IController)container.Resolve(controllerType);
        }
    }
}