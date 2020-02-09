using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using EasyBlog.Web.Core;
using System;
using System.Linq;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace EasyBlog.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            IExtensibilityManager extensibilityManager = new ExtensibilityManager();

            if (Application["ModuleEvents"] == null)
                Application["ModuleEvents"] = extensibilityManager.GetModuleEvents();

            ContainerBuilder builder = new ContainerBuilder();
            //No need to register each controller one by one
            //builder.RegisterType<HomeController>();

            //All controllers can be registered at once.
            builder.RegisterControllers(typeof(MvcApplication).Assembly); //MVC
            builder.RegisterApiControllers(typeof(MvcApplication).Assembly);//WebAPI
            //Set the container
            IContainer container = builder.Build();

            //Dependency Resolver
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container)); //MVC
            GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver(container);//WebAPI
            


        }
    }
}
