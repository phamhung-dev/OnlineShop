using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Model.Repository
{
    public class ShoppingCartRepository
    {
        private OnlineShopDataContext data;
        public ShoppingCartRepository(OnlineShopDataContext data)
        {
            this.data = data;
        }
        public List<ShoppingCart> GetShoppingCartByUserID(Guid userID)
        {
            return data.ShoppingCarts.Where(x => x.UserID == userID).ToList();
        }
        public bool InsertShoppingCart(ShoppingCart shoppingCart)
        {
            try
            {
                data.ShoppingCarts.Add(shoppingCart);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool DeleteShoppingCart(Guid userID, string productID, string sizeID, string colorID)
        {
            try
            {
                var product = data.ShoppingCarts.FirstOrDefault(x => x.UserID == userID && x.ProductID == productID && x.SizeID == sizeID && x.ColorID == colorID);
                data.ShoppingCarts.Remove(product);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool UpdateShoppingCart(Guid userID, string productID, string sizeID, string colorID, int quantity)
        {
            try
            {
                var product = data.ShoppingCarts.FirstOrDefault(x => x.UserID == userID && x.ProductID == productID && x.SizeID == sizeID && x.ColorID == colorID);
                product.Quantity = quantity;
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool DeleteAllShoppingCart(Guid userID)
        {
            try
            {
                data.ShoppingCarts.RemoveRange(data.ShoppingCarts.Where(x => x.UserID == userID));
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
