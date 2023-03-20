using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Model.Repository
{
    public class UserAccountRepository
    {
        private OnlineShopDataContext data;
        public UserAccountRepository(OnlineShopDataContext data)
        {
            this.data = data;
        }
        public List<UserAccount> GetAllUserAccount()
        {
            return data.UserAccounts.ToList();
        }
        public int CountUserAccount()
        {
            return data.UserAccounts.Where(x => x.Status == true).Count();
        }
        public UserAccount GetUserById(Guid id)
        {
            return data.UserAccounts.FirstOrDefault(x => x.UserID == id);
        }
        public bool UpdateUser(Guid id, string firstname, string lastname, bool gender, DateTime birthday, string phone, string address, bool status)
        {
            try
            {
                var userUpdate = data.UserAccounts.FirstOrDefault(x => x.UserID == id);
                userUpdate.FirstName = firstname;
                userUpdate.LastName = lastname;
                userUpdate.Gender = gender;
                userUpdate.Birthday = birthday;
                userUpdate.Phone = phone;
                userUpdate.Address = address;
                userUpdate.Status = status;
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool ChangeStatusUser(Guid id)
        {
            var userUpdate = data.UserAccounts.FirstOrDefault(x => x.UserID == id);
            userUpdate.Status = !userUpdate.Status;
            return userUpdate.Status;
        }
        public UserAccount Login(string email, string password)
        {
            try
            {
                return data.UserAccounts.FirstOrDefault(x => x.Email == email && x.Password == password);
            }
            catch
            {
                return null;
            }
        }
        public bool PhoneIsUsed(string phone)
        {
            return data.UserAccounts.FirstOrDefault(x => x.Phone == phone) == null ? false : true;
        }
        public bool EmailIsUsed(string email)
        {
            return data.UserAccounts.FirstOrDefault(x => x.Email == email) == null ? false : true;
        }
        public bool InsertUserAccount(string firstName, string lastName, string phone, string address, string email, string password)
        {
            try
            {
                UserAccount userAccount = new UserAccount() { FirstName = firstName, LastName = lastName, Avatar = null, Gender = true, Birthday = DateTime.Now.AddYears(-18), Email = email, EmailConfirmed = false, Phone = phone, Password = password, Address = address, CreateAt = DateTime.Now, Status = true };
                data.UserAccounts.Add(userAccount);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public UserAccount GetUserByEmail(string email)
        {
            try
            {
                return data.UserAccounts.FirstOrDefault(x => x.Email == email);
            }
            catch
            {
                return null;
            }
        }
        public bool UpdateAvatar(Guid id, byte[] avatar)
        {
            try
            {
                var userAccount = data.UserAccounts.FirstOrDefault(x => x.UserID == id);
                userAccount.Avatar = avatar;
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool ConfirmEmail(Guid id)
        {
            try
            {
                var userAccount = data.UserAccounts.FirstOrDefault(x => x.UserID == id);
                userAccount.EmailConfirmed = true;
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
                var userAccount = data.UserAccounts.FirstOrDefault(x => x.UserID == id);
                userAccount.Password = password;
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
                var userAccount = data.UserAccounts.FirstOrDefault(x => x.UserID == id);
                if (userAccount.Password.Equals(password))
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
