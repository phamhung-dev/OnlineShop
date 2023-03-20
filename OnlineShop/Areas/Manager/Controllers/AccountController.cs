using Model;
using OnlineShop.Areas.Manager.Service;
using System.Web.Mvc;

namespace OnlineShop.Areas.Manager.Controllers
{
    public class AccountController : Controller
    {
        // GET: Manager/Account
        [ActionName("danh-sach-tai-khoan-khach-hang")]
        [HasAdminCredential(Role = 1)]
        public ActionResult GetAllUserAccount()
        {
            if(Session["UserCurrent"] == null)
            {
                return RedirectToAction("dang-nhap", "Admin");
            }
            return View("GetAllUserAccount",WorkerClass.Ins.UserAccountRepo.GetAllUserAccount());
        }
        [ActionName("danh-sach-tai-khoan-nhan-vien")]
        [HasAdminCredential(Role = 1)]
        public ActionResult GetAllEmployeeAccount()
        {
            if (Session["UserCurrent"] == null)
            {
                return RedirectToAction("dang-nhap", "Admin");
            }
            return View("GetAllEmployeeAccount", WorkerClass.Ins.EmployeeAccountRepo.GetAllEmployeeAccount());
        }
    }
}