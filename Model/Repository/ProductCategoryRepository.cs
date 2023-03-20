using Model.EF;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace Model.Repository
{
    public class ProductCategoryRepository
    {
        private OnlineShopDataContext data;
        public ProductCategoryRepository(OnlineShopDataContext data)
        {
            this.data = data;
        }
        public List<ProductCategory> GetAllProductCategory()
        {
            try
            {
                var nullid = int.Parse(ConfigurationManager.AppSettings["IntNullID"].ToString());
                return data.ProductCategories.Where(x => x.ProductCategoryID != nullid).ToList();
            }
            catch
            {
                return null;
            }
        }
        public ProductCategory GetProductCategoryByName(string name)
        {
            return data.ProductCategories.FirstOrDefault(x => x.ProductCategoryName == name);
        }
        public ProductCategory GetNewProductCategory()
        {
            return data.ProductCategories.OrderByDescending(x =>x.ProductCategoryID).FirstOrDefault();
        }
        public List<ProductCategory> GetAllProductCategorySelling()
        {
            try
            {
                return data.ProductCategories.Where(x => x.Status == true).ToList();
            }
            catch
            {
                return null;
            }
        }
        public bool ChangeStatusProductCategory(int id, string updateby)
        {
            var productcategoryUpdate = data.ProductCategories.FirstOrDefault(x => x.ProductCategoryID == id);
            productcategoryUpdate.Status = !productcategoryUpdate.Status;
            productcategoryUpdate.UpdateAt = DateTime.Now;
            productcategoryUpdate.UpdateBy = updateby;
            return productcategoryUpdate.Status;
        }
        public bool UpdateProductCategory(int id, string productcategoryname, string updateby)
        {
            try
            {
                var productcategoryUpdate = data.ProductCategories.FirstOrDefault(x => x.ProductCategoryID == id);
                productcategoryUpdate.ProductCategoryName = productcategoryname;
                productcategoryUpdate.UpdateAt = DateTime.Now;
                productcategoryUpdate.UpdateBy = updateby;
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool DeleteProductCategory(int id)
        {
            try
            {
                var nullid = int.Parse(ConfigurationManager.AppSettings["IntNullID"].ToString());
                var productcategoryDelete = data.ProductCategories.FirstOrDefault(x => x.ProductCategoryID == id);
                data.Products.Where(x => x.ProductCategoryID == productcategoryDelete.ProductCategoryID).ToList().ForEach(x => x.ProductCategoryID = nullid);
                data.ProductCategories.Remove(productcategoryDelete);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool InsertProductCategory(string productcategoryname, string updateby, bool status)
        {
            try
            {
                ProductCategory productCategory = new ProductCategory() { ProductCategoryName = productcategoryname, CreateAt = DateTime.Now, UpdateAt = DateTime.Now, UpdateBy = updateby, Status = status };
                data.ProductCategories.Add(productCategory);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
