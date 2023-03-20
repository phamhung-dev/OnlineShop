using Model;
using Model.EF;
using OnlineShop.Areas.Manager.Service;
using System.Web.Mvc;

namespace OnlineShop.Areas.Manager.Controllers
{
    public class ProductCategoryController : Controller
    {
        // GET: Manager/ProductCategory
        [HttpPost]
        [ActionName("thay-doi-trang-thai-loai-san-pham")]
        [HasAdminCredential(Role = 2)]
        public ActionResult ChangeStatusProductCategory(string id)
        {
            try
            {
                bool status = WorkerClass.Ins.ProductCategoryRepo.ChangeStatusProductCategory(int.Parse(id), (Session["UserCurrent"] as EmployeeAccount).Email);
                WorkerClass.Ins.Save();
                return Json(new { success = true, status }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        [ActionName("cap-nhat-loai-san-pham")]
        [HasAdminCredential(Role = 2)]
        public ActionResult UpdateProductCategory(string id, string productcategoryname)
        {
            try
            {
                if (string.IsNullOrEmpty(productcategoryname))
                {
                    return Json(new { success = false, message = "Vui lòng nhập đầy đủ thông tin!" }, JsonRequestBehavior.AllowGet);
                }
                productcategoryname = string.IsNullOrEmpty(productcategoryname.Trim()) ? null : productcategoryname.Trim();
                if(WorkerClass.Ins.ProductCategoryRepo.GetProductCategoryByName(productcategoryname) != null)
                {
                    return Json(new { success = false, message = "Loại sản phẩm này đã tồn tại." }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    if (WorkerClass.Ins.ProductCategoryRepo.UpdateProductCategory(int.Parse(id), productcategoryname, (Session["UserCurrent"] as EmployeeAccount).Email))
                    {
                        WorkerClass.Ins.Save();
                        return Json(new { success = true, message = "Cập nhật loại sản phẩm thành công." }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { success = false, message = "Cập nhật loại sản phẩm thất bại." }, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch
            {
                return Json(new { success = false, message = "Cập nhật loại sản phẩm thất bại." }, JsonRequestBehavior.AllowGet);
            }

        }
        [HttpPost]
        [ActionName("xoa-loai-san-pham")]
        [HasAdminCredential(Role = 2)]
        public ActionResult DeleteProductCategory(string id)
        {
            try
            {
                if (WorkerClass.Ins.ProductCategoryRepo.DeleteProductCategory(int.Parse(id)))
                {
                    WorkerClass.Ins.Save();
                    return Json(new { success = true, message = "Xóa loại sản phẩm thành công." }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { success = false, message = "Xóa loại sản phẩm thất bại." }, JsonRequestBehavior.AllowGet);
                }
            }
            catch
            {
                return Json(new { success = false, message = "Xóa loại sản phẩm thất bại." }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        [ActionName("them-loai-san-pham")]
        [HasAdminCredential(Role = 2)]
        public ActionResult InsertProductCategory(string productcategoryname, string status)
        {
            try
            {
                if (string.IsNullOrEmpty(productcategoryname))
                {
                    return Json(new { success = false, message = "Vui lòng nhập đầy đủ thông tin!" }, JsonRequestBehavior.AllowGet);
                }
                if (WorkerClass.Ins.ProductCategoryRepo.GetProductCategoryByName(productcategoryname) != null)
                {
                    return Json(new { success = false, message = "Loại sản phẩm này đã tồn tại." }, JsonRequestBehavior.AllowGet);
                }
                productcategoryname = string.IsNullOrEmpty(productcategoryname.Trim()) ? null : productcategoryname.Trim();
                bool statusBool = bool.Parse(status);
                if (WorkerClass.Ins.ProductCategoryRepo.InsertProductCategory(productcategoryname, (Session["UserCurrent"] as EmployeeAccount).Email, statusBool))
                {
                    WorkerClass.Ins.Save();
                    return Json(new { success = true, productCategory = WorkerClass.Ins.ProductCategoryRepo.GetNewProductCategory(), message = "Thêm loại sản phẩm thành công." }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { success = false, message = "Thêm loại sản phẩm thất bại." }, JsonRequestBehavior.AllowGet);
                }
            }
            catch
            {
                return Json(new { success = false, message = "Thêm loại sản phẩm thất bại." }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}