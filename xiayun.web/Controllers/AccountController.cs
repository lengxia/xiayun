using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using xiayun.Model;
using xiayun.DAL;
using xiayun.IDAL;
namespace xiayun.web.Controllers
{
    public class AccountController : Controller
    {
        UserRepository userinfo = new UserRepository();
        // GET: Account
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public JsonResult Login(string userName, string pwd, string remember)
        {

            if (!userinfo.Exist(o => o.Username == userName))
            {
                return new JsonResult() { Data = "{\"code\":500,\"msg\":\"用户名不存在\"}" };
            }

            User user = userinfo.Find(x => x.Username == userName && x.Password == pwd);
            // user = userinfo.Find(x => x.Id == 1);
            // return new JsonResult() { Data = "{\"code\":500,\"msg\":\"" + userName + "\"}" };
            if (user == null)
            {
                return new JsonResult() { Data = "{\"code\":500,\"msg\":\"用户名和密码不匹配\"}" };
            }
            if (!string.IsNullOrEmpty(remember) && remember.Equals("checked"))
            {
                HttpCookie nameCookie = new HttpCookie("n", userName);
                nameCookie.Expires = DateTime.Now.AddDays(7);
                //将md5串写入cookie，或者再次进行AES加密写入
                HttpCookie pwdCookie = new HttpCookie("p", pwd);
                pwdCookie.Expires = DateTime.Now.AddDays(7);
                Response.Cookies.Add(nameCookie);
                Response.Cookies.Add(pwdCookie);
            }
            Session["user"] = userName;
            // FormsAuthentication.SetAuthCookie(userName,false);
            return new JsonResult() { Data = "{\"code\":200,\"msg\":\"登录成功\"}" };


        }
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public JsonResult Register(string userName, string pwd)
        {

            if (userinfo.Exist(o => o.Username == userName))
            {
                return new JsonResult() { Data = "{\"code\":500,\"msg\":\"用户名已存在\"}" };
            }


            User user = userinfo.Add(new User { Username = userName, Password = pwd });
            userinfo.SaveChanges();
            return new JsonResult() { Data = "{\"code\":200,\"msg\":\"注册成功，请使用新账号登陆\"}" };


        }
        public ActionResult LoginOut()
        {
            // FormsAuthentication.SignOut();
            Session["user"] = null;
            return RedirectToAction("Login");


        }
    }
}