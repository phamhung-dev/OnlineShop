using Model;
using Model.EF;
using OnlineShop.Areas.Manager.Service;
using System.Web.Mvc;

namespace OnlineShop.Areas.Manager.Controllers
{
    public class SizeController : Controller
    {
        // GET: Manager/Size
        [HttpPost]
        [ActionName("thay-doi-trang-thai-kich-thuoc")]
        [HasAdminCredential(Role = 2)]
        public ActionResult ChangeStatusSize(string id)
        {
            try
            {
                bool status = WorkerClass.Ins.SizeRepo.ChangeStatusSize(id, (Session["UserCurrent"] as EmployeeAccount).Email);
                WorkerClass.Ins.Save();
                return Json(new { success = true, status }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        [ActionName("cap-nhat-kich-thuoc")]
        [HasAdminCredential(Role = 2)]
        public ActionResult UpdateSize(string id, string sizename)
        {
            try
            {
                if (string.IsNullOrEmpty(sizename))
                {
                    return Json(new { success = false, message = "Vui lòng nhập đầy đủ thông tin!" }, JsonRequestBehavior.AllowGet);
                }
                sizename = string.IsNullOrEmpty(sizename.Trim()) ? null : sizename.Trim();
                if (WorkerClass.Ins.SizeRepo.UpdateSize(id, sizename, (Session["UserCurrent"] as EmployeeAccount).Email))
                {
                    WorkerClass.Ins.Save();
                    return Json(new { success = true, message = "Cập nhật kích thước thành công." }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { success = false, message = "Cập nhật kích thước thất bại." }, JsonRequestBehavior.AllowGet);
                }
            }
            catch
            {
                return Json(new { success = false, message = "Cập nhật kích thước thất bại." }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        [ActionName("them-kich-thuoc")]
        [HasAdminCredential(Role = 2)]
        public ActionResult InsertSize(string sizeid, string sizename, string status)
        {
            try
            {
                if (string.IsNullOrEmpty(sizeid) || string.IsNullOrEmpty(sizename))
                {
                    return Json(new { success = false, message = "Vui lòng nhập đầy đủ thông tin!" }, JsonRequestBehavior.AllowGet);
                }
                if (sizeid.Length > 7)
                {
                    return Json(new { success = false, message = "Mã kích thước tối đa 7 ký tự!" }, JsonRequestBehavior.AllowGet);
                }
                if (WorkerClass.Ins.SizeRepo.GetSizeById(sizeid) != null)
                {
                    return Json(new { success = false, message = "Mã kích thước đã tồn tại!" }, JsonRequestBehavior.AllowGet);
                }
                sizename = string.IsNullOrEmpty(sizename.Trim()) ? null : sizename.Trim();
                bool statusBool = bool.Parse(status);
                if (WorkerClass.Ins.SizeRepo.InsertSize(sizeid, sizename, (Session["UserCurrent"] as EmployeeAccount).Email, statusBool))
                {
                    WorkerClass.Ins.Save();
                    return Json(new { success = true, message = "Thêm kích thước thành công." }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { success = false, message = "Thêm kích thước thất bại." }, JsonRequestBehavior.AllowGet);
                }
            }
            catch
            {
                return Json(new { success = false, message = "Thêm kích thước thất bại." }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}