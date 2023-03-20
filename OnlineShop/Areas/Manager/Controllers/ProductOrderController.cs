using Model;
using Model.EF;
using OnlineShop.Areas.Manager.Service;
using System;
using System.Configuration;
using System.Web.Mvc;

namespace OnlineShop.Areas.Manager.Controllers
{
    public class ProductOrderController : Controller
    {
        // GET: Manager/ProductOrder
        [HttpPost]
        [ActionName("duyet-phieu-dat-hang")]
        [HasAdminCredential(Role = 2)]
        public ActionResult Browsing(string productorderid, string shipdate)
        {
            try
            {
                if (string.IsNullOrEmpty(shipdate))
                {
                    return Json(new { success = false, message = "Vui lòng chọn ngày giao hàng!" }, JsonRequestBehavior.AllowGet);
                }
                Guid productorderidGuid = new Guid(productorderid);
                DateTime shipdateDateTime = DateTime.Parse(shipdate);
                if(shipdateDateTime < DateTime.Now)
                {
                    return Json(new { success = false, message = "Vui lòng chọn ngày giao hàng phù hợp." }, JsonRequestBehavior.AllowGet);
                }
                var listShoppingCart = WorkerClass.Ins.ProductOrderDetailRepo.GetProductOrderDetailById(productorderidGuid);
                foreach(var item in listShoppingCart)
                {
                    int quantityInStock = WorkerClass.Ins.StockRepo.CheckQuantityInStock(item.ProductID, item.SizeID, item.ColorID);
                    if (item.Quantity > quantityInStock)
                    {
                        return Json(new { success = false, message = "Sản phẩm " + item.ProductID + " " + item.Stock.Size.SizeName + " " + item.Stock.Color.ColorName + " chỉ còn " + quantityInStock.ToString() + " sản phẩm." }, JsonRequestBehavior.AllowGet);
                    }
                }
                if (WorkerClass.Ins.ProductOrderRepo.Browsing(productorderidGuid, shipdateDateTime))
                {
                    decimal totalPayment = WorkerClass.Ins.ProductOrderDetailRepo.CalculateTotalPayment(productorderidGuid);
                    if( totalPayment >= 0)
                    {
                        if(WorkerClass.Ins.InvoiceRepo.InsertInvoice(productorderidGuid, totalPayment, (Session["UserCurrent"] as EmployeeAccount).EmployeeID) && WorkerClass.Ins.StockRepo.UpdateStockAfterSellProduct(WorkerClass.Ins.ProductOrderDetailRepo.GetProductOrderDetailById(productorderidGuid)))
                        {
                            WorkerClass.Ins.Save();
                            var invoice = WorkerClass.Ins.InvoiceRepo.GetInvoiceByProductOrderId(productorderidGuid);
                            string content = "Mã hóa đơn: " + invoice.InvoiceID + "<br/>" +
                                             "Ngày xuất: " + invoice.ExportDate.ToString("dd/MM/yyyy hh:mm:ss tt") + "<br/>" +
                                             "Thanh toán cho đơn hàng: " + invoice.ProductOrderID + "<br/>" +
                                             "Họ tên KH: " + (invoice.ProductOrder.UserAccount.FirstName + " " + invoice.ProductOrder.UserAccount.LastName).Trim() + "<br/>" +
                                             "Điện thoại: " + invoice.ProductOrder.UserAccount.Phone + "<br/>" +
                                             "Email: " + invoice.ProductOrder.UserAccount.Email + "<br/>" +
                                             "Địa chỉ: " + invoice.ProductOrder.UserAccount.Address + "<br/>" +
                                             "Ngày giao: " + invoice.ProductOrder.ShipDate.Value.ToString("dd/MM/yyyy") + "<br/><br/>" +
                                             "Sản phẩm đặt mua: <br/>";
                            var listProductOrderDetail = WorkerClass.Ins.ProductOrderDetailRepo.GetProductOrderDetailById(productorderidGuid);
                            foreach (var item in listProductOrderDetail)
                            {
                                content += "Mã giày: " + item.ProductID + "<br/>" +
                                           "Size: " + item.Stock.Size.SizeName + "<br/>" +
                                           "Màu sắc: " + item.Stock.Color.ColorName + "<br/>" +
                                           "Giá: " + String.Format("{0:0,0}", item.Price) + " VNĐ <br/>" +
                                           "Số lượng: " + item.Quantity + "<br/><br/>";
                            }
                            content += "Phương thức thanh toán: " + invoice.ProductOrder.Payment.PaymentName + "<br/>" +
                                       (invoice.ProductOrder.PaymentOnlineID == null ? "Tổng số tiền cần thanh toán: " + String.Format("{0:0,0}", invoice.TotalPayment) + " VNĐ <br/>Quý khách vui lòng thanh toán đủ số tiền khi nhận được hàng." : "Mã thanh toán: " + invoice.ProductOrder.PaymentOnlineID + "<br/>Đã thanh toán đủ số tiền: " + String.Format("{0:0,0}", invoice.TotalPayment) + " VNĐ") +
                                       "<br/>Cảm ơn quý khách đã đồng hành cùng cửa hàng!";

                            string emailTo = invoice.ProductOrder.UserAccount.Email;
                            string subject = "HOÁ ĐƠN ĐẶT HÀNG - YOLO Shop";
                            string body = string.Format("Bạn vừa nhận được liên hệ từ: <b>YOLO Shop</b><br/>Email: {0}<br/>Nội dung:<br/>{1}<br/>Mọi thắc mắc xin liên hệ qua link facebook: <a href= \" https://www.facebook.com/kunn.ngoc.5 \">https://www.facebook.com/kunn.ngoc.5</a><br/>", ConfigurationManager.AppSettings["smtpUserName"].ToString(), content);
                            EmailService.Send(emailTo, subject, body);
                            return Json(new { success = true, message = "Xuất hóa đơn thành công." }, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            return Json(new { success = false, message = "Xuất hóa đơn thất bại." }, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        return Json(new { success = false, message = "Xuất hóa đơn thất bại." }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json(new { success = false, message = "Thêm kích thước thất bại." }, JsonRequestBehavior.AllowGet);
                }
            }
            catch
            {
                return Json(new { success = false, message = "Xuất hóa đơn thất bại." }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        [ActionName("xoa-phieu-dat-hang")]
        [HasAdminCredential(Role = 2)]
        public ActionResult DeleteProductOrder(string id)
        {
            try
            {
                Guid productOrderId = new Guid(id);             
                if (WorkerClass.Ins.ProductOrderDetailRepo.DeleteProductOrderDetail(productOrderId))
                {
                    if (WorkerClass.Ins.ProductOrderRepo.DeleteProductOrder(productOrderId))
                    {
                        WorkerClass.Ins.Save();
                        return Json(new { success = true, message = "Xóa phiếu đặt hàng thành công." }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { success = false, message = "Xóa phiếu đặt hàng thất bại." }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json(new { success = false, message = "Xóa phiếu đặt hàng thất bại." }, JsonRequestBehavior.AllowGet);
                }
            }
            catch
            {
                return Json(new { success = false, message = "Xóa phiếu đặt hàng thất bại." }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}