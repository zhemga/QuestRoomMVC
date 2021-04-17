using System.Web.Mvc;
using System.Web.Routing;
using UI.Utils;

namespace UI
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            AutofacConfiguration.Configurate();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
    }
}
