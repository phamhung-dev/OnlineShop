using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Model.Repository
{
    public class ProductOrderRepository
    {
        private OnlineShopDataContext data;
        public ProductOrderRepository(OnlineShopDataContext data)
        {
            this.data = data;
        }
        public List<ProductOrder> GetAllProductOrder()
        {
            return data.ProductOrders.ToList();
        }
        public List<ProductOrder> GetHistoryOrder(Guid userID)
        {
            return data.ProductOrders.Where(x => x.UserID == userID).ToList();
        }
        public ProductOrder GetProductOrderById(Guid productOrderID)
        {
            return data.ProductOrders.FirstOrDefault(x => x.ProductOrderID == productOrderID);
        }
        public int CountNewOrder()
        {
            return data.ProductOrders.Where(x => x.Status == false).Count();
        }
        public bool Browsing(Guid productorderid, DateTime shipdate)
        {
            try
            {
                var productOrder = data.ProductOrders.FirstOrDefault(x => x.ProductOrderID == productorderid);
                productOrder.ShipDate = shipdate;
                productOrder.Status = true;
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool DeleteProductOrder(Guid id)
        {
            try
            {
                var productOrderDelete = data.ProductOrders.FirstOrDefault(x => x.ProductOrderID == id);
                data.ProductOrders.Remove(productOrderDelete);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool CheckProductOrderIdExist(Guid productOrderID)
        {
            try
            {
                return data.ProductOrders.FirstOrDefault(x => x.ProductOrderID == productOrderID) == null ? false : true;
            }
            catch
            {
                return true;
            }
        }
        public bool InsertProductOrder(ProductOrder productOrder)
        {
            try
            {
                data.ProductOrders.Add(productOrder);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
