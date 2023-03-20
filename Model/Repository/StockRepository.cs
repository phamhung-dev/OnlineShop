using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Model.Repository
{
    public class StockRepository
    {
        private OnlineShopDataContext data;
        public StockRepository(OnlineShopDataContext data)
        {
            this.data = data;
        }
        public List<Stock> GetAllProductInStock()
        {
            return data.Stocks.ToList();
        }
        public Stock GetProductInStockById(string productid, string sizeid, string colorid)
        {
            return data.Stocks.FirstOrDefault(x => x.ProductID == productid && x.SizeID == sizeid && x.ColorID == colorid);
        }
        public bool ChangeStatusStock(string productid, string sizeid, string colorid, string updateby)
        {
            var productUpdate = data.Stocks.FirstOrDefault(x => x.ProductID == productid && x.SizeID == sizeid && x.ColorID == colorid);
            productUpdate.Status = !productUpdate.Status;
            productUpdate.UpdateAt = DateTime.Now;
            productUpdate.UpdateBy = updateby;
            return productUpdate.Status;
        }
        public bool Import(string productid, string sizeid, string colorid, int quantity, string updateby)
        {
            try
            {
                var productUpdate = data.Stocks.FirstOrDefault(x => x.ProductID == productid && x.SizeID == sizeid && x.ColorID == colorid);
                productUpdate.Quantity += quantity;
                productUpdate.UpdateAt = DateTime.Now;
                productUpdate.UpdateBy = updateby;
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool Export(string productid, string sizeid, string colorid, int quantity, string updateby)
        {
            try
            {
                var productUpdate = data.Stocks.FirstOrDefault(x => x.ProductID == productid && x.SizeID == sizeid && x.ColorID == colorid);
                productUpdate.Quantity -= quantity;
                productUpdate.UpdateAt = DateTime.Now;
                productUpdate.UpdateBy = updateby;
                return true;
            }
            catch
            {
                return false;
            }
        }
        public int CheckQuantityInStock(string productid, string sizeid, string colorid)
        {
            try
            {
                var product = data.Stocks.FirstOrDefault(x => x.ProductID == productid && x.SizeID == sizeid && x.ColorID == colorid);
                if(product == null)
                {
                    return 0;
                }
                return product.Quantity;
            }
            catch
            {
                return 0;
            }
        }
        public bool InsertStock(string productid, string sizeid, string colorid, string updateby)
        {
            try
            {
                Stock stock = new Stock() { ProductID = productid, SizeID = sizeid, ColorID = colorid, Quantity = 0, CreateAt = DateTime.Now, UpdateAt = DateTime.Now, UpdateBy = updateby, Status = false };
                data.Stocks.Add(stock);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool UpdateStockAfterSellProduct(List<ProductOrderDetail> listProductOrderDetail)
        {
            try
            {
                if(listProductOrderDetail == null)
                {
                    return false;
                }
                else
                {
                    foreach (var item in listProductOrderDetail)
                    {
                        data.Stocks.FirstOrDefault(x => x.ProductID == item.ProductID && x.SizeID == item.SizeID && x.ColorID == item.ColorID).Quantity -= item.Quantity;
                    }
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }
        public bool UpdateStockAfterCancelInvoice(List<ProductOrderDetail> listProductOrderDetail)
        {
            try
            {
                if (listProductOrderDetail == null)
                {
                    return false;
                }
                else
                {
                    foreach (var item in listProductOrderDetail)
                    {
                        data.Stocks.FirstOrDefault(x => x.ProductID == item.ProductID && x.SizeID == item.SizeID && x.ColorID == item.ColorID).Quantity += item.Quantity;
                    }
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }
        public int TotalProductInStock()
        {
            return data.Stocks.Count() <= 0 ? 0 : data.Stocks.Sum(x => x.Quantity);
        }
        public int TotalProductInventoryInStock()
        {
            return data.Stocks.Count() <= 0 ? 0 : data.Stocks.Where(x => x.Status == true).Sum(x => x.Quantity);
        }
    }
}
