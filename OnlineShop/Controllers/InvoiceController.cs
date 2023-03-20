using Model;
using System;
using System.Web.Mvc;

namespace OnlineShop.Controllers
{
    public class InvoiceController : Controller
    {
        // GET: Invoice
        [HttpGet]
        [ActionName("xem-hoa-don")]
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
    }
}