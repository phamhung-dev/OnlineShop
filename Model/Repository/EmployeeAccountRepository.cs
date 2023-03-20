using Model.EF;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace Model.Repository
{
    public class EmployeeAccountRepository
    {
        private OnlineShopDataContext data;
        public EmployeeAccountRepository(OnlineShopDataContext data)
        {
            this.data = data;
        }
        public List<EmployeeAccount> GetAllEmployeeAccount()
        {
            Guid adminID = new Guid(ConfigurationManager.AppSettings["adminID"].ToString());
            return data.EmployeeAccounts.Where(x => x.EmployeeID != adminID).ToList();
        }
        public EmployeeAccount GetEmployeeById(Guid id)
        {
            return data.EmployeeAccounts.FirstOrDefault(x => x.EmployeeID == id);
        }
        public bool UpdateEmployee(Guid id, string firstname, string lastname, string phone, string address, int role, bool status)
        {
            try
            {
                var employeeUpdate = data.EmployeeAccounts.FirstOrDefault(x => x.EmployeeID == id);
                employeeUpdate.FirstName = firstname;
                employeeUpdate.LastName = lastname;
                employeeUpdate.Phone = phone;
                employeeUpdate.Address = address;
                employeeUpdate.Role = role;
                employeeUpdate.Status = status;
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool ChangeStatusEmployee(Guid id)
        {
            var employeeUpdate = data.EmployeeAccounts.FirstOrDefault(x => x.EmployeeID == id);
            employeeUpdate.Status = !employeeUpdate.Status;
            return employeeUpdate.Status;
        }
        public EmployeeAccount Login(string email, string password)
        {
            try
            {
                return data.EmployeeAccounts.FirstOrDefault(x => x.Email == email && x.Password == password);
            }
            catch
            {
                return null;
            }
        }
        public EmployeeAccount GetEmployeeByEmail(string email)
        {
            try
            {
                return data.EmployeeAccounts.FirstOrDefault(x => x.Email == email);
            }
            catch
            {
                return null;
            }
        }
        public bool InsertEmployeeAccount(string firstName, string lastName, string email, string phone, string password)
        {
            try
            {
                EmployeeAccount employee = new EmployeeAccount() { FirstName = firstName, LastName = lastName, Avatar = null, Email = email, Phone = phone, Password = password, Address = null, Role = 2, CreateAt = DateTime.Now, Status = false };
                data.EmployeeAccounts.Add(employee);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool PhoneIsUsed(string phone)
        {
            return data.EmployeeAccounts.FirstOrDefault(x => x.Phone == phone) == null ? false : true;
        }
        public bool EmailIsUsed(string email)
        {
            return data.EmployeeAccounts.FirstOrDefault(x => x.Email == email) == null ? false : true;
        }
        public bool UpdateAvatar(Guid id, byte[] avatar)
        {
            try
            {
                var employee = data.EmployeeAccounts.FirstOrDefault(x => x.EmployeeID == id);
                employee.Avatar = avatar;
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool ChangePassword(Guid id, string password)
        {
            try
            {
                var employee = data.EmployeeAccounts.FirstOrDefault(x => x.EmployeeID == id);
                employee.Password = password;
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool CheckPassword(Guid id, string password)
        {
            try
            {
                var employee = data.EmployeeAccounts.FirstOrDefault(x => x.EmployeeID == id);
                if (employee.Password.Equals(password))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}
