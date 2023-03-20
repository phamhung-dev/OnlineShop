using Model;
using OnlineShop.Areas.Manager.Service;
using System.Web.Mvc;

namespace OnlineShop.Areas.Manager.Controllers
{
    public class ShopController : Controller
    {
        // GET: Manager/Shop
        [ActionName("xem-thong-tin")]
        [HasAdminCredential(Role = 2)]
        public ActionResult GetInfoShop()
        {
            if (Session["UserCurrent"] == null)
            {
                return RedirectToAction("dang-nhap", "Admin");
            }
            ViewBag.ListSlide = WorkerClass.Ins.SlideRepo.GetAllSlide();
            ViewBag.Contact = WorkerClass.Ins.ContactRepo.GetContact();
            return View("GetInfoShop");
        }
        [ActionName("danh-sach-nha-cung-cap")]
        [HasAdminCredential(Role = 2)]
        public ActionResult GetAllSupplier()
        {
            if (Session["UserCurrent"] == null)
            {
                return RedirectToAction("dang-nhap", "Admin");
            }
            return View("GetAllSupplier", WorkerClass.Ins.SupplierRepo.GetAllSupplier());
        }
        [ActionName("danh-sach-san-pham")]
        [HasAdminCredential(Role = 2)]
        public ActionResult GetAllProduct()
        {
            if (Session["UserCurrent"] == null)
            {
                return RedirectToAction("dang-nhap", "Admin");
            }
            ViewBag.ListProductCategory = WorkerClass.Ins.ProductCategoryRepo.GetAllProductCategory();
            ViewBag.ListAge = WorkerClass.Ins.AgeRepo.GetAllAge();
            ViewBag.ListSize = WorkerClass.Ins.SizeRepo.GetAllSize();
            ViewBag.ListColor = WorkerClass.Ins.ColorRepo.GetAllColor();
            ViewBag.ListSupplierContacting = WorkerClass.Ins.SupplierRepo.GetAllSupplierContacting();
            ViewBag.ListProductCategorySelling = WorkerClass.Ins.ProductCategoryRepo.GetAllProductCategorySelling();
            ViewBag.ListAgeSelling = WorkerClass.Ins.AgeRepo.GetAllAgeSelling();
            return View("GetAllProduct", WorkerClass.Ins.ProductRepo.GetAllProduct());
        }
        [ActionName("danh-sach-san-pham-trong-kho")]
        [HasAdminCredential(Role = 2)]
        public ActionResult GetAllProductInStock()
        {
            if (Session["UserCurrent"] == null)
            {
                return RedirectToAction("dang-nhap", "Admin");
            }
            ViewBag.ListSizeSelling = WorkerClass.Ins.SizeRepo.GetAllSizeSelling();
            ViewBag.ListColorSelling = WorkerClass.Ins.ColorRepo.GetAllColorSelling();
            ViewBag.ListProductSelling = WorkerClass.Ins.ProductRepo.GetAllProductSelling();
            return View("GetAllProductInStock", WorkerClass.Ins.StockRepo.GetAllProductInStock());
        }
    }
}