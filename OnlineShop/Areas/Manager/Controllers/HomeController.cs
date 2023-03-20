using Model;
using Model.EF;
using OnlineShop.Areas.Manager.Service;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;

namespace OnlineShop.Areas.Manager.Controllers
{
    public class HomeController : Controller
    {
        // GET: Manager/Home
        [ActionName("trang-chu")]
        [HasAdminCredential(Role = 1)]
        public ActionResult Index()
        {
            if (Session["UserCurrent"] == null)
            {
                return RedirectToAction("dang-nhap", "Admin");
            }
            var revenuePreviousMonth = (double)WorkerClass.Ins.InvoiceRepo.GetAllInvoiceSucceededByDateTime(DateTime.Now.Month-1, DateTime.Now.Year).Sum(x => x.TotalPayment);
            var revenueCurrentMonth = (double)WorkerClass.Ins.InvoiceRepo.GetAllInvoiceSucceededByDateTime(DateTime.Now.Month, DateTime.Now.Year).Sum(x => x.TotalPayment);
            ViewBag.NewOrder = WorkerClass.Ins.ProductOrderRepo.CountNewOrder();
            ViewBag.RevenueRate = revenuePreviousMonth == 0 ? 100 : Math.Round((revenueCurrentMonth - revenuePreviousMonth) / revenuePreviousMonth * 100, 2);
            ViewBag.UserAccount = WorkerClass.Ins.UserAccountRepo.CountUserAccount();
            ViewBag.Product = WorkerClass.Ins.ProductRepo.CountProduct();
            var listMonthlyRevenue = WorkerClass.Ins.InvoiceRepo.GetAllMonthlyRevenue(DateTime.Now.Year).GroupBy(x => x.ExportDate.Month).OrderBy(x => x.First().ExportDate.Month).Select(x => new { Month = "Tháng " + x.First().ExportDate.Month.ToString(), Revenue = x.Sum(o => o.TotalPayment) }).ToList();
            List<string> listMonth = new List<string>();
            List<decimal> listRevenue = new List<decimal>();
            foreach(var item in listMonthlyRevenue)
            {
                listMonth.Add(item.Month);
                listRevenue.Add(item.Revenue);
            }
            ViewBag.ListLabels = listMonth;
            ViewBag.ListData = listRevenue;
            var time = WorkerClass.Ins.InvoiceRepo.GetFirstInvoiceInYear(DateTime.Now.Year);
            if(time != null)
            {
                ViewBag.FirstTime = time.ExportDate.ToString("dd-MM-yyyy");
            }
            ViewBag.InvoiceCanceled = WorkerClass.Ins.InvoiceRepo.CountInvoiceCanceled();
            ViewBag.InvoiceDelivering = WorkerClass.Ins.InvoiceRepo.CountInvoiceDelivering();
            ViewBag.InvoiceSucceeded = WorkerClass.Ins.InvoiceRepo.CountInvoiceSucceeded();
            ViewBag.TotalRevenueYear = listRevenue.Sum();
            ViewBag.TotalProductSold = WorkerClass.Ins.ProductOrderDetailRepo.TotalProductSold();
            ViewBag.TotalProduct = WorkerClass.Ins.StockRepo.TotalProductInStock() + ViewBag.TotalProductSold;
            ViewBag.TotalProductInventory = WorkerClass.Ins.StockRepo.TotalProductInventoryInStock();
            ViewBag.SoldRate = ViewBag.TotalProductSold * 1.0 / ViewBag.TotalProduct * 100;
            ViewBag.InventoryRate = ViewBag.TotalProductInventory * 1.0 / ViewBag.TotalProduct * 100;
            var listProduct = WorkerClass.Ins.ProductRepo.GetAllProductSelling();
            var listProductOrderDetail = WorkerClass.Ins.ProductOrderDetailRepo.GetAllProductOrderDetailSold().GroupBy(x => x.ProductID).Select(x => new { x.First().ProductID, Quantity = x.Sum(o => o.Quantity)}).ToList();
            ViewBag.ListProductBestSeller = (from p in listProduct
                     join po in listProductOrderDetail on p.ProductID equals po.ProductID
                     select new ProductBestSeller { ProductID = p.ProductID, ProductName = p.ProductName, SupplierName = p.Supplier.SupplierName, Gender = p.Gender == true ? "Nam" : "Nữ", UnitSellPrice = p.UnitSellPrice, Quantity = po.Quantity, Photo = p.ProductPhotoes.FirstOrDefault() == null ? null : Convert.ToBase64String(p.ProductPhotoes.FirstOrDefault().Photo) }).Distinct().OrderByDescending(x => x.Quantity).Take(4).ToList();
            return View("Index");
        }
        [HttpPost]
        [ActionName("lay-doanh-thu-ngay")]
        [HasAdminCredential(Role = 1)]
        public ActionResult GetListDateRevenue(string date)
        {
            try
            {
                var listProduct = WorkerClass.Ins.ProductRepo.GetAllProduct();
                var temp = DateTime.ParseExact(date, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                var listProductOrderDetailByDate = WorkerClass.Ins.ProductOrderDetailRepo.GetAllProductOrderDetailByDate(temp).GroupBy(x => x.ProductID).Select(x => new { x.First().ProductID, Quantity = x.Sum(o => o.Quantity) }).ToList();
                var result = (from p in listProduct
                              join po in listProductOrderDetailByDate on p.ProductID equals po.ProductID
                              select new ProductBestSeller { ProductID = p.ProductID, ProductName = p.ProductName, UnitSellPrice = p.UnitSellPrice, Quantity = po.Quantity, Revenue = p.UnitSellPrice * po.Quantity }).Distinct().ToList();
                return Json(result);
            }
            catch
            {
                return Json(null);
            }

        }
    }
}