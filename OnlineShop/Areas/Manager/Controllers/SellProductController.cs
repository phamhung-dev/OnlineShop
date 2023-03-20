using Model;
using OnlineShop.Areas.Manager.Service;
using System.Web.Mvc;

namespace OnlineShop.Areas.Manager.Controllers
{
    public class SellProductController : Controller
    {
        // GET: Manager/SellProduct
        [ActionName("danh-sach-phieu-dat-hang")]
        [HasAdminCredential(Role = 2)]
        public ActionResult GetAllProductOrder() 
        {
            if (Session["UserCurrent"] == null)
            {
                return RedirectToAction("dang-nhap", "Admin");
            }
            return View("GetAllProductOrder", WorkerClass.Ins.ProductOrderRepo.GetAllProductOrder());
        }
        [ActionName("danh-sach-hoa-don")]
        [HasAdminCredential(Role = 2)]
        public ActionResult GetAllInvoice()
        {
            if (Session["UserCurrent"] == null)
            {
                return RedirectToAction("dang-nhap", "Admin");
            }
            return View("GetAllInvoice", WorkerClass.Ins.InvoiceRepo.GetAllInvoice());
        }
    }
}