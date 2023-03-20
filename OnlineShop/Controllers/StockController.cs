using Model;
using System.Web.Mvc;

namespace OnlineShop.Controllers
{
    public class StockController : Controller
    {
        // GET: Stock
        [HttpGet]
        [ActionName("lay-so-luong-trong-kho")]
        public ActionResult GetQuantityById(string productid, string sizeid, string colorid)
        {
            var product = WorkerClass.Ins.StockRepo.GetProductInStockById(productid, sizeid, colorid);
            var result = 0;
            if (product != null)
            {
                result = product.Quantity;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}