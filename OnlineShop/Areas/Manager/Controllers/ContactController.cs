using Model;
using Model.EF;
using OnlineShop.Areas.Manager.Service;
using System.Web.Mvc;

namespace OnlineShop.Areas.Manager.Controllers
{
    public class ContactController : Controller
    {
        // GET: Manager/Contact
        [HttpPost]
        [ActionName("cap-nhat-thong-tin")]
        [HasAdminCredential(Role = 2)]
        public ActionResult UpdateContact(string idInfo, string input)
        {
            try
            {
                input = string.IsNullOrEmpty(input.Trim()) ? null : input.Trim();
                var contactUpdate = WorkerClass.Ins.ContactRepo.GetContact();
                switch (idInfo)
                {
                    case "Address":
                        {
                            contactUpdate.Address = input;
                            break;
                        }
                    case "LocationOnGoogleMap":
                        {
                            contactUpdate.LocationOnGoogleMap = input;
                            break;
                        }
                    case "Phone":
                        {
                            contactUpdate.Phone = input;
                            break;
                        }
                    case "Email":
                        {
                            contactUpdate.Email = input;
                            break;
                        }
                    case "FacebookLink":
                        {
                            contactUpdate.FacebookLink = input;
                            break;
                        }
                    case "InstagramLink":
                        {
                            contactUpdate.InstagramLink = input;
                            break;
                        }
                    case "TwitterLink":
                        {
                            contactUpdate.TwitterLink = input;
                            break;
                        }
                    default:
                        {
                            return Json(new { success = true, message = "Cập nhật thông tin thất bại." }, JsonRequestBehavior.AllowGet);
                        }
                }
                contactUpdate.UpdateAt = System.DateTime.Now;
                contactUpdate.UpdateBy = (Session["UserCurrent"] as EmployeeAccount).Email;
                WorkerClass.Ins.Save();
                return Json(new { success = true, message = "Cập nhật thông tin thành công." }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { success = true, message = "Cập nhật thông tin thất bại." }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}