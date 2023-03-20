using CaptchaMvc.HtmlHelpers;
using Model;
using Model.EF;
using OnlineShop.Areas.Manager.Service;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace OnlineShop.Controllers
{
    public class UserAccountController : Controller
    {
        // GET: UserAccount
        [ActionName("dang-nhap")]
        public ActionResult Login()
        {
            if(Session["CustomerCurrent"] != null)
            {
                return RedirectToAction("trang-chu", "Main");
            }
            return View("Login");
        }
        [HttpPost]
        [ActionName("dang-nhap")]
        [ValidateAntiForgeryToken]
        public ActionResult Login(FormCollection collection)
        {
            // Code for validating the Captcha  
            if (!this.IsCaptchaValid("Xác thực mã captcha"))
            {
                ViewBag.ErrorMessage = "Mã captcha không đúng.";
                return View("Login");
            }
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(collection["email"]) || string.IsNullOrEmpty(collection["password"]))
                {
                    ViewBag.LoginFailed = "Vui lòng nhập đầy đủ tài khoản và mật khẩu.";
                    return View("Login");
                }
                var email = collection["email"];
                var password = MD5Hash(Base64Encode(collection["password"]));
                var account = WorkerClass.Ins.UserAccountRepo.Login(email, password);
                if (account != null)
                {
                    if (account.Status == true)
                    {
                        Session["CustomerCurrent"] = account;
                        var listShoppingCartFake = Session["ShoppingCart"] as List<ShoppingCart>;
                        var listShoppingCartReal = WorkerClass.Ins.ShoppingCartRepo.GetShoppingCartByUserID((Session["CustomerCurrent"] as UserAccount).UserID);
                        if (listShoppingCartFake?.Count() > 0)
                        {
                            foreach(var itemFake in listShoppingCartFake)
                            {
                                var itemReal = listShoppingCartReal.FirstOrDefault(x => x.ProductID == itemFake.ProductID && x.SizeID == itemFake.SizeID && x.ColorID == itemFake.ColorID);
                                if(itemReal != null)
                                {
                                    itemReal.Quantity = itemFake.Quantity;
                                }
                                else
                                {
                                    WorkerClass.Ins.ShoppingCartRepo.InsertShoppingCart(new ShoppingCart() { UserID = (Session["CustomerCurrent"] as UserAccount).UserID, ProductID = itemFake.ProductID, SizeID = itemFake.SizeID, ColorID = itemFake.ColorID, Quantity = itemFake.Quantity });
                                }
                            }
                            WorkerClass.Ins.Save();
                        }
                        Session["ShoppingCart"] = WorkerClass.Ins.ShoppingCartRepo.GetShoppingCartByUserID((Session["CustomerCurrent"] as UserAccount).UserID);
                        return RedirectToAction("trang-chu", "Main");
                    }
                    else
                    {
                        ViewBag.LoginFailed = "Tài khoản này đã bị khóa.";
                    }
                }
                else
                {
                    ViewBag.LoginFailed = "Sai tài khoản hoặc mật khẩu.";
                }
            }
            return View("Login");
        }
        [ActionName("dang-xuat")]
        public ActionResult Logout()
        {
            Session["CustomerCurrent"] = null;
            Session["ShoppingCart"] = null;
            return RedirectToAction("dang-nhap","UserAccount");
        }
        [HttpGet]
        [ActionName("dang-ky")]
        public ActionResult Register()
        {
            return View("Register");
        }
        [HttpPost]
        [ActionName("dang-ky")]
        public ActionResult Register(FormCollection collection)
        {
            // Code for validating the Captcha  
            if (!this.IsCaptchaValid("Xác thực mã captcha"))
            {
                ViewBag.ErrorMessage = "Mã captcha không đúng.";
                return View("Register");
            }
            if (string.IsNullOrEmpty(collection["firstname"]) || string.IsNullOrEmpty(collection["lastname"]) || string.IsNullOrEmpty(collection["phone"]) || string.IsNullOrEmpty(collection["address"]) || string.IsNullOrEmpty(collection["email"]) || string.IsNullOrEmpty(collection["password"]) || string.IsNullOrEmpty(collection["retypepassword"]))
            {
                ViewBag.Notification = "Vui lòng nhập đầy đủ thông tin.";
                return View("Register");
            }
            else
            {
                if (!Regex.IsMatch(collection["phone"], @"(84|0[3|5|7|8|9])+([0-9]{8})\b"))
                {
                    ViewBag.Notification = "Số điện thoại không hợp lệ.";
                    return View("Register");
                }
                var checkEmailFormat = CheckEmailFormat(collection["email"]);
                if (checkEmailFormat != null)
                {
                    ViewBag.Notification = checkEmailFormat;
                    return View("Register");
                }
                if (WorkerClass.Ins.UserAccountRepo.PhoneIsUsed(collection["phone"]))
                {
                    ViewBag.Notification = "Số điện thoại đã được sử dụng.";
                    return View("Register");
                }
                if (WorkerClass.Ins.UserAccountRepo.EmailIsUsed(collection["email"]))
                {
                    ViewBag.Notification = "Email đã được sử dụng.";
                    return View("Register");
                }
                var checkPassword = CheckPassword(collection["password"]);
                if (checkPassword != null)
                {
                    ViewBag.Notification = checkPassword;
                    return View("Register");
                }
                if (!collection["password"].Equals(collection["retypepassword"]))
                {
                    ViewBag.Notification = "Mật khẩu nhập lại không khớp.";
                    return View("Register");
                }
                if (WorkerClass.Ins.UserAccountRepo.InsertUserAccount(collection["firstname"].Trim(), collection["lastname"].Trim(), collection["phone"].Trim(), collection["address"].Trim(), collection["email"].Trim(), MD5Hash(Base64Encode(collection["password"]))))
                {
                    WorkerClass.Ins.Save();
                    ViewBag.Notification = "Đăng ký tài khoản thành công.";
                    return View("Register");
                }
                ViewBag.Notification = "Đăng ký tài khoản thất bại.";
                return View("Register");
            }
        }
        [HttpGet]
        [ActionName("quen-mat-khau")]
        public ActionResult ForgotPassword()
        {
            return View("ForgotPassword");
        }
        [HttpPost]
        [ActionName("quen-mat-khau")]
        public ActionResult ForgotPassword(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return Json(new { success = false, message = "Vui lòng nhập email." }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var checkEmailFormat = CheckEmailFormat(email);
                if (checkEmailFormat != null)
                {
                    return Json(new { success = false, message = checkEmailFormat }, JsonRequestBehavior.AllowGet);
                }
                var userAccount = WorkerClass.Ins.UserAccountRepo.GetUserByEmail(email);
                if (userAccount != null)
                {
                    var newPassword = StrongPasswordService.GeneratePassword(8);
                    string emailTo = userAccount.Email;
                    string subject = "LẤY LẠI MẬT KHẨU - YOLO Shop";
                    string content = "Mật khẩu mới của bạn là: " + newPassword;
                    string body = string.Format("Bạn vừa nhận được liên hệ từ: <b>YOLO Shop</b><br/>Email: {0}<br/>Nội dung:<br/>{1}<br/>Mọi thắc mắc xin liên hệ qua link facebook: <a href= \" https://www.facebook.com/kunn.ngoc.5 \">https://www.facebook.com/kunn.ngoc.5</a><br/>", ConfigurationManager.AppSettings["smtpUserName"].ToString(), content);
                    if (EmailService.Send(emailTo, subject, body))
                    {
                        userAccount.Password = MD5Hash(Base64Encode(newPassword));
                        WorkerClass.Ins.Save();
                        return Json(new { success = true, message = "Vui lòng kiểm tra email đã đăng ký tài khoản." }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { success = false, message = "Lấy lại mật khẩu thất bại." }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json(new { success = false, message = "Email này chưa đăng ký tài khoản." }, JsonRequestBehavior.AllowGet);
                }
            }
        }
        [HttpGet]
        [ActionName("thong-tin-ca-nhan")]
        public ActionResult PersonalInformation()
        {
            var ListHistoryOrder = WorkerClass.Ins.ProductOrderRepo.GetHistoryOrder((Session["CustomerCurrent"] as UserAccount).UserID);
            if(ListHistoryOrder == null)
            {
                ViewBag.ListHistoryOrder = new List<ProductOrder>();
            }
            else
            {
                ViewBag.ListHistoryOrder = ListHistoryOrder;
            }
            return View("PersonalInformation", Session["CustomerCurrent"] as UserAccount);
        }
        [HttpPost]
        [ActionName("xac-nhan-da-nhan-hang")]
        public ActionResult ConfirmProductReceived(string invoiceid)
        {
            try
            {
                Guid invoiceID = new Guid(invoiceid);
                if (WorkerClass.Ins.InvoiceRepo.ConfirmProductReceived(invoiceID))
                {
                    WorkerClass.Ins.Save();
                    return Json(new { success = true }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { success = false, message = "Lỗi." }, JsonRequestBehavior.AllowGet);
                }
            }
            catch
            {
                return Json(new { success = false, message = "Lỗi." }, JsonRequestBehavior.AllowGet);
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
        [HttpPost]
        [ActionName("cap-nhat-thong-tin")]
        public ActionResult UpdateUserAccount(string id, string firstname, string lastname, string gender, string birthday, string phone, string address)
        {
            try
            {
                if ((string.IsNullOrEmpty(firstname) && string.IsNullOrEmpty(lastname)) || string.IsNullOrEmpty(gender) || string.IsNullOrEmpty(birthday) || string.IsNullOrEmpty(phone) || string.IsNullOrEmpty(address))
                {
                    return Json(new { success = false, message = "Vui lòng nhập đầy đủ thông tin!" }, JsonRequestBehavior.AllowGet);
                }
                Guid idGuid = new Guid(id);
                firstname = string.IsNullOrEmpty(firstname.Trim()) ? null : firstname.Trim();
                lastname = string.IsNullOrEmpty(lastname.Trim()) ? null : lastname.Trim();
                bool genderBool = bool.Parse(gender.Equals("1") ? "true" : "false");
                DateTime birthdayDateTime = DateTime.Parse(birthday);
                if(birthdayDateTime > DateTime.Now.AddYears(-18))
                {
                    return Json(new { success = false, message = "Số tuổi phải lớn hơn 18." }, JsonRequestBehavior.AllowGet);
                }
                if (!Regex.IsMatch(phone, @"(84|0[3|5|7|8|9])+([0-9]{8})\b"))
                {
                    return Json(new { success = false, message = "Số điện thoại không hợp lệ." }, JsonRequestBehavior.AllowGet);
                }
                if (!phone.Equals((Session["CustomerCurrent"] as UserAccount).Phone))
                {
                    if (WorkerClass.Ins.EmployeeAccountRepo.PhoneIsUsed(phone))
                    {
                        return Json(new { success = false, message = "Số điện thoại đã được sử dụng." }, JsonRequestBehavior.AllowGet);
                    }
                }
                address = string.IsNullOrEmpty(address.Trim()) ? null : address.Trim();


                if (WorkerClass.Ins.UserAccountRepo.UpdateUser(idGuid, firstname, lastname, genderBool, birthdayDateTime, phone, address, true))
                {
                    WorkerClass.Ins.Save();
                    Session["CustomerCurrent"] = WorkerClass.Ins.UserAccountRepo.GetUserById(idGuid);
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
                    if (WorkerClass.Ins.UserAccountRepo.UpdateAvatar((Session["CustomerCurrent"] as UserAccount).UserID, avatar))
                    {
                        WorkerClass.Ins.Save();
                        DeleteFilesInFolder("~/Asset/Upload");
                        Session["CustomerCurrent"] = WorkerClass.Ins.UserAccountRepo.GetUserById((Session["CustomerCurrent"] as UserAccount).UserID);
                        return Json(new { success = true, message = "Cập nhật ảnh đại diện thành công.", avatar = Convert.ToBase64String(avatar) }, JsonRequestBehavior.AllowGet);
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
        [HttpGet]
        [ActionName("gui-ma-xac-nhan-email")]
        public ActionResult SendEmailConfirmCode(string id)
        {
            try
            {
                Guid idGuid = new Guid(id);
                var userAccount = WorkerClass.Ins.UserAccountRepo.GetUserById(idGuid);
                if(userAccount != null)
                {
                    string code = new Random().Next(10000000, 99999999).ToString();
                    string emailTo = userAccount.Email;
                    string subject = "XÁC NHẬN EMAIL - YOLO Shop";
                    string content = "Mã xác nhận của bạn là: " + code;
                    string body = string.Format("Bạn vừa nhận được liên hệ từ: <b>YOLO Shop</b><br/>Email: {0}<br/>Nội dung:<br/>{1}<br/>Mã này có hiệu lực trong vòng 2 phút<br/>Mọi thắc mắc xin liên hệ qua link facebook: <a href= \" https://www.facebook.com/kunn.ngoc.5 \">https://www.facebook.com/kunn.ngoc.5</a><br/>", ConfigurationManager.AppSettings["smtpUserName"].ToString(), content);
                    if (EmailService.Send(emailTo, subject, body))
                    {
                        userAccount.EmailConfirmCode = code;
                        userAccount.CreateEmailConfirmCodeAt = DateTime.Now;
                        WorkerClass.Ins.Save();
                        return Json(new { success = true, message = "Vui lòng kiểm tra email để nhận mã xác nhận." }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { success = false, message = "Xác nhận email thất bại." }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json(new { success = false, message = "Xác nhận email thất bại." }, JsonRequestBehavior.AllowGet);
                }
            }
            catch
            {
                return Json(new { success = false, message = "Xác nhận email thất bại." }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        [ActionName("xac-nhan-email")]
        public ActionResult ConfirmEmail(string id, string code)
        {
            try
            {
                if (string.IsNullOrEmpty(code))
                {
                    return Json(new { success = false, message = "Vui lòng nhập mã xác nhận." }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    Guid idGuid = new Guid(id);
                    var userAccount = WorkerClass.Ins.UserAccountRepo.GetUserById(idGuid);
                    if(userAccount != null)
                    {
                        if (!userAccount.EmailConfirmCode.Equals(code))
                        {
                            return Json(new { success = false, message = "Mã xác nhận không khớp." }, JsonRequestBehavior.AllowGet);
                        }
                        if((DateTime.Now - userAccount.CreateEmailConfirmCodeAt.Value).TotalMinutes > 2)
                        {
                            return Json(new { success = false, message = "Mã xác nhận này đã hết hiệu lực." }, JsonRequestBehavior.AllowGet);
                        }
                        if (WorkerClass.Ins.UserAccountRepo.ConfirmEmail(idGuid))
                        {
                            WorkerClass.Ins.Save();
                            Session["CustomerCurrent"] = WorkerClass.Ins.UserAccountRepo.GetUserById(idGuid);
                            return Json(new { success = true, message = "Xác nhận email thành công." }, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            return Json(new { success = false, message = "Xác nhận email thất bại." }, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        return Json(new { success = false, message = "Xác nhận email thất bại." }, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch
            {
                return Json(new { success = false, message = "Xác nhận email thất bại." }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        [ActionName("thay-doi-mat-khau")]
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
                    if (!WorkerClass.Ins.UserAccountRepo.CheckPassword((Session["CustomerCurrent"] as UserAccount).UserID, MD5Hash(Base64Encode(oldpassword))))
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
                    if (WorkerClass.Ins.UserAccountRepo.ChangePassword((Session["CustomerCurrent"] as UserAccount).UserID, MD5Hash(Base64Encode(newpassword))))
                    {
                        WorkerClass.Ins.Save();
                        return Json(new { success = true, message = "Thay đổi mật khẩu thành công." }, JsonRequestBehavior.AllowGet);
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
        private string CheckEmailFormat(string email)
        {
            var check = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            string error = null;
            if (!check.IsMatch(email))
            {
                error = "Sai định dạng email!";
                return error;
            }
            return error;
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
    }
}