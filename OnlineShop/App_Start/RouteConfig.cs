using System.Web.Mvc;
using System.Web.Routing;

namespace OnlineShop
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "DefaultCaptchaGenerate", 
                url: "DefaultCaptcha/Generate", 
                defaults: new { controller = "DefaultCaptcha", action = "Generate" }
            );
            routes.MapRoute(
                name: "DefaultCaptchaRefresh",
                url: "DefaultCaptcha/Refresh",
                defaults: new { controller = "DefaultCaptcha", action = "Refresh" }
            );
            routes.MapRoute(
                name: "Product",
                url: "san-pham/{action}/{id}",
                defaults: new { controller = "Product", id = UrlParameter.Optional },
                new[] { "OnlineShop.Controllers" }
            );
            routes.MapRoute(
                name: "Stock",
                url: "kho/{action}/{id}",
                defaults: new { controller = "Stock", id = UrlParameter.Optional },
                new[] { "OnlineShop.Controllers" }
            );
            routes.MapRoute(
                name: "ShoppingCart",
                url: "gio-hang/{action}/{id}",
                defaults: new { controller = "ShoppingCart", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "Contact",
                url: "cua-hang/{action}/{id}",
                defaults: new { controller = "Contact", id = UrlParameter.Optional },
                new[] { "OnlineShop.Controllers" }
            );
            routes.MapRoute(
                name: "Invoice",
                url: "hoa-don/{action}/{id}",
                defaults: new { controller = "Invoice", id = UrlParameter.Optional },
                new[] { "OnlineShop.Controllers" }
            );
            routes.MapRoute(
                name: "ProductOrderDetail",
                url: "chi-tiet-don-hang/{action}/{id}",
                defaults: new { controller = "ProductOrderDetail", id = UrlParameter.Optional },
                new[] { "OnlineShop.Controllers" }
            );
            routes.MapRoute(
                name: "UserAccount",
                url: "{action}/{id}",
                defaults: new { controller = "UserAccount", id = UrlParameter.Optional },
                new[] { "OnlineShop.Controllers" }
            );
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Main", action = "trang-chu", id = "" }
            );
        }
    }
}
