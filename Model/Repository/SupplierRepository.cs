using Model.EF;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace Model.Repository
{
    public class SupplierRepository
    {
        private OnlineShopDataContext data;
        public SupplierRepository(OnlineShopDataContext data)
        {
            this.data = data;
        }
        public List<Supplier> GetAllSupplier()
        {
            var nullid = ConfigurationManager.AppSettings["NullID"].ToString();
            return data.Suppliers.Where(x => x.SupplierID != nullid).ToList();
        }
        public List<Supplier> GetAllSupplierContacting()
        {
            return data.Suppliers.Where(x => x.Status == true).ToList();
        }
        public Supplier GetSupplierById(string id)
        {
            return data.Suppliers.FirstOrDefault(x => x.SupplierID == id);
        }
        public bool UpdateSupplier(string id, string suppliername, DateTime contractdate, string phone, string email, string updateby, bool status)
        {
            try
            {
                var supplierUpdate = data.Suppliers.FirstOrDefault(x => x.SupplierID == id);
                supplierUpdate.SupplierName = suppliername;
                supplierUpdate.ContractDate = contractdate;
                supplierUpdate.Phone = phone;
                supplierUpdate.Email = email;
                supplierUpdate.Status = status;
                supplierUpdate.UpdateAt = DateTime.Now;
                supplierUpdate.UpdateBy = updateby;
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool InsertSupplier(string supplierid, string suppliername, DateTime contractdate, string phone, string email, string updateby, bool status)
        {
            try
            {
                Supplier supplier = new Supplier() { SupplierID = supplierid, SupplierName = suppliername, ContractDate = contractdate, Phone = phone, Email = email, CreateAt = DateTime.Now, UpdateAt = DateTime.Now, UpdateBy = updateby, Status = status };
                data.Suppliers.Add(supplier);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool ChangeStatusSupplier(string id, string updateby)
        {
            var supplierUpdate = data.Suppliers.FirstOrDefault(x => x.SupplierID == id);
            supplierUpdate.Status = !supplierUpdate.Status;
            supplierUpdate.UpdateAt = DateTime.Now;
            supplierUpdate.UpdateBy = updateby;
            return supplierUpdate.Status;
        }
        public bool DeleteSupplier(string id)
        {
            try
            {
                var nullid = ConfigurationManager.AppSettings["NullID"].ToString();
                var supplierDelete = data.Suppliers.FirstOrDefault(x => x.SupplierID == id);
                data.Products.Where(x => x.SupplierID == supplierDelete.SupplierID).ToList().ForEach(x => x.SupplierID = nullid);
                data.Suppliers.Remove(supplierDelete);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
