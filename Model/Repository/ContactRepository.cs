using Model.EF;
using System.Linq;

namespace Model.Repository
{
    public class ContactRepository
    {
        private OnlineShopDataContext data;
        public ContactRepository(OnlineShopDataContext data)
        {
            this.data = data;
        }
        public Contact GetContact()
        {
            return data.Contacts.FirstOrDefault();
        }
    }
}
