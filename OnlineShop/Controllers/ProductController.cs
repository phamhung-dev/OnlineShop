using Model;
using Model.EF;
using System.Linq;
using System.Web.Mvc;

namespace OnlineShop.Controllers
{
    public class ProductController : Controller
    {
        // GET: Product
        [HttpGet]
        [ActionName("danh-sach-san-pham")]
        public ActionResult GetAllProductSelling()
        {
            ViewBag.ListSupplier = WorkerClass.Ins.SupplierRepo.GetAllSupplierContacting().OrderBy(x => x.SupplierName).ToList();
            ViewBag.ListAge = WorkerClass.Ins.AgeRepo.GetAllAgeSelling().OrderBy(x => x.AgeName).ToList();
            return View("GetAllProductSelling", WorkerClass.Ins.ProductRepo.GetAllProductSelling().OrderBy(x => ((double)x.UnitSellPrice) * (1 - x.DiscountPercent / 100)).ToList());
        }
        [HttpGet]
        [ActionName("tim-kiem-san-pham")]
        public ActionResult SearchingProductByName(string id)
        {
            id = id.Replace('-', ' ');
            ViewBag.Keyword = id;
            ViewBag.ListSupplier = WorkerClass.Ins.SupplierRepo.GetAllSupplierContacting().OrderBy(x => x.SupplierName).ToList();
            ViewBag.ListAge = WorkerClass.Ins.AgeRepo.GetAllAgeSelling().OrderBy(x => x.AgeName).ToList();
            return View("SearchingProductByName", WorkerClass.Ins.ProductRepo.GetAllProductSelling().Where(x => x.ProductName.ToLower().Contains(id.ToLower())).ToList());
        }

        [HttpGet]
        [ActionName("chi-tiet-san-pham")]
        public ActionResult ProductDetail(string id)
        {          
            return View("ProductDetail", WorkerClass.Ins.ProductRepo.GetProductById(id));
        }
        [HttpPost]
        [ActionName("yeu-thich-san-pham")]
        public ActionResult FavouriteProduct(string productid)
        {
            if(Session["CustomerCurrent"] == null)
            {
                return Json(new { success = false, url = "/dang-nhap" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                if(WorkerClass.Ins.FavouriteRepo.InsertFavourite((Session["CustomerCurrent"] as UserAccount).UserID, productid))
                {
                    WorkerClass.Ins.Save();
                    return Json(new { success = true }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { success = false, url = "/dang-nhap" }, JsonRequestBehavior.AllowGet);
                }
            }
        }
        [HttpPost]
        [ActionName("huy-yeu-thich-san-pham")]
        public ActionResult UnfavouriteProduct(string productid)
        {
            if (Session["CustomerCurrent"] == null)
            {
                return Json(new { success = false, url = "/dang-nhap" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                if (WorkerClass.Ins.FavouriteRepo.DeleteFavourite((Session["CustomerCurrent"] as UserAccount).UserID, productid))
                {
                    WorkerClass.Ins.Save();
                    return Json(new { success = true }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { success = false, url = "/dang-nhap" }, JsonRequestBehavior.AllowGet);
                }
            }
        }
        [HttpPost]
        [ActionName("danh-gia-san-pham")]
        public ActionResult Rating(string productid, string scorerating)
        {
            try
            {
                byte scoreRating = byte.Parse(scorerating);
                var evaluation = WorkerClass.Ins.EvaluationRepo.GetEvaluationById((Session["CustomerCurrent"] as UserAccount).UserID, productid);
                var product = WorkerClass.Ins.ProductRepo.GetProductById(productid);
                var amountEvaluation = WorkerClass.Ins.EvaluationRepo.GetAmountEvaluationById(productid);
                var scoreRatingProduct = WorkerClass.Ins.ProductRepo.GetScoreRatingById(productid);
                if (evaluation == null)
                {
                    if (WorkerClass.Ins.EvaluationRepo.InsertEvaluation((Session["CustomerCurrent"] as UserAccount).UserID, productid, scoreRating))
                    {
                        product.ScoreRating = ( scoreRatingProduct * amountEvaluation + scoreRating) / (amountEvaluation + 1);
                        WorkerClass.Ins.Save();
                        return Json(new { success = true }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { success = false, message = "Đánh giá thất bại." }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    product.ScoreRating = (scoreRatingProduct * amountEvaluation + (scoreRating - evaluation.ScoreRating)) / amountEvaluation;
                    evaluation.ScoreRating = scoreRating;
                    WorkerClass.Ins.Save();
                    return Json(new { success = true }, JsonRequestBehavior.AllowGet);
                }
            }
            catch
            {
                return Json(new { success = false, message = "Đánh giá thất bại." }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        [ActionName("viet-phan-hoi")]
        public ActionResult WriteFeedback(string productid, string content)
        {
            try
            {
                if(WorkerClass.Ins.FeedbackRepo.InsertFeedback((Session["CustomerCurrent"] as UserAccount).UserID, productid, content))
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
    }
}