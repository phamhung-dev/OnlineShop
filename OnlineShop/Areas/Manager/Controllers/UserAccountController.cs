using Model;
using OnlineShop.Areas.Manager.Service;
using System;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace OnlineShop.Areas.Manager.Controllers
{
    public class UserAccountController : Controller
    {
        // GET: Manager/UserAccount
        [HttpGet]
        [ActionName("xem-tai-khoan")]
        [HasAdminCredential(Role = 1)]
        public ActionResult GetUserById(string id)
        {
            var user = WorkerClass.Ins.UserAccountRepo.GetUserById(new Guid(id));
            var result = new { user.UserID, user.FirstName, user.LastName, user.Avatar, user.Gender, user.Birthday, user.Email, user.Password, user.Phone, user.Address, user.CreateAt, user.Status };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ActionName("cap-nhat-tai-khoan")]
        [HasAdminCredential(Role = 1)]
        public ActionResult UpdateUser(string id, string firstname, string lastname, string gender, string birthday, string phone, string address, string status)
        {
            try
            {
                if ((string.IsNullOrEmpty(firstname) && string.IsNullOrEmpty(lastname)) || string.IsNullOrEmpty(birthday) || string.IsNullOrEmpty(phone))
                {
                    return Json(new { success = false, message = "Vui lòng nhập đầy đủ thông tin!" }, JsonRequestBehavior.AllowGet);
                }
                if (!Regex.IsMatch(birthday, @"^([0-2][0-9]|(3)[0-1])(\/)(((0)[0-9])|((1)[0-2]))(\/)\d{4}$"))
                {
                    return Json(new { success = false, message = "Ngày sinh không đúng định dạng!" }, JsonRequestBehavior.AllowGet);
                }
                Guid idGuid = new Guid(id);
                firstname = string.IsNullOrEmpty(firstname.Trim()) ? null : firstname.Trim();
                lastname = string.IsNullOrEmpty(lastname.Trim()) ? null : lastname.Trim();
                bool genderBool = bool.Parse(gender);
                DateTime birthdayDateTime = DateTime.ParseExact(birthday, "dd/MM/yyyy", null);
                phone = Regex.Replace(phone, @"[^\d]", "");
                address = string.IsNullOrEmpty(address.Trim()) ? null : address.Trim();
                bool statusBool = bool.Parse(status);
                if (WorkerClass.Ins.UserAccountRepo.UpdateUser(idGuid, firstname, lastname, genderBool, birthdayDateTime, phone, address, statusBool))
                {
                    WorkerClass.Ins.Save();
                    return Json(new { success = true, message = "Cập nhật tài khoản thành công." }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { success = false, message = "Cập nhật tài khoản thất bại." }, JsonRequestBehavior.AllowGet);
                }
            }
            catch
            {
                return Json(new { success = false, message = "Cập nhật tài khoản thất bại." }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        [ActionName("thay-doi-trang-thai-tai-khoan")]
        [HasAdminCredential(Role = 1)]
        public ActionResult ChangeStatusUser(string id)
        {
            try
            {
                bool status = WorkerClass.Ins.UserAccountRepo.ChangeStatusUser(new Guid(id));
                WorkerClass.Ins.Save();
                return Json(new { success = true, status }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}