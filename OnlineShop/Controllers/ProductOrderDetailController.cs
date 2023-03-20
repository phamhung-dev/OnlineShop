using Model;
using System;
using System.Linq;
using System.Web.Mvc;

namespace OnlineShop.Controllers
{
    public class ProductOrderDetailController : Controller
    {
        // GET: ProductOrderDetail
        [HttpGet]
        [ActionName("xem-chi-tiet-phieu-dat-hang")]
        public ActionResult GetProductOrderDetailById(string id)
        {
            try
            {
                Guid productOrderId = new Guid(id);
                var list = WorkerClass.Ins.ProductOrderDetailRepo.GetProductOrderDetailById(productOrderId).Select(x => new { x.Stock.Product.ProductName, x.Stock.Size.SizeName, x.Stock.Color.ColorName, x.Quantity, x.Price, Payment = x.Quantity * x.Price });
                var productOrder = WorkerClass.Ins.ProductOrderRepo.GetProductOrderById(productOrderId);
                var payment = new
                {
                    productOrder.Payment.PaymentName,
                    productOrder.PaymentOnlineID
                };
                return Json(new { success = true, listProductOrderDetail = list, payment = payment }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { success = false, message = "Lỗi không xem được chi tiết phiếu đặt hàng." }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}