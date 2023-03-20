using Model.EF;
using System.Collections.Generic;
using System.Linq;
namespace Model.Repository
{
    public class PaymentRepository
    {
        private OnlineShopDataContext data;
        public PaymentRepository(OnlineShopDataContext data)
        {
            this.data = data;
        }
        public List<Payment> GetAllPayment()
        {
            return data.Payments.ToList();
        }
    }
}
