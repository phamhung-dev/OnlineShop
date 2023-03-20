using Model;
using Model.EF;
using OnlineShop.Areas.Manager.Service;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace OnlineShop.Areas.Manager.Controllers
{
    public class EmployeeAccountController : Controller
    {
        // GET: Manager/EmployeeAccount
        [HttpGet]
        [ActionName("xem-tai-khoan")]
        [HasAdminCredential(Role = 1)]
        public ActionResult GetEmployeeById(string id)
        {
            var employee = WorkerClass.Ins.EmployeeAccountRepo.GetEmployeeById(new Guid(id));
            var result = new { employee.EmployeeID, employee.Avatar, employee.FirstName, employee.LastName, employee.Email, employee.Password, employee.Phone, employee.Address, employee.Role, employee.CreateAt, employee.Status };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ActionName("cap-nhat-tai-khoan")]
        [HasAdminCredential(Role = 1)]
        public ActionResult UpdateEmployee(string id, string firstname, string lastname, string phone, string address, string role, string status)
        {
            try
            {
                if ((string.IsNullOrEmpty(firstname) && string.IsNullOrEmpty(lastname)) || string.IsNullOrEmpty(phone))
                {
                    return Json(new { success = false, message = "Vui lòng nhập đầy đủ thông tin!" }, JsonRequestBehavior.AllowGet);
                }
                Guid idGuid = new Guid(id);
                firstname = string.IsNullOrEmpty(firstname.Trim()) ? null : firstname.Trim();
                lastname = string.IsNullOrEmpty(lastname.Trim()) ? null : lastname.Trim();
                phone = Regex.Replace(phone, @"[^\d]", "");
                address = string.IsNullOrEmpty(address.Trim()) ? null : address.Trim();
                int roleInt = int.Parse(role);
                bool statusBool = bool.Parse(status);
                if (WorkerClass.Ins.EmployeeAccountRepo.UpdateEmployee(idGuid, firstname, lastname, phone, address, roleInt, statusBool))
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
        public ActionResult ChangeStatusEmployee(string id)
        {
            try
            {
                bool status = WorkerClass.Ins.EmployeeAccountRepo.ChangeStatusEmployee(new Guid(id));
                WorkerClass.Ins.Save();
                return Json(new { success = true, status }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        [ActionName("cap-nhat-thong-tin")]
        [HasAdminCredential(Role = 1)]
        public ActionResult UpdateEmployeeInfo(string id, string firstname, string lastname, string phone, string address)
        {
            try
            {
                if ((string.IsNullOrEmpty(firstname) && string.IsNullOrEmpty(lastname)) || string.IsNullOrEmpty(phone))
                {
                    return Json(new { success = false, message = "Vui lòng nhập đầy đủ thông tin!" }, JsonRequestBehavior.AllowGet);
                }
                Guid idGuid = new Guid(id);
                firstname = string.IsNullOrEmpty(firstname.Trim()) ? null : firstname.Trim();
                lastname = string.IsNullOrEmpty(lastname.Trim()) ? null : lastname.Trim();
                if (!Regex.IsMatch(phone, @"(84|0[3|5|7|8|9])+([0-9]{8})\b"))
                {
                    return Json(new { success = false, message = "Số điện thoại không hợp lệ." }, JsonRequestBehavior.AllowGet);
                }
                if(!phone.Equals((Session["UserCurrent"] as EmployeeAccount).Phone))
                {
                    if (WorkerClass.Ins.EmployeeAccountRepo.PhoneIsUsed(phone))
                    {
                        return Json(new { success = false, message = "Số điện thoại đã được sử dụng." }, JsonRequestBehavior.AllowGet);
                    }
                }
                address = string.IsNullOrEmpty(address.Trim()) ? null : address.Trim();
                if (WorkerClass.Ins.EmployeeAccountRepo.UpdateEmployee(idGuid, firstname, lastname, phone, address, (Session["UserCurrent"] as EmployeeAccount).Role, (Session["UserCurrent"] as EmployeeAccount).Status))
                {
                    WorkerClass.Ins.Save();
                    Session["UserCurrent"] = WorkerClass.Ins.EmployeeAccountRepo.GetEmployeeById(idGuid);
                    return Json(new { success = true, message = "Cập nhật thông tin thành công." }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { success = false, message = "Cập nhật thông tin thất bại." }, JsonRequestBehavior.AllowGet);
                }
            }
            catch
            {
                return Json(new { success = false, message = "Cập nhật thông tin thất bại." }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [ActionName("cap-nhat-anh-dai-dien")]
        [HasAdminCredential(Role = 1)]
        public ActionResult UpdateAvatar(HttpPostedFileBase file)
        {
            try
            {
                if (file == null)
                {
                    return Json(new { success = false, message = "Cập nhật ảnh đại diện thất bại." }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    file.SaveAs(Server.MapPath("~/Asset/Upload/" + file.FileName));
                    var path = "~/Asset/Upload/" + file.FileName;
                    var avatar = converImagetoByte(path);
                    if (WorkerClass.Ins.EmployeeAccountRepo.UpdateAvatar((Session["UserCurrent"] as EmployeeAccount).EmployeeID, avatar))
                    {
                        WorkerClass.Ins.Save();
                        DeleteFilesInFolder("~/Asset/Upload");
                        return Json(new { success = true, message = "Cập nhật ảnh đại diện thành công.", avatar = Convert.ToBase64String(avatar)}, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { success = false, message = "Cập nhật ảnh đại diện thất bại." }, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch
            {
                return Json(new { success = false, message = "Cập nhật ảnh đại diện thất bại." }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        [ActionName("thay-doi-mat-khau")]
        [HasAdminCredential(Role = 1)]
        public ActionResult ChangePassword(string oldpassword, string newpassword, string retypenewpassword)
        {
            try
            {
                if (string.IsNullOrEmpty(oldpassword) || string.IsNullOrEmpty(newpassword) || string.IsNullOrEmpty(retypenewpassword))
                {
                    return Json(new { success = false, message = "Vui lòng nhập đầy đủ thông tin." }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    if(!WorkerClass.Ins.EmployeeAccountRepo.CheckPassword((Session["UserCurrent"] as EmployeeAccount).EmployeeID, MD5Hash(Base64Encode(oldpassword))))
                    {
                        return Json(new { success = false, message = "Mật khẩu cũ không đúng." }, JsonRequestBehavior.AllowGet);
                    }
                    var checkPassword = CheckPassword(newpassword);
                    if (checkPassword != null)
                    {
                        return Json(new { success = false, message = checkPassword }, JsonRequestBehavior.AllowGet);
                    }
                    if (!newpassword.Equals(retypenewpassword))
                    {
                        return Json(new { success = false, message = "Mật khẩu nhập lại không khớp." }, JsonRequestBehavior.AllowGet);
                    }
                    if (WorkerClass.Ins.EmployeeAccountRepo.ChangePassword((Session["UserCurrent"] as EmployeeAccount).EmployeeID, MD5Hash(Base64Encode(newpassword))))
                    {
                        WorkerClass.Ins.Save();
                        return Json(new { success = true, message = "Thay đổi mật khẩu thành công."}, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { success = false, message = "Thay đổi mật khẩu thất bại." }, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch
            {
                return Json(new { success = false, message = "Thay đổi mật khẩu thất bại." }, JsonRequestBehavior.AllowGet);
            }
        }
        private byte[] converImagetoByte(string url)
        {
            var path = Server.MapPath(url);
            FileStream fs;
            fs = new FileStream(path, FileMode.Open, FileAccess.Read);
            byte[] picbyte = new byte[fs.Length];
            fs.Read(picbyte, 0, System.Convert.ToInt32(fs.Length));
            fs.Close();
            return picbyte;
        }
        private void DeleteFilesInFolder(string input)
        {
            var path = Server.MapPath(input);
            System.IO.DirectoryInfo di = new DirectoryInfo(path);

            foreach (FileInfo file in di.GetFiles())
            {
                file.Delete();
            }
        }
        private string CheckPassword(string password)
        {
            var hasNumber = new Regex(@"[0-9]+");
            var hasUpperChar = new Regex(@"[A-Z]+");
            var hasLowerChar = new Regex(@"[a-z]+");
            var hasMinimum8Chars = new Regex(@".{8,}");
            var hasSymbols = new Regex(@"[!@#$%^&*()_+=\[{\]};:<>|./?,-]");
            string error = null;
            if (!hasMinimum8Chars.IsMatch(password))
            {
                error = "Mật khẩu phải chứa ít nhất 8 ký tự.";
                return error;
            }
            else if (!hasLowerChar.IsMatch(password))
            {
                error = "Mật khẩu phải chứa ít nhất một chữ cái thường.";
                return error;
            }
            else if (!hasUpperChar.IsMatch(password))
            {
                error = "Mật khẩu phải chứa ít nhất một chữ cái hoa.";
                return error;
            }
            else if (!hasNumber.IsMatch(password))
            {
                error = "Mật khẩu phải chứa ít nhất một chữ số.";
                return error;
            }

            else if (!hasSymbols.IsMatch(password))
            {
                error = "Mật khẩu phải chứa ít nhất một ký tự đặc biệt.";
                return error;
            }
            else
            {
                return error;
            }
        }
        private static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }
        private static string MD5Hash(string input)
        {
            StringBuilder hash = new StringBuilder();
            MD5CryptoServiceProvider md5provider = new MD5CryptoServiceProvider();
            byte[] bytes = md5provider.ComputeHash(new UTF8Encoding().GetBytes(input));

            for (int i = 0; i < bytes.Length; i++)
            {
                hash.Append(bytes[i].ToString("x2"));
            }
            return hash.ToString();
        }
    }
}