using Model;
using Model.EF;
using OnlineShop.Areas.Manager.Service;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineShop.Areas.Manager.Controllers
{
    public class ProductController : Controller
    {
        private static List<byte[]> arrImage = new List<byte[]>();
        private static List<byte[]> arrImageCur = new List<byte[]>();
        // GET: Manager/Product
        [HttpGet]
        [ActionName("xem-san-pham")]
        [HasAdminCredential(Role = 2)]
        public ActionResult GetProductById(string id)
        {
            arrImageCur.Clear();
            var productMain = WorkerClass.Ins.ProductRepo.GetProductById(id);
            var product = new
            {
                productMain.ProductID,
                productMain.ProductName,
                productMain.SupplierID,
                productMain.ProductCategoryID,
                productMain.AgeID,
                productMain.Gender,
                productMain.Description,
                productMain.Warranty,
                productMain.ScoreRating,
                productMain.UnitImportPrice,
                productMain.UnitSellPrice,
                productMain.DiscountPercent,
                productMain.ShowOnHome,
                productMain.Status
            };
            var productPhoto = WorkerClass.Ins.ProductPhotoRepo.GetProductPhotoesByProductId(id).Select(x => x.Photo);
            foreach(var item in productPhoto)
            {
                arrImageCur.Add(item);
            }
            return Json(new { product, productPhoto }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [ActionName("thay-doi-trang-thai-san-pham")]
        [HasAdminCredential(Role = 2)]
        public ActionResult ChangeStatusProduct(string id)
        {
            try
            {
                bool status = WorkerClass.Ins.ProductRepo.ChangeStatusProduct(id, (Session["UserCurrent"] as EmployeeAccount).Email);
                WorkerClass.Ins.Save();
                return Json(new { success = true, status }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [ActionName("them-hinh-san-pham-chua-ton-tai")]
        [HasAdminCredential(Role = 2)]
        public ActionResult AddImageNonExist(HttpPostedFileBase file)
        {
            try
            {
                if (file == null)
                {
                    return Json(new { success = false, message = "Thêm ảnh thất bại." }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    file.SaveAs(Server.MapPath("~/Asset/Upload/" + file.FileName));
                    var path = "~/Asset/Upload/" + file.FileName;
                    arrImage.Add(converImagetoByte(path));
                    return Json(new { success = true, path }, JsonRequestBehavior.AllowGet);
                }
            }
            catch
            {
                return Json(new { success = false, message = "Thêm ảnh thất bại." }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        [ActionName("them-hinh-san-pham-da-ton-tai")]
        [HasAdminCredential(Role = 2)]
        public ActionResult AddImageExist(HttpPostedFileBase file)
        {
            try
            {
                if (file == null)
                {
                    return Json(new { success = false, message = "Thêm ảnh thất bại." }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    file.SaveAs(Server.MapPath("~/Asset/Upload/" + file.FileName));
                    var path = "~/Asset/Upload/" + file.FileName;
                    arrImageCur.Add(converImagetoByte(path));
                    return Json(new { success = true, path }, JsonRequestBehavior.AllowGet);
                }
            }
            catch
            {
                return Json(new { success = false, message = "Thêm ảnh thất bại." }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        [ActionName("xoa-hinh-san-pham-chua-ton-tai")]
        [HasAdminCredential(Role = 2)]
        public ActionResult RemoveImageNonExist(string file)
        {
            try
            {
                if (string.IsNullOrEmpty(file))
                {
                    return Json(new { success = false, message = "Xoá ảnh thất bại." }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    file = file.Replace("../../../", "~");
                    for (int i = 0; i < arrImage.Count(); i++) 
                    {
                        if (arrImage[i].SequenceEqual(converImagetoByte(file)))
                        {
                            arrImage.RemoveAt(i);
                            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
                        }
                    }
                    return Json(new { success = false, message = "Xóa ảnh thất bại." }, JsonRequestBehavior.AllowGet);
                }
            }
            catch
            {
                return Json(new { success = false, message = "Xóa ảnh thất bại." }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        [ActionName("xoa-hinh-san-pham-da-ton-tai")]
        [HasAdminCredential(Role = 2)]
        public ActionResult RemoveImageExist(string file)
        {
            try
            {
                if (string.IsNullOrEmpty(file))
                {
                    return Json(new { success = false, message = "Xoá ảnh thất bại." }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    byte[] img = null ;
                    if (file.Contains("data:image/png;base64,"))
                    {
                        img = Convert.FromBase64String(file.Replace("data:image/png;base64, ", ""));
                    }
                    else
                    {                   
                        img = converImagetoByte(file.Replace("../../../", "~"));
                    }
                    if(img != null)
                    {
                        for (int i = 0; i < arrImageCur.Count(); i++)
                        {
                            if (arrImageCur[i].SequenceEqual(img))
                            {
                                arrImageCur.RemoveAt(i);
                                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
                            }
                        }
                    }
                    return Json(new { success = false, message = "Xóa ảnh thất bại." }, JsonRequestBehavior.AllowGet);
                }
            }
            catch
            {
                return Json(new { success = false, message = "Xóa ảnh thất bại." }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        [ActionName("them-san-pham")]
        [HasAdminCredential(Role = 2)]
        public ActionResult InsertProduct(string productid, string productname, string supplierid, string productcategoryid, string ageid, string gender, string description, string warranty, string unitimportprice, string unitsellprice, string discountpercent, string showonhome, string status)
        {
            try
            {
                if (string.IsNullOrEmpty(productid) || string.IsNullOrEmpty(productname) || string.IsNullOrEmpty(warranty) || string.IsNullOrEmpty(unitimportprice) || string.IsNullOrEmpty(unitsellprice) || string.IsNullOrEmpty(discountpercent))
                {
                    return Json(new { success = false, message = "Vui lòng nhập đầy đủ thông tin!" }, JsonRequestBehavior.AllowGet);
                }
                if (productid.Length > 20)
                {
                    return Json(new { success = false, message = "Mã sản phẩm tối đa 20 ký tự!" }, JsonRequestBehavior.AllowGet);
                }
                int warrantyInt = int.Parse(warranty);
                if (warrantyInt < 0)
                {
                    return Json(new { success = false, message = "Số tháng bảo hành phải >= 0!" }, JsonRequestBehavior.AllowGet);
                }
                decimal unitimportpriceDecimal = decimal.Parse(unitimportprice);
                if (unitimportpriceDecimal < 0)
                {
                    return Json(new { success = false, message = "Giá nhập phải >= 0!" }, JsonRequestBehavior.AllowGet);
                }
                decimal unitsellpriceDecimal = decimal.Parse(unitsellprice);
                if (unitsellpriceDecimal < 0)
                {
                    return Json(new { success = false, message = "Giá bán phải >= 0!" }, JsonRequestBehavior.AllowGet);
                }
                double discountpercentDouble = double.Parse(discountpercent);
                if (discountpercentDouble < 0)
                {
                    return Json(new { success = false, message = "Khuyến mãi phải >= 0%!" }, JsonRequestBehavior.AllowGet);
                }
                if (WorkerClass.Ins.ProductRepo.GetProductById(productid) != null)
                {
                    return Json(new { success = false, message = "Mã sản phẩm đã tồn tại!" }, JsonRequestBehavior.AllowGet);
                }
                productid = string.IsNullOrEmpty(productid.Trim()) ? null : productid.Trim();
                productname = string.IsNullOrEmpty(productname.Trim()) ? null : productname.Trim();
                string metatitle = DiacriticsService.RemoveDiacritics(productname.ToLower()).Replace(' ', '-');
                int productcategoryidInt = int.Parse(productcategoryid);
                int ageidInt = int.Parse(ageid);
                bool genderBool = bool.Parse(gender);
                description = string.IsNullOrEmpty(description) ? null : description.Trim();
                bool showonhomeBool = bool.Parse(showonhome);
                bool statusBool = bool.Parse(status);
                if (WorkerClass.Ins.ProductRepo.InsertProduct(productid, productname, metatitle, supplierid, productcategoryidInt, ageidInt, genderBool, description, warrantyInt, unitimportpriceDecimal, unitsellpriceDecimal, discountpercentDouble, showonhomeBool, (Session["UserCurrent"] as EmployeeAccount).Email, statusBool))
                {
                    for (int i = 0; i < arrImage.Count(); i++)
                    {
                        if (!WorkerClass.Ins.ProductPhotoRepo.InsertProductPhoto(productid, arrImage[i], (Session["UserCurrent"] as EmployeeAccount).Email))
                        {
                            return Json(new { success = false, message = "Thêm sản phẩm thất bại." }, JsonRequestBehavior.AllowGet);
                        }
                    }
                    WorkerClass.Ins.Save();
                    arrImage.Clear();
                    DeleteFilesInFolder("~/Asset/Upload");
                    return Json(new { success = true, message = "Thêm sản phẩm thành công." }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { success = false, message = "Thêm sản phẩm thất bại." }, JsonRequestBehavior.AllowGet);
                }
            }
            catch
            {
                return Json(new { success = false, message = "Thêm sản phẩm thất bại." }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [ActionName("huy-them-san-pham")]
        [HasAdminCredential(Role = 2)]
        public ActionResult CancelInsertProduct()
        {
            try
            {
                arrImage.Clear();
                DeleteFilesInFolder("~/Asset/Upload");
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { success = false, message = "Lỗi!" }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        [ActionName("cap-nhat-san-pham")]
        [HasAdminCredential(Role = 2)]
        public ActionResult UpdateProduct(string productid, string productname, string supplierid, string productcategoryid, string ageid, string gender, string description, string warranty, string unitimportprice, string unitsellprice, string discountpercent, string showonhome, string status)
        {
            try
            {
                if (string.IsNullOrEmpty(productname) || string.IsNullOrEmpty(warranty) || string.IsNullOrEmpty(unitimportprice) || string.IsNullOrEmpty(unitsellprice) || string.IsNullOrEmpty(discountpercent))
                {
                    return Json(new { success = false, message = "Vui lòng nhập đầy đủ thông tin!" }, JsonRequestBehavior.AllowGet);
                }
                int warrantyInt = int.Parse(warranty);
                if (warrantyInt < 0)
                {
                    return Json(new { success = false, message = "Số tháng bảo hành phải >= 0!" }, JsonRequestBehavior.AllowGet);
                }
                decimal unitimportpriceDecimal = decimal.Parse(unitimportprice);
                if (unitimportpriceDecimal < 0)
                {
                    return Json(new { success = false, message = "Giá nhập phải >= 0!" }, JsonRequestBehavior.AllowGet);
                }
                decimal unitsellpriceDecimal = decimal.Parse(unitsellprice);
                if (unitsellpriceDecimal < 0)
                {
                    return Json(new { success = false, message = "Giá bán phải >= 0!" }, JsonRequestBehavior.AllowGet);
                }
                double discountpercentDouble = double.Parse(discountpercent);
                if (discountpercentDouble < 0)
                {
                    return Json(new { success = false, message = "Khuyến mãi phải >= 0%!" }, JsonRequestBehavior.AllowGet);
                }
                productname = string.IsNullOrEmpty(productname.Trim()) ? null : productname.Trim();
                string metatitle = DiacriticsService.RemoveDiacritics(productname.ToLower()).Replace(' ', '-');
                int productcategoryidInt = int.Parse(productcategoryid);
                int ageidInt = int.Parse(ageid);
                bool genderBool = bool.Parse(gender);
                description = string.IsNullOrEmpty(description) ? null : description.Trim();
                bool showonhomeBool = bool.Parse(showonhome);
                bool statusBool = bool.Parse(status);
                if (WorkerClass.Ins.ProductRepo.UpdateProduct(productid, productname, metatitle, supplierid, productcategoryidInt, ageidInt, genderBool, description, warrantyInt, unitimportpriceDecimal, unitsellpriceDecimal, discountpercentDouble, showonhomeBool, (Session["UserCurrent"] as EmployeeAccount).Email, statusBool))
                {
                    if (!WorkerClass.Ins.ProductPhotoRepo.UpdateProductPhoto(productid, arrImageCur, (Session["UserCurrent"] as EmployeeAccount).Email))
                    {
                        return Json(new { success = false, message = "Cập nhật sản phẩm thất bại." }, JsonRequestBehavior.AllowGet);
                    }
                    WorkerClass.Ins.Save();
                    arrImageCur.Clear();
                    DeleteFilesInFolder("~/Asset/Upload");
                    return Json(new { success = true, message = "Cập nhật sản phẩm thành công." }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { success = false, message = "Cập nhật sản phẩm thất bại." }, JsonRequestBehavior.AllowGet);
                }
            }
            catch
            {
                return Json(new { success = false, message = "Cập nhật sản phẩm thất bại." }, JsonRequestBehavior.AllowGet);
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
    }
}