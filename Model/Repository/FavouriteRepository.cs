using Model.EF;
using System;
using System.Linq;

namespace Model.Repository
{
    public class FavouriteRepository
    {
        private OnlineShopDataContext data;
        public FavouriteRepository(OnlineShopDataContext data)
        {
            this.data = data;
        }
        public bool InsertFavourite(Guid userID, string productID)
        {
            try
            {
                Favourite favourite = new Favourite() { UserID = userID, ProductID = productID };
                data.Favourites.Add(favourite);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool DeleteFavourite(Guid userID, string productID)
        {
            try
            {
                var favourite = data.Favourites.FirstOrDefault(x => x.UserID == userID && x.ProductID == productID);
                data.Favourites.Remove(favourite);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
