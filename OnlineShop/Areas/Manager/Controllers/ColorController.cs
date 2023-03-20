using Model;
using Model.EF;
using OnlineShop.Areas.Manager.Service;
using System.Web.Mvc;

namespace OnlineShop.Areas.Manager.Controllers
{
    public class ColorController : Controller
    {
        // GET: Manager/Color
        [HttpPost]
        [ActionName("thay-doi-trang-thai-mau-sac")]
        [HasAdminCredential(Role = 2)]
        public ActionResult ChangeStatusColor(string id)
        {
            try
            {
                bool status = WorkerClass.Ins.ColorRepo.ChangeStatusColor(id, (Session["UserCurrent"] as EmployeeAccount).Email);
                WorkerClass.Ins.Save();
                return Json(new { success = true, status }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        [ActionName("cap-nhat-mau-sac")]
        [HasAdminCredential(Role = 2)]
        public ActionResult UpdateColor(string id, string colorname)
        {
            try
            {
                if (string.IsNullOrEmpty(colorname))
                {
                    return Json(new { success = false, message = "Vui lòng nhập đầy đủ thông tin!" }, JsonRequestBehavior.AllowGet);
                }
                colorname = string.IsNullOrEmpty(colorname.Trim()) ? null : colorname.Trim();
                if (WorkerClass.Ins.ColorRepo.UpdateColor(id, colorname, (Session["UserCurrent"] as EmployeeAccount).Email))
                {
                    WorkerClass.Ins.Save();
                    return Json(new { success = true, message = "Cập nhật màu sắc thành công." }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { success = false, message = "Cập nhật màu sắc thất bại." }, JsonRequestBehavior.AllowGet);
                }
            }
            catch
            {
                return Json(new { success = false, message = "Cập nhật màu sắc thất bại." }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        [ActionName("them-mau-sac")]
        [HasAdminCredential(Role = 2)]
        public ActionResult InsertColor(string colorid, string colorname, string status)
        {
            try
            {
                if (string.IsNullOrEmpty(colorid) || string.IsNullOrEmpty(colorname))
                {
                    return Json(new { success = false, message = "Vui lòng nhập đầy đủ thông tin!" }, JsonRequestBehavior.AllowGet);
                }
                if (WorkerClass.Ins.ColorRepo.GetColorById(colorid) != null)
                {
                    return Json(new { success = false, message = "Mã màu đã tồn tại!" }, JsonRequestBehavior.AllowGet);
                }
                colorname = string.IsNullOrEmpty(colorname.Trim()) ? null : colorname.Trim();
                bool statusBool = bool.Parse(status);
                if (WorkerClass.Ins.ColorRepo.InsertColor(colorid, colorname, (Session["UserCurrent"] as EmployeeAccount).Email, statusBool))
                {
                    WorkerClass.Ins.Save();
                    return Json(new { success = true, message = "Thêm màu thành công." }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { success = false, message = "Thêm màu thất bại." }, JsonRequestBehavior.AllowGet);
                }
            }
            catch
            {
                return Json(new { success = false, message = "Thêm màu thất bại." }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}