using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Model.Repository
{
    public class EvaluationRepository
    {
        private OnlineShopDataContext data;
        public EvaluationRepository(OnlineShopDataContext data)
        {
            this.data = data;
        }
        public bool InsertEvaluation(Guid userID, string productID, byte scoreRating)
        {
            try
            {
                Evaluation evaluation = new Evaluation() { UserID = userID, ProductID = productID, ScoreRating = scoreRating };
                data.Evaluations.Add(evaluation);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public Evaluation GetEvaluationById(Guid userID, string productID)
        {
            return data.Evaluations.FirstOrDefault(x => x.UserID == userID && x.ProductID == productID);
        }
        public int GetAmountEvaluationById(string productID)
        {
            return data.Evaluations.Where(x => x.ProductID == productID).Count();
        }
        public List<Evaluation> GetListUserEvaluationByProductId(string productID)
        {
            return data.Evaluations.Where(x => x.ProductID == productID).ToList();
        }
    }
}
