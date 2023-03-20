using Model.EF;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace Model.Repository
{
    public class AgeRepository
    {
        private OnlineShopDataContext data;
        public AgeRepository(OnlineShopDataContext data)
        {
            this.data = data;
        }
        public List<Age> GetAllAge()
        {
            try
            {
                var nullid = int.Parse(ConfigurationManager.AppSettings["IntNullID"].ToString());
                return data.Ages.Where(x => x.AgeID != nullid).ToList();
            }
            catch
            {
                return null;
            }
        }
        public List<Age> GetAllAgeSelling()
        {
            try
            {
                return data.Ages.Where(x => x.Status == true).ToList();
            }
            catch
            {
                return null;
            }
        }
        public Age GetAgeByName(string name)
        {
            return data.Ages.FirstOrDefault(x => x.AgeName == name);
        }
        public Age GetNewAge()
        {
            return data.Ages.OrderByDescending(x => x.AgeID).FirstOrDefault();
        }
        public bool InsertAge(string agename, string updateby, bool status)
        {
            try
            {
                Age age = new Age() { AgeName = agename, CreateAt = DateTime.Now, UpdateAt = DateTime.Now, UpdateBy = updateby, Status = status };
                data.Ages.Add(age);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool ChangeStatusAge(int id, string updateby)
        {
            var ageUpdate = data.Ages.FirstOrDefault(x => x.AgeID == id);
            ageUpdate.Status = !ageUpdate.Status;
            ageUpdate.UpdateAt = DateTime.Now;
            ageUpdate.UpdateBy = updateby;
            return ageUpdate.Status;
        }
        public bool UpdateAge(int id, string agename, string updateby)
        {
            try
            {
                var ageUpdate = data.Ages.FirstOrDefault(x => x.AgeID == id);
                ageUpdate.AgeName = agename;
                ageUpdate.UpdateAt = DateTime.Now;
                ageUpdate.UpdateBy = updateby;
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool DeleteAge(int id)
        {
            try
            {
                var nullid = int.Parse(ConfigurationManager.AppSettings["IntNullID"].ToString());
                var ageDelete = data.Ages.FirstOrDefault(x => x.AgeID == id);
                data.Products.Where(x => x.AgeID == ageDelete.AgeID).ToList().ForEach(x => x.AgeID = nullid);
                data.Ages.Remove(ageDelete);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
