using Model;
using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace OnlineShop
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
        protected void Session_Start()
        {
            // 'using' will call entity.Dispose() at the end of the block so you
            // don't have to bother about disposing your entity
            HttpContext context = HttpContext.Current;
            if (context != null && context.Session != null)
            {
                // fill the Session var with the tables from your database
                context.Session["ListProduct"] = WorkerClass.Ins.ProductRepo.GetAllProductSelling().Select(x => new { x.ProductName, Photo = (x.ProductPhotoes.FirstOrDefault() == null ? null : Convert.ToBase64String(x.ProductPhotoes.FirstOrDefault().Photo)) }).ToList();
                context.Session["Contact"] = WorkerClass.Ins.ContactRepo.GetContact();
            }
        }
        protected void Application_BeginRequest()
        {
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetExpires(DateTime.UtcNow.AddHours(-1));
            Response.Cache.SetNoStore();
        }
    }
}
