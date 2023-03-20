using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Model.Repository
{
    public class ProductRepository
    {
        private OnlineShopDataContext data;
        public ProductRepository(OnlineShopDataContext data)
        {
            this.data = data;
        }
        public List<Product> GetAllProduct()
        {
            return data.Products.ToList();
        }
        public List<Product> GetListNewProduct(int amount)
        {
            return data.Products.Where(x => x.Status == true).OrderByDescending(x => x.CreateAt).Take(amount).ToList();
        }
        public List<Product> GetListDiscountProduct(int amount)
        {
            return data.Products.Where(x => x.Status == true).OrderByDescending(x => x.DiscountPercent).Take(amount).ToList();
        }
        public List<Product> GetListFeaturedProduct(int amount)
        {
            return data.Products.Where(x => x.Status == true && x.ShowOnHome == true).Take(amount).ToList();
        }
        public int CountProduct()
        {
            return data.Products.Count();
        }
        public List<Product> GetAllProductSelling()
        {
            return data.Products.Where(x => x.Status == true).ToList();
        }
        public Product GetProductById(string id)
        {
            return data.Products.FirstOrDefault(x => x.ProductID == id);
        }
        public bool ChangeStatusProduct(string id, string updateby)
        {
            var productUpdate = data.Products.FirstOrDefault(x => x.ProductID == id);
            productUpdate.Status = !productUpdate.Status;
            productUpdate.UpdateAt = DateTime.Now;
            productUpdate.UpdateBy = updateby;
            return productUpdate.Status;
        }
        public bool UpdateProduct(string productid, string productname, string metatitle, string supplierid, int productcategoryid, int ageid, bool gender, string description, int warranty, decimal unitimportprice, decimal unitsellprice, double discountpercent, bool showonhome, string updateby, bool status)
        {
            try
            {
                var productUpdate = data.Products.FirstOrDefault(x => x.ProductID == productid);
                productUpdate.ProductName = productname;
                productUpdate.MetaTitle = metatitle;
                productUpdate.SupplierID = supplierid;
                productUpdate.ProductCategoryID = productcategoryid;
                productUpdate.AgeID = ageid;
                productUpdate.Gender = gender;
                productUpdate.Description = description;
                productUpdate.Warranty = warranty;
                productUpdate.UnitImportPrice = unitimportprice;
                productUpdate.UnitSellPrice = unitsellprice;
                productUpdate.DiscountPercent = discountpercent;
                productUpdate.ShowOnHome = showonhome;
                productUpdate.Status = status;
                productUpdate.UpdateAt = DateTime.Now;
                productUpdate.UpdateBy = updateby;
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool InsertProduct(string productid, string productname, string metatitle, string supplierid, int productcategoryid, int ageid, bool gender, string description, int warranty, decimal unitimportprice, decimal unitsellprice, double discountpercent, bool showonhome, string updateby, bool status)
        {
            try
            {
                Product product = new Product() { 
                    ProductID = productid,
                    ProductName = productname,
                    MetaTitle = metatitle,
                    SupplierID = supplierid,
                    ProductCategoryID = productcategoryid,
                    AgeID = ageid, 
                    Gender = gender,
                    Description = description,
                    Warranty = warranty,
                    ScoreRating = 0,
                    UnitImportPrice = unitimportprice, 
                    UnitSellPrice = unitsellprice, 
                    DiscountPercent = discountpercent, 
                    ShowOnHome =  showonhome, 
                    CreateAt = DateTime.Now, 
                    UpdateAt = DateTime.Now, 
                    UpdateBy = updateby, 
                    Status = status
                };
                data.Products.Add(product);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool UpdateScoreRating(string productID, double scoreRating)
        {
            try
            {
                var product = data.Products.FirstOrDefault(x => x.ProductID == productID);
                product.ScoreRating = scoreRating;
                return true;
            }
            catch
            {
                return false;
            }
        }
        public double GetScoreRatingById(string productID)
        {
            return data.Products.FirstOrDefault(x => x.ProductID == productID).ScoreRating;
        }
    }
}
