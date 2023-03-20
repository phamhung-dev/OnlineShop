using Model;
using Model.EF;
using OnlineShop.Areas.Manager.Service;
using System.Web.Mvc;

namespace OnlineShop.Areas.Manager.Controllers
{
    public class PersonalInformationController : Controller
    {
        // GET: Manager/PersonalInformation
        [HttpGet]
        [ActionName("thong-tin-ca-nhan")]
        [HasAdminCredential(Role = 2)]
        public ActionResult GetPersonalInformation()
        {
            if (Session["UserCurrent"] == null)
            {
                return RedirectToAction("dang-nhap", "Admin");
            }
            return View("GetPersonalInformation", WorkerClass.Ins.EmployeeAccountRepo.GetEmployeeById((Session["UserCurrent"] as EmployeeAccount).EmployeeID));
        }
    }
}