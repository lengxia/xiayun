using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
namespace xiayun.web.Models
{
    public class CustomAuthAttribute:AuthorizeAttribute
    {
        public string[] roles;
        public string[] users;
        public CustomAuthAttribute(string role, string user)//params
        {
            roles = role.Split(',');
            users = user.Split(',');
        }
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext.Session["user"] != null) return true;
            else return false;
            /*
            if (httpContext.Session["user"] != null)
            {
                string s = httpContext.Session["user"].ToString();
                foreach (var item in users)
                {
                    if (item == s)
                        return true;
                }
            }
            if (httpContext.Session["role"] != null)
            {
                string s = httpContext.Session["role"].ToString();
                foreach (var item in roles)
                {
                    if (item == s)
                        return true;
                }
            }

            
            return false;
            */
            // return base.AuthorizeCore(httpContext);
        }
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {


            filterContext.Result = new RedirectResult("/Account/Login");
        }
    }
}