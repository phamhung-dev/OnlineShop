using Model;
using OnlineShop.Areas.Manager.Service;
using System.Configuration;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace OnlineShop.Areas.Manager.Controllers
{
    public class AdminController : Controller
    {
        // GET: Manager/Admin
        [HttpGet]
        [ActionName("dang-nhap")]
        public ActionResult Login()
        {
            if (Session["UserCurrent"] != null)
            {
                return RedirectToAction("trang-chu", "Home");
            }
            return View("Login");
        }
        [HttpPost]
        [ActionName("dang-nhap")]
        [ValidateAntiForgeryToken]
        public ActionResult Login(FormCollection collection)
        {
            if (ModelState.IsValid)
            {
                if(string.IsNullOrEmpty(collection["email"]) || string.IsNullOrEmpty(collection["password"]))
                {
                    ViewBag.LoginFailed = "Vui lòng nhập đầy đủ tài khoản và mật khẩu.";
                    return View("Login");
                }
                var email = collection["email"];
                var password = MD5Hash(Base64Encode (collection["password"]));
                var account = WorkerClass.Ins.EmployeeAccountRepo.Login(email, password);
                if (account != null)
                {
                    if (account.Status == true)
                    {
                        Session["UserCurrent"] = account;
                        if(account.Role == 1)
                        {
                            return RedirectToAction("trang-chu", "Home");
                        }
                        else
                        {
                            return RedirectToAction("thong-tin-ca-nhan", "PersonalInformation");
                        }
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
        [HttpGet]
        [ActionName("dang-xuat")]
        public ActionResult Logout()
        {
            Session["UserCurrent"] = null;
            return RedirectToAction("dang-nhap", "Admin");
        }
        [HttpGet]
        [ActionName("quen-mat-khau")]
        public ActionResult ForgotPassword()
        {
            return View("ForgotPassword");
        }
        [HttpPost]
        [ActionName("quen-mat-khau")]
        public ActionResult ForgotPassword(FormCollection collection)
        {
            if (string.IsNullOrEmpty(collection["email"]))
            {
                ViewBag.Notification = "Vui lòng nhập email.";
                return View("ForgotPassword");
            }
            else
            {
                var employee = WorkerClass.Ins.EmployeeAccountRepo.GetEmployeeByEmail(collection["email"]);
                if(employee != null)
                {
                    var newPassword = StrongPasswordService.GeneratePassword(8);
                    string emailTo = employee.Email;
                    string subject = "LẤY LẠI MẬT KHẨU - YOLO Shop";
                    string content = "Mật khẩu mới của bạn là: " + newPassword;
                    string body = string.Format("Bạn vừa nhận được liên hệ từ: <b>YOLO Shop</b><br/>Email: {0}<br/>Nội dung:<br/>{1}<br/>Mọi thắc mắc xin liên hệ qua link facebook: <a href= \" https://www.facebook.com/kunn.ngoc.5 \">https://www.facebook.com/kunn.ngoc.5</a><br/>", ConfigurationManager.AppSettings["smtpUserName"].ToString(), content);
                    if(EmailService.Send(emailTo, subject, body))
                    {
                        employee.Password = MD5Hash(Base64Encode(newPassword));
                        WorkerClass.Ins.Save();
                        ViewBag.Notification = "Vui lòng kiểm tra email đã đăng ký.";
                        return View("ForgotPassword");
                    }
                    else
                    {
                        ViewBag.Notification = "Lấy lại mật khẩu thất bại.";
                        return View("ForgotPassword");
                    }
                }
                else
                {
                    ViewBag.Notification = "Email này chưa đăng ký tài khoản.";
                    return View("ForgotPassword");
                }
            }
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
            if (string.IsNullOrEmpty(collection["firstname"]) || string.IsNullOrEmpty(collection["lastname"]) || string.IsNullOrEmpty(collection["email"]) || string.IsNullOrEmpty(collection["password"]) || string.IsNullOrEmpty(collection["retypepassword"]) || string.IsNullOrEmpty(collection["phone"]))
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
                if (WorkerClass.Ins.EmployeeAccountRepo.PhoneIsUsed(collection["phone"]))
                {
                    ViewBag.Notification = "Số điện thoại đã được sử dụng.";
                    return View("Register");
                }
                if (WorkerClass.Ins.EmployeeAccountRepo.EmailIsUsed(collection["email"]))
                {
                    ViewBag.Notification = "Email đã được sử dụng.";
                    return View("Register");
                }
                var checkPassword = CheckPassword(collection["password"]);
                if(checkPassword != null)
                {
                    ViewBag.Notification = checkPassword;
                    return View("Register");
                }
                if (!collection["password"].Equals(collection["retypepassword"]))
                {
                    ViewBag.Notification = "Mật khẩu nhập lại không khớp.";
                    return View("Register");
                }
                if (WorkerClass.Ins.EmployeeAccountRepo.InsertEmployeeAccount(collection["firstname"].Trim(), collection["lastname"].Trim(), collection["email"].Trim(), collection["phone"].Trim(), MD5Hash(Base64Encode(collection["password"].Trim())))){
                    WorkerClass.Ins.Save();
                    ViewBag.Notification = "Đăng ký tài khoản thành công.";
                    return View("Register");
                }
                ViewBag.Notification = "Đăng ký tài khoản thất bại.";
                return View("Register");
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