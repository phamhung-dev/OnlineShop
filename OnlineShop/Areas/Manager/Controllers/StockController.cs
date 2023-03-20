using Model;
using Model.EF;
using OnlineShop.Areas.Manager.Service;
using System.Web.Mvc;

namespace OnlineShop.Areas.Manager.Controllers
{
    public class StockController : Controller
    {
        // GET: Manager/Stock
        [HttpPost]
        [ActionName("thay-doi-trang-thai-san-pham")]
        [HasAdminCredential(Role = 2)]
        public ActionResult ChangeStatusSize(string productid, string sizeid, string colorid)
        {
            try
            {
                bool status = WorkerClass.Ins.StockRepo.ChangeStatusStock(productid, sizeid, colorid, (Session["UserCurrent"] as EmployeeAccount).Email);
                WorkerClass.Ins.Save();
                return Json(new { success = true, status }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        [ActionName("nhap-kho")]
        [HasAdminCredential(Role = 2)]
        public ActionResult Import(string productid, string sizeid, string colorid, string quantity)
        {
            try
            {
                int quantityInt = int.Parse(quantity);
                if(quantityInt < 0)
                {
                    return Json(new { success = false, message = "Số lượng nhập phải >= 0." }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    if(WorkerClass.Ins.StockRepo.Import(productid, sizeid, colorid, quantityInt, (Session["UserCurrent"] as EmployeeAccount).Email))
                    {
                        WorkerClass.Ins.Save();
                        return Json(new { success = true, message = "Cập nhật kho thành công." }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { success = false, message = "Cập nhật kho thất bại." }, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch
            {
                return Json(new { success = false, message = "Cập nhật kho thất bại." }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        [ActionName("xuat-kho")]
        [HasAdminCredential(Role = 2)]
        public ActionResult Export(string productid, string sizeid, string colorid, string quantity)
        {
            try
            {
                int quantityInt = int.Parse(quantity);
                if (quantityInt < 0)
                {
                    return Json(new { success = false, message = "Số lượng nhập phải >= 0." }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    int quantityInStock = WorkerClass.Ins.StockRepo.CheckQuantityInStock(productid, sizeid, colorid);
                    if(quantityInt > quantityInStock)
                    {
                        return Json(new { success = false, message = "Trong kho chỉ còn " + quantityInStock.ToString() + " sản phẩm." }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        if (WorkerClass.Ins.StockRepo.Export(productid, sizeid, colorid, quantityInt, (Session["UserCurrent"] as EmployeeAccount).Email))
                        {
                            WorkerClass.Ins.Save();
                            return Json(new { success = true, message = "Cập nhật kho thành công." }, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            return Json(new { success = false, message = "Cập nhật kho thất bại." }, JsonRequestBehavior.AllowGet);
                        }
                    }
                }
            }
            catch
            {
                return Json(new { success = false, message = "Cập nhật kho thất bại." }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        [ActionName("them-san-pham-vao-kho")]
        [HasAdminCredential(Role = 2)]
        public ActionResult InsertSize(string productid, string sizeid, string colorid)
        {
            try
            {
                if (string.IsNullOrEmpty(productid) || string.IsNullOrEmpty(sizeid) || string.IsNullOrEmpty(colorid))
                {
                    return Json(new { success = false, message = "Vui lòng chọn đầy đủ thông tin!" }, JsonRequestBehavior.AllowGet);
                }

                if (WorkerClass.Ins.StockRepo.GetProductInStockById(productid, sizeid, colorid) != null)
                {
                    return Json(new { success = false, message = "Sản phẩm này đã tồn tại!" }, JsonRequestBehavior.AllowGet);
                }
                productid = string.IsNullOrEmpty(productid.Trim()) ? null : productid.Trim();
                sizeid = string.IsNullOrEmpty(sizeid.Trim()) ? null : sizeid.Trim();
                colorid = string.IsNullOrEmpty(colorid.Trim()) ? null : colorid.Trim();
                if (WorkerClass.Ins.StockRepo.InsertStock(productid, sizeid, colorid, (Session["UserCurrent"] as EmployeeAccount).Email))
                {
                    WorkerClass.Ins.Save();
                    return Json(new { success = true, message = "Thêm sản phẩm vào kho thành công." }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { success = false, message = "Thêm sản phẩm vào kho thất bại." }, JsonRequestBehavior.AllowGet);
                }
            }
            catch
            {
                return Json(new { success = false, message = "Thêm sản phẩm vào kho thất bại." }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}