using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Model.Repository
{
    public class SizeRepository
    {
        private OnlineShopDataContext data;
        public SizeRepository(OnlineShopDataContext data)
        {
            this.data = data;
        }
        public List<Size> GetAllSize()
        {
            return data.Sizes.ToList();
        }
        public List<Size> GetAllSizeSelling()
        {
            return data.Sizes.Where(x => x.Status == true).ToList();
        }
        public Size GetSizeById(string id)
        {
            return data.Sizes.FirstOrDefault(x => x.SizeID == id);
        }
        public bool ChangeStatusSize(string id, string updateby)
        {
            var sizeUpdate = data.Sizes.FirstOrDefault(x => x.SizeID == id);
            sizeUpdate.Status = !sizeUpdate.Status;
            sizeUpdate.UpdateAt = DateTime.Now;
            sizeUpdate.UpdateBy = updateby;
            return sizeUpdate.Status;
        }
        public bool UpdateSize(string id, string sizename, string updateby)
        {
            try
            {
                var sizeUpdate = data.Sizes.FirstOrDefault(x => x.SizeID == id);
                sizeUpdate.SizeName = sizename;
                sizeUpdate.UpdateAt = DateTime.Now;
                sizeUpdate.UpdateBy = updateby;
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool InsertSize(string sizeid, string sizename, string updateby, bool status)
        {
            try
            {
                Size size = new Size() { SizeID = sizeid, SizeName = sizename, CreateAt = DateTime.Now, UpdateAt = DateTime.Now, UpdateBy = updateby, Status = status };
                data.Sizes.Add(size);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
