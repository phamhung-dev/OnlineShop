using Model;
using System.Web.Mvc;

namespace OnlineShop.Controllers
{
    public class ContactController : Controller
    {
        // GET: Contact
        [HttpGet]
        [ActionName("thong-tin-lien-he")]
        public ActionResult GetContact()
        {
            return View("GetContact", WorkerClass.Ins.ContactRepo.GetContact());
        }
    }
}