using Model;
using Model.EF;
using OnlineShop.Areas.Manager.Service;
using System;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace OnlineShop.Areas.Manager.Controllers
{
    public class SupplierController : Controller
    {
        // GET: Manager/Supplier
        [HttpGet]
        [ActionName("xem-nha-cung-cap")]
        [HasAdminCredential(Role = 2)]
        public ActionResult GetSupplierById(string id)
        {
            var supplierMain = WorkerClass.Ins.SupplierRepo.GetSupplierById(id);
            var supplier = new
            {
                supplierMain.SupplierID,
                supplierMain.SupplierName,
                supplierMain.ContractDate,
                supplierMain.Phone,
                supplierMain.Email,
                supplierMain.Status
            };
            return Json(supplier, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ActionName("cap-nhat-nha-cung-cap")]
        [HasAdminCredential(Role = 2)]
        public ActionResult UpdateSupplier(string id, string suppliername, string contractdate, string phone, string email, string status)
        {
            try
            {
                if (string.IsNullOrEmpty(suppliername) || string.IsNullOrEmpty(contractdate))
                {
                    return Json(new { success = false, message = "Vui lòng nhập đầy đủ thông tin!" }, JsonRequestBehavior.AllowGet);
                }
                if (!Regex.IsMatch(contractdate, @"^([0-2][0-9]|(3)[0-1])(\/)(((0)[0-9])|((1)[0-2]))(\/)\d{4}$"))
                {
                    return Json(new { success = false, message = "Ngày hợp tác không đúng định dạng!" }, JsonRequestBehavior.AllowGet);
                }
                suppliername = string.IsNullOrEmpty(suppliername.Trim()) ? null : suppliername.Trim();
                DateTime contractdateDateTime = DateTime.ParseExact(contractdate, "dd/MM/yyyy", null);
                phone = Regex.Replace(phone, @"[^\d]", "");
                email = string.IsNullOrEmpty(email.Trim()) ? null : email.Trim();
                bool statusBool = bool.Parse(status);
                if (WorkerClass.Ins.SupplierRepo.UpdateSupplier(id, suppliername, contractdateDateTime, phone, email, (Session["UserCurrent"] as EmployeeAccount).Email, statusBool))
                {
                    WorkerClass.Ins.Save();
                    return Json(new { success = true, message = "Cập nhật nhà cung cấp thành công." }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { success = false, message = "Cập nhật nhà cung cấp thất bại." }, JsonRequestBehavior.AllowGet);
                }
            }
            catch
            {
                return Json(new { success = false, message = "Cập nhật nhà cung cấp thất bại." }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [ActionName("them-nha-cung-cap")]
        [HasAdminCredential(Role = 2)]
        public ActionResult InsertSupplier(string supplierid, string suppliername, string contractdate, string phone, string email, string status)
        {
            try
            {
                if (string.IsNullOrEmpty(supplierid) || string.IsNullOrEmpty(suppliername) || string.IsNullOrEmpty(contractdate))
                {
                    return Json(new { success = false, message = "Vui lòng nhập đầy đủ thông tin!" }, JsonRequestBehavior.AllowGet);
                }
                if (supplierid.Length > 7)
                {
                    return Json(new { success = false, message = "Mã nhà cung cấp tối đa 7 ký tự!" }, JsonRequestBehavior.AllowGet);
                }
                if (!Regex.IsMatch(contractdate, @"^([0-2][0-9]|(3)[0-1])(\/)(((0)[0-9])|((1)[0-2]))(\/)\d{4}$"))
                {
                    return Json(new { success = false, message = "Ngày hợp tác không đúng định dạng!" }, JsonRequestBehavior.AllowGet);
                }
                if (!Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase))
                {
                    return Json(new { success = false, message = "Email không đúng định dạng!" }, JsonRequestBehavior.AllowGet);
                }
                if (WorkerClass.Ins.SupplierRepo.GetSupplierById(supplierid) != null)
                {
                    return Json(new { success = false, message = "Mã nhà cung cấp đã tồn tại!" }, JsonRequestBehavior.AllowGet);
                }
                suppliername = string.IsNullOrEmpty(suppliername.Trim()) ? null : suppliername.Trim();
                DateTime contractdateDateTime = DateTime.ParseExact(contractdate, "dd/MM/yyyy", null);
                phone = Regex.Replace(phone, @"[^\d]", "");
                email = string.IsNullOrEmpty(email.Trim()) ? null : email.Trim();
                bool statusBool = bool.Parse(status);
                if (WorkerClass.Ins.SupplierRepo.InsertSupplier(supplierid, suppliername, contractdateDateTime, phone, email, (Session["UserCurrent"] as EmployeeAccount).Email, statusBool))
                {
                    WorkerClass.Ins.Save();
                    return Json(new { success = true, message = "Thêm nhà cung cấp thành công." }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { success = false, message = "Thêm nhà cung cấp thất bại." }, JsonRequestBehavior.AllowGet);
                }
            }
            catch
            {
                return Json(new { success = false, message = "Thêm nhà cung cấp thất bại." }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [ActionName("thay-doi-trang-thai-nha-cung-cap")]
        [HasAdminCredential(Role = 2)]
        public ActionResult ChangeStatusSupplier(string id)
        {
            try
            {
                bool status = WorkerClass.Ins.SupplierRepo.ChangeStatusSupplier(id, (Session["UserCurrent"] as EmployeeAccount).Email);
                WorkerClass.Ins.Save();
                return Json(new { success = true, status }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        [ActionName("xoa-nha-cung-cap")]
        [HasAdminCredential(Role = 2)]
        public ActionResult DeleteSupplier(string id)
        {
            try
            {
                if (WorkerClass.Ins.SupplierRepo.DeleteSupplier(id)) 
                { 
                    WorkerClass.Ins.Save();
                    return Json(new { success = true, message = "Xóa nhà cung cấp thành công." }, JsonRequestBehavior.AllowGet);                
                }
                else
                {
                    return Json(new { success = false, message = "Xóa nhà cung cấp thất bại." }, JsonRequestBehavior.AllowGet);
                }
            }
            catch
            {
                return Json(new { success = false, message = "Xóa nhà cung cấp thất bại." }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}