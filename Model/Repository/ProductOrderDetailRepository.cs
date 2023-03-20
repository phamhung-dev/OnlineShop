using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Model.Repository
{
    public class ProductOrderDetailRepository
    {
        private OnlineShopDataContext data;
        public ProductOrderDetailRepository(OnlineShopDataContext data)
        {
            this.data = data;
        }
        public decimal CalculateTotalPayment(Guid productorderid)
        {
            try
            {
                return data.ProductOrderDetails.Where(x => x.ProductOrderID == productorderid).Sum(x => x.Quantity * x.Price);
            }
            catch
            {
                return -1;
            }
        }
        public List<ProductOrderDetail> GetProductOrderDetailById(Guid? productorderid)
        {
            try
            {
                if(productorderid == null)
                {
                    return null;
                }
                return data.ProductOrderDetails.Where(x => x.ProductOrderID == productorderid).ToList();
            }
            catch
            {
                return null;
            }
        }
        public bool DeleteProductOrderDetail(Guid productOrderId)
        {
            try
            {
                data.ProductOrderDetails.RemoveRange(data.ProductOrderDetails.Where(x => x.ProductOrderID == productOrderId));
                return true;
            }
            catch
            {
                return false;
            }
        }
        public int TotalProductSold()
        {
             return (from p in data.ProductOrderDetails
                     join i in data.Invoices on p.ProductOrderID equals i.ProductOrderID
                     where i.Status == true
                     select (int?)p.Quantity).Sum() ?? 0;
        }
        public List<ProductOrderDetail> GetAllProductOrderDetailSold()
        {
            return (from p in data.ProductOrderDetails
                    join i in data.Invoices on p.ProductOrderID equals i.ProductOrderID
                    where i.Status == true
                    select p).ToList();
        }
        public List<ProductOrderDetail> GetAllProductOrderDetailByDate(DateTime date)
        {
            return (from p in data.ProductOrderDetails
                    join i in data.Invoices on p.ProductOrderID equals i.ProductOrderID
                    where System.Data.Entity.DbFunctions.TruncateTime(i.ExportDate) == date.Date && i.Status == true
                    select p).ToList();
        }
        public bool InsertProductOrderDetail(ProductOrderDetail productOrderDetail)
        {
            try
            {
                data.ProductOrderDetails.Add(productOrderDetail);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public List<ProductOrderDetail> GetListProductOrderDetailBought(Guid userID)
        {
            try
            {
                return data.ProductOrderDetails.Where(x => x.ProductOrder.UserID == userID && (x.ProductOrder.Invoices.FirstOrDefault() == null ? true : x.ProductOrder.Invoices.FirstOrDefault().Status == true)).ToList();
            }
            catch
            {
                return null;
            }
        }
    }
}
