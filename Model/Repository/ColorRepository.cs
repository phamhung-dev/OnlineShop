using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Model.Repository
{
    public class ColorRepository
    {
        private OnlineShopDataContext data;
        public ColorRepository(OnlineShopDataContext data)
        {
            this.data = data;
        }
        public List<Color> GetAllColor()
        {
            return data.Colors.ToList();
        }
        public List<Color> GetAllColorSelling()
        {
            return data.Colors.Where(x => x.Status  == true).ToList();
        }
        public Color GetColorById(string id)
        {
            return data.Colors.FirstOrDefault(x => x.ColorID == id);
        }
        public bool ChangeStatusColor(string id, string updateby)
        {
            var colorUpdate = data.Colors.FirstOrDefault(x => x.ColorID == id);
            colorUpdate.Status = !colorUpdate.Status;
            colorUpdate.UpdateAt = DateTime.Now;
            colorUpdate.UpdateBy = updateby;
            return colorUpdate.Status;
        }
        public bool UpdateColor(string id, string colorname, string updateby)
        {
            try
            {
                var colorUpdate = data.Colors.FirstOrDefault(x => x.ColorID == id);
                colorUpdate.ColorName = colorname;
                colorUpdate.UpdateAt = DateTime.Now;
                colorUpdate.UpdateBy = updateby;
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool InsertColor(string colorid, string colorname, string updateby, bool status)
        {
            try
            {
                Color color = new Color() { ColorID  = colorid, ColorName = colorname, CreateAt = DateTime.Now, UpdateAt = DateTime.Now, UpdateBy = updateby, Status = status };
                data.Colors.Add(color);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
