using Model.EF;
using System;

namespace Model.Repository
{
    public class FeedbackRepository
    {
        private OnlineShopDataContext data;
        public FeedbackRepository(OnlineShopDataContext data)
        {
            this.data = data;
        }
        public bool InsertFeedback(Guid userID, string productID, string content)
        {
            try
            {
                Feedback feedback = new Feedback() { UserID = userID, ProductID = productID, FeedbackDate = DateTime.Now, Content = content };
                data.Feedbacks.Add(feedback);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
