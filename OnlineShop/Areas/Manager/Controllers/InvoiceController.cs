using Model;
using Model.EF;
using OnlineShop.Areas.Manager.Service;
using System;
using System.Configuration;
using System.Web.Mvc;

namespace OnlineShop.Areas.Manager.Controllers
{
    public class InvoiceController : Controller
    {
        // GET: Manager/Invoice
        [HttpGet]
        [ActionName("xem-hoa-don")]
        [HasAdminCredential(Role = 2)]
        public ActionResult GetInvoiceById(string id)
        {
            try
            {
                Guid invoiceId = new Guid(id);
                var result = WorkerClass.Ins.InvoiceRepo.GetInvoiceById(invoiceId);

                var invoice = new
                {
                    result.InvoiceID,
                    result.ProductOrderID,
                    ExportDate = result.ExportDate.ToString("dd/MM/yyyy"),
                    EmployeeEmail = result.EmployeeAccount.Email,
                    result.CustomerConfirm,
                    result.Status,
                    NameOrderer = (result.ProductOrder.UserAccount.FirstName + " " + result.ProductOrder.UserAccount.LastName).Trim(),
                    PhoneOrderer = result.ProductOrder.UserAccount.Phone,
                    EmailOrderer = result.ProductOrder.UserAccount.Email,
                    AddressOrderer = result.ProductOrder.UserAccount.Address,
                    result.TotalPayment,
                    result.ProductOrder.Payment.PaymentName,
                    result.ProductOrder.PaymentOnlineID
                };
                return Json(new { success = true, invoice }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { success = false, message = "Lỗi không xem được hóa đơn." }, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPost]
        [ActionName("huy-hoa-don")]
        [HasAdminCredential(Role = 2)]
        public ActionResult CancelInvoice(string id)
        {
            try
            {
                Guid invoiceId = new Guid(id);
                if (WorkerClass.Ins.InvoiceRepo.CancelInvoice(invoiceId, (Session["UserCurrent"] as EmployeeAccount).EmployeeID))
                {
                    var productOrderId = WorkerClass.Ins.InvoiceRepo.GetInvoiceById(invoiceId).ProductOrderID;
                    var listProductOrderDetail = WorkerClass.Ins.ProductOrderDetailRepo.GetProductOrderDetailById(productOrderId);
                    if (WorkerClass.Ins.StockRepo.UpdateStockAfterCancelInvoice(listProductOrderDetail))
                    {
                        WorkerClass.Ins.Save();
                        var invoice = WorkerClass.Ins.InvoiceRepo.GetInvoiceById(invoiceId);
                        string content = "Mã hóa đơn: " + invoice.InvoiceID + "<br/>" +
                                         "Thanh toán cho đơn hàng: " + invoice.ProductOrderID + "<br/>" +
                                         "Thông báo: Đơn hàng này đã bị hủy<br/>" +
                                         "Nếu quý khách thanh toán bằng hình thức trực tuyến vui lòng chờ nhận lại tiền đã thanh toán trong vòng 72 giờ kể từ thời điểm nhận được thông báo từ cửa hàng.<br/>Cảm ơn quý khách đã đồng hành cùng cửa hàng!";
                        string emailTo = invoice.ProductOrder.UserAccount.Email;
                        string subject = "HỦY ĐƠN ĐẶT HÀNG - YOLO Shop";
                        string body = string.Format("Bạn vừa nhận được liên hệ từ: <b>YOLO Shop</b><br/>Email: {0}<br/>Nội dung:<br/>{1}<br/>Mọi thắc mắc xin liên hệ qua link facebook: <a href= \" https://www.facebook.com/kunn.ngoc.5 \">https://www.facebook.com/kunn.ngoc.5</a><br/>", ConfigurationManager.AppSettings["smtpUserName"].ToString(), content);
                        EmailService.Send(emailTo, subject, body);
                        return Json(new { success = true, message = "Hủy hóa đơn thành công.", employeeEmail = (Session["UserCurrent"] as EmployeeAccount).Email }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { success = false, message = "Hủy hóa đơn thất bại." }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json(new { success = false, message = "Hủy hóa đơn thất bại." }, JsonRequestBehavior.AllowGet);
                }
            }
            catch
            {
                return Json(new { success = false, message = "Hủy hóa đơn thất bại." }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}