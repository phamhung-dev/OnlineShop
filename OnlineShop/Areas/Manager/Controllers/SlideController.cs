using Model;
using Model.EF;
using OnlineShop.Areas.Manager.Service;
using System;
using System.IO;
using System.Web;
using System.Web.Mvc;

namespace OnlineShop.Areas.Manager.Controllers
{
    public class SlideController : Controller
    {
        // GET: Manager/Slide
        [HttpPost]
        [ActionName("xoa-slide")]
        [HasAdminCredential(Role = 2)]
        public ActionResult DeleteSlide(string id)
        {
            try
            {
                int idInt = int.Parse(id);
                if (WorkerClass.Ins.SlideRepo.DeleteSlide(idInt))
                {
                    WorkerClass.Ins.Save();
                    return Json(new { success = true, message = "Xóa slide thành công." }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { success = false, message = "Xóa slide thất bại." }, JsonRequestBehavior.AllowGet);
                }
            }
            catch
            {
                return Json(new { success = false, message = "Xóa slide thất bại." }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        [ActionName("thay-doi-trang-thai")]
        [HasAdminCredential(Role = 2)]
        public ActionResult ChangeStatusSlide(string id)
        {
            try
            {
                int idInt = int.Parse(id);
                if (WorkerClass.Ins.SlideRepo.ChangeStatusSlide(idInt, (Session["UserCurrent"] as EmployeeAccount).Email))
                {
                    WorkerClass.Ins.Save();
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
        [ActionName("thay-doi-vi-tri-xuat-hien")]
        [HasAdminCredential(Role = 2)]
        public ActionResult ChangePositionAppearSlide(string id, string position)
        {
            try
            {
                int idInt = int.Parse(id);
                int positionInt = int.Parse(position);
                if (positionInt < 1)
                {
                    return Json(new { success = false, message = "Vị trí xuất hiện phải lớn hơn hoặc bằng 1." }, JsonRequestBehavior.AllowGet);
                }
                if (!WorkerClass.Ins.SlideRepo.IsExistPositionAppear(positionInt))
                {
                    if (WorkerClass.Ins.SlideRepo.ChangePositionAppearSlide(idInt, positionInt, (Session["UserCurrent"] as EmployeeAccount).Email))
                    {
                        WorkerClass.Ins.Save();
                        return Json(new { success = true, message = "Cập nhật thông tin thành công." }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { success = false, message = "Cập nhật thông tin thất bại." }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json(new { success = false, message = "Vị trí xuất hiện đã bị trùng." }, JsonRequestBehavior.AllowGet);
                }
            }
            catch
            {
                return Json(new { success = false, message = "Cập nhật thông tin thất bại." }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [ActionName("them-slide")]
        [HasAdminCredential(Role = 2)]
        public ActionResult InsertSlide(HttpPostedFileBase file)
        {
            try
            {
                if (file == null)
                {
                    return Json(new { success = false, message = "Lưu ảnh thất bại." }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    file.SaveAs(Server.MapPath("~/Asset/Upload/" + file.FileName));
                    var path = "~/Asset/Upload/" + file.FileName;
                    if (WorkerClass.Ins.SlideRepo.InsertSlide(converImagetoByte(path), (Session["UserCurrent"] as EmployeeAccount).Email))
                    {
                        WorkerClass.Ins.Save();
                        Slide newSlide = WorkerClass.Ins.SlideRepo.GetNewSlide();
                        DeleteFilesInFolder("~/Asset/Upload");
                        var slide = new
                        {
                            newSlide.SlideID,
                            Photo = Convert.ToBase64String(newSlide.Photo),
                            newSlide.CreateAt,
                            newSlide.UpdateAt,
                            newSlide.UpdateBy,
                            newSlide.Status,
                            newSlide.PositionAppear
                        };
                        return Json(new { success = true, message = "Lưu ảnh thành công.", slide = slide }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { success = false, message = "Lưu ảnh thất bại." }, JsonRequestBehavior.AllowGet);
                    }

                }
            }
            catch
            {
                return Json(new { success = false, message = "Lưu ảnh thất bại." }, JsonRequestBehavior.AllowGet);
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