using System.Web.Mvc;

namespace OnlineShop.Areas.Manager
{
    public class ManagerAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Manager";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Manager_Account",
                "admin/quan-ly/tai-khoan/{action}/{id}",
                new { controller = "Account", id = UrlParameter.Optional }
            );

            context.MapRoute(
                "Manager_UserAccount",
                "admin/quan-ly/khach-hang/{action}/{id}",
                new { controller = "UserAccount", id = UrlParameter.Optional }
            );

            context.MapRoute(
                "Manager_EmployeeAccount",
                "admin/quan-ly/nhan-vien/{action}/{id}",
                new { controller = "EmployeeAccount", id = UrlParameter.Optional }
            );

            context.MapRoute(
                "Manager_Shop",
                "admin/quan-ly/cua-hang/{action}/{id}",
                new { controller = "Shop", id = UrlParameter.Optional }
            );

            context.MapRoute(
                "Manager_Contact",
                "admin/quan-ly/lien-he/{action}/{id}",
                new { controller = "Contact", id = UrlParameter.Optional }
            );

            context.MapRoute(
                "Manager_Slide",
                "admin/quan-ly/slide/{action}/{id}",
                new { controller = "Slide", id = UrlParameter.Optional }
            );

            context.MapRoute(
                "Manager_Supplier",
                "admin/quan-ly/nha-cung-cap/{action}/{id}",
                new { controller = "Supplier", id = UrlParameter.Optional }
            );
            context.MapRoute(
                "Manager_Size",
                "admin/quan-ly/kich-thuoc/{action}/{id}",
                new { controller = "Size", id = UrlParameter.Optional }
            );
            context.MapRoute(
                "Manager_Color",
                "admin/quan-ly/mau-sac/{action}/{id}",
                new { controller = "Color", id = UrlParameter.Optional }
            );
            context.MapRoute(
                "Manager_ProductCategory",
                "admin/quan-ly/loai-san-pham/{action}/{id}",
                new { controller = "ProductCategory", id = UrlParameter.Optional }
            );
            context.MapRoute(
                "Manager_Age",
                "admin/quan-ly/do-tuoi/{action}/{id}",
                new { controller = "Age", id = UrlParameter.Optional }
            );
            context.MapRoute(
                "Manager_Product",
                "admin/quan-ly/san-pham/{action}/{id}",
                new { controller = "Product", id = UrlParameter.Optional }
            );
            context.MapRoute(
                "Manager_Stock",
                "admin/quan-ly/kho/{action}/{id}",
                new { controller = "Stock", id = UrlParameter.Optional }
            );
            context.MapRoute(
                "Manager_SellProduct",
                "admin/quan-ly/ban-hang/{action}/{id}",
                new { controller = "SellProduct", id = UrlParameter.Optional }
            );
            context.MapRoute(
                "Manager_ProductOrder",
                "admin/quan-ly/phieu-dat-hang/{action}/{id}",
                new { controller = "ProductOrder", id = UrlParameter.Optional }
            );
            context.MapRoute(
                "Manager_ProductOrderDetail",
                "admin/quan-ly/chi-tiet-phieu-dat-hang/{action}/{id}",
                new { controller = "ProductOrderDetail", id = UrlParameter.Optional }
            );
            context.MapRoute(
                "Manager_Invoice",
                "admin/quan-ly/hoa-don/{action}/{id}",
                new { controller = "Invoice", id = UrlParameter.Optional }
            );
            context.MapRoute(
                "Manager_Home",
                "admin/quan-ly/{action}/{id}",
                new { controller = "Home", id = UrlParameter.Optional }
            );
            context.MapRoute(
                "Manager_PersonalInformation",
                "admin/tai-khoan/{action}/{id}",
                new { controller = "PersonalInformation", id = UrlParameter.Optional }
            );
            context.MapRoute(
                "Manager_Admin",
                "admin/{action}/{id}",
                new { controller = "Admin", id = UrlParameter.Optional }
            );
            context.MapRoute(
                "Manager_default",
                "Manager/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}