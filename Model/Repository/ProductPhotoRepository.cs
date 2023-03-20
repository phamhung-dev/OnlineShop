using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Model.Repository
{
    public class ProductPhotoRepository
    {
        private OnlineShopDataContext data;
        public ProductPhotoRepository(OnlineShopDataContext data)
        {
            this.data = data;
        }
        public List<ProductPhoto> GetProductPhotoesByProductId(string id)
        {
            return data.ProductPhotoes.Where(x => x.ProductID == id).ToList();
        }

        public bool InsertProductPhoto(string productid, byte[] photo, string updateBy)
        {
            try
            {
                ProductPhoto productPhoto = new ProductPhoto() { ProductID = productid, Photo = photo, CreateAt = DateTime.Now, UpdateAt = DateTime.Now, UpdateBy = updateBy, Status = true };
                data.ProductPhotoes.Add(productPhoto);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool UpdateProductPhoto(string productid, List<byte[]> arrImage, string updateby)
        {
            try
            {
                data.ProductPhotoes.RemoveRange(data.ProductPhotoes.Where(x => x.ProductID == productid));
                for (int i = 0; i < arrImage.Count(); i++)
                {
                    if (!InsertProductPhoto(productid, arrImage[i], updateby))
                    {
                        return false;
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
