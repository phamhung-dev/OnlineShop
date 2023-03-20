using Model.EF;
using System.Web;
using System.Web.Mvc;

namespace OnlineShop.Areas.Manager.Service
{
    public class HasAdminCredentialAttribute : AuthorizeAttribute
    {
        public int Role { get; set; }
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var sessionEmployee = HttpContext.Current.Session["UserCurrent"] as EmployeeAccount;
            if(sessionEmployee == null)
            {
                return false;
            }
            else
            {
                if(sessionEmployee.Role == 1)
                {
                    return true;
                }
                else
                {
                    if (sessionEmployee.Role == this.Role)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
        }
    }
}