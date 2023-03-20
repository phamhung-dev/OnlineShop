using Model;
using Model.EF;
using OnlineShop.Areas.Manager.Service;
using System.Web.Mvc;

namespace OnlineShop.Areas.Manager.Controllers
{
    public class AgeController : Controller
    {
        // GET: Manager/Age
        [HttpPost]
        [ActionName("them-do-tuoi")]
        [HasAdminCredential(Role = 2)]
        public ActionResult InsertAge(string agename, string status)
        {
            try
            {
                if (string.IsNullOrEmpty(agename))
                {
                    return Json(new { success = false, message = "Vui lòng nhập đầy đủ thông tin!" }, JsonRequestBehavior.AllowGet);
                }
                if (WorkerClass.Ins.AgeRepo.GetAgeByName(agename) != null)
                {
                    return Json(new { success = false, message = "Độ tuổi này đã tồn tại." }, JsonRequestBehavior.AllowGet);
                }
                agename = string.IsNullOrEmpty(agename.Trim()) ? null : agename.Trim();
                bool statusBool = bool.Parse(status);
                if (WorkerClass.Ins.AgeRepo.InsertAge(agename, (Session["UserCurrent"] as EmployeeAccount).Email, statusBool))
                {
                    WorkerClass.Ins.Save();
                    return Json(new { success = true, age = WorkerClass.Ins.AgeRepo.GetNewAge(), message = "Thêm độ tuổi thành công." }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { success = false, message = "Thêm độ tuổi thất bại." }, JsonRequestBehavior.AllowGet);
                }
            }
            catch
            {
                return Json(new { success = false, message = "Thêm độ tuổi thất bại." }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        [ActionName("thay-doi-trang-thai-do-tuoi")]
        [HasAdminCredential(Role = 2)]
        public ActionResult ChangeStatusAge(string id)
        {
            try
            {
                bool status = WorkerClass.Ins.AgeRepo.ChangeStatusAge(int.Parse(id), (Session["UserCurrent"] as EmployeeAccount).Email);
                WorkerClass.Ins.Save();
                return Json(new { success = true, status }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        [ActionName("cap-nhat-do-tuoi")]
        [HasAdminCredential(Role = 2)]
        public ActionResult UpdateAge(string id, string agename)
        {
            try
            {
                if (string.IsNullOrEmpty(agename))
                {
                    return Json(new { success = false, message = "Vui lòng nhập đầy đủ thông tin!" }, JsonRequestBehavior.AllowGet);
                }
                agename = string.IsNullOrEmpty(agename.Trim()) ? null : agename.Trim();
                if (WorkerClass.Ins.AgeRepo.GetAgeByName(agename) != null)
                {
                    return Json(new { success = false, message = "Độ tuổi này đã tồn tại." }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    if (WorkerClass.Ins.AgeRepo.UpdateAge(int.Parse(id), agename, (Session["UserCurrent"] as EmployeeAccount).Email))
                    {
                        WorkerClass.Ins.Save();
                        return Json(new { success = true, message = "Cập nhật độ tuổi thành công." }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { success = false, message = "Cập nhật độ tuổi thất bại." }, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch
            {
                return Json(new { success = false, message = "Cập nhật độ tuổi thất bại." }, JsonRequestBehavior.AllowGet);
            }

        }
        [HttpPost]
        [ActionName("xoa-do-tuoi")]
        [HasAdminCredential(Role = 2)]
        public ActionResult DeleteAge(string id)
        {
            try
            {
                if (WorkerClass.Ins.AgeRepo.DeleteAge(int.Parse(id)))
                {
                    WorkerClass.Ins.Save();
                    return Json(new { success = true, message = "Xóa độ tuổi thành công." }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { success = false, message = "Xóa độ tuổi thất bại." }, JsonRequestBehavior.AllowGet);
                }
            }
            catch
            {
                return Json(new { success = false, message = "Xóa độ tuổi thất bại." }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}