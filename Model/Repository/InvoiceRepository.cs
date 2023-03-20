using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Model.Repository
{
    public class InvoiceRepository
    {
        private OnlineShopDataContext data;
        public InvoiceRepository(OnlineShopDataContext data)
        {
            this.data = data;
        }
        public List<Invoice> GetAllInvoiceSucceededByDateTime(int month, int year)
        {
            try
            {
                return data.Invoices.Where(x => x.Status == true && x.CustomerConfirm == true && x.ExportDate.Month == month && x.ExportDate.Year == year).ToList();
            }
            catch
            {
                return null;
            }
        }
        public bool InsertInvoice(Guid productorderid, decimal totalpayment, Guid employeeid)
        {
            try
            {
                Invoice invoice = new Invoice() { ExportDate = DateTime.Now, TotalPayment = totalpayment, CustomerConfirm = false, ProductOrderID = productorderid, EmployeeID = employeeid, Status = true };
                data.Invoices.Add(invoice);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public Invoice GetInvoiceById(Guid invoiceId)
        {
            return data.Invoices.FirstOrDefault(x => x.InvoiceID == invoiceId);
        }
        public Invoice GetInvoiceByProductOrderId(Guid productOrderID)
        {
            return data.Invoices.FirstOrDefault(x => x.ProductOrderID == productOrderID);
        }
        public List<Invoice> GetAllInvoice()
        {
            return data.Invoices.ToList();
        }
        public bool CancelInvoice(Guid invoiceid, Guid employeeid)
        {
            try
            {
                var invoice = data.Invoices.FirstOrDefault(x => x.InvoiceID == invoiceid);
                invoice.CustomerConfirm = false;
                invoice.EmployeeID = employeeid;
                invoice.Status = false;
                return true;
            }
            catch
            {
                return false;
            }
        }
        public List<Invoice> GetAllMonthlyRevenue(int year)
        {
            try
            {
                return data.Invoices.Where(x => x.Status == true && x.CustomerConfirm == true && x.ExportDate.Year == year).ToList();
            }
            catch
            {
                return null;
            }
        }
        public Invoice GetFirstInvoiceInYear(int year)
        {
            return data.Invoices.OrderBy(x => x.ExportDate).FirstOrDefault();
        }
        public int CountInvoiceCanceled()
        {
            return data.Invoices.Where(x => x.Status == false).Count();
        }
        public int CountInvoiceDelivering()
        {
            return data.Invoices.Where(x => x.Status == true && x.CustomerConfirm == false).Count();
        }
        public int CountInvoiceSucceeded()
        {
            return data.Invoices.Where(x => x.Status == true && x.CustomerConfirm == true).Count();
        }
        public bool ConfirmProductReceived(Guid invoiceid)
        {
            try
            {
                var invoice = data.Invoices.FirstOrDefault(x => x.InvoiceID == invoiceid);
                invoice.CustomerConfirm = true;
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
