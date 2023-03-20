using Model;
using System.Web.Mvc;

namespace OnlineShop.Controllers
{
    public class MainController : Controller
    {
        // GET: Main
        [ActionName("trang-chu")]
        public ActionResult Index()
        {
            ViewBag.ListFeaturedProduct = WorkerClass.Ins.ProductRepo.GetListFeaturedProduct(8);
            ViewBag.ListNewProduct = WorkerClass.Ins.ProductRepo.GetListNewProduct(8);
            ViewBag.ListDiscountProduct = WorkerClass.Ins.ProductRepo.GetListDiscountProduct(8);
            ViewBag.ListSlide = WorkerClass.Ins.SlideRepo.GetAllSlideUsing();
            return View("Index");
        }
    }
}