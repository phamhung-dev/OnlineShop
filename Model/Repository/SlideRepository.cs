using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Model.Repository
{
    public class SlideRepository
    {
        private OnlineShopDataContext data;
        public SlideRepository(OnlineShopDataContext data)
        {
            this.data = data;
        }
        public List<Slide> GetAllSlide()
        {
            return data.Slides.ToList();
        }
        public List<Slide> GetAllSlideUsing()
        {
            return data.Slides.Where(x => x.Status == true).OrderBy(x => x.PositionAppear).ToList();
        }
        public Slide GetNewSlide()
        {
            return data.Slides.OrderByDescending(x => x.SlideID).FirstOrDefault();
        }
        public bool DeleteSlide(int id)
        {
            try
            {
                var slideDetele = data.Slides.FirstOrDefault(x => x.SlideID == id);
                data.Slides.Remove(slideDetele);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool ChangeStatusSlide(int id, string updateby)
        {
            try
            {
                var slideUpdate = data.Slides.FirstOrDefault(x => x.SlideID == id);
                slideUpdate.Status = !slideUpdate.Status;
                slideUpdate.UpdateAt = DateTime.Now;
                slideUpdate.UpdateBy = updateby;
                if (!slideUpdate.Status)
                {
                    slideUpdate.PositionAppear = 0;
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool ChangePositionAppearSlide(int id, int position, string updateby)
        {
            try
            {
                var slideUpdate = data.Slides.FirstOrDefault(x => x.SlideID == id);
                slideUpdate.PositionAppear = position;
                slideUpdate.Status = true;
                slideUpdate.UpdateAt = DateTime.Now;
                slideUpdate.UpdateBy = updateby;
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool IsExistPositionAppear(int position)
        {
            try
            {
                var slideDetele = data.Slides.FirstOrDefault(x => x.PositionAppear == position);
                if (slideDetele == null)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        public bool InsertSlide(byte[] photo, string updateBy)
        {
            try
            {
                Slide slide = new Slide() { Photo = photo, PositionAppear = 0, CreateAt = DateTime.Now, UpdateAt = DateTime.Now, UpdateBy = updateBy, Status = false };
                data.Slides.Add(slide);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
