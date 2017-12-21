using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace FraudMS.Components.Login
{
    public class LoginController : Controller
    {

        RAFMSDataContext m_context;

        public LoginController()
        {
            m_context = new RAFMSDataContext();
        }

        protected override JsonResult Json(object data, string contentType, System.Text.Encoding contentEncoding, JsonRequestBehavior behavior)
        {
            return new JsonResult()
            {
                Data = data,
                ContentType = contentType,
                ContentEncoding = contentEncoding,
                JsonRequestBehavior = behavior,
                MaxJsonLength = Int32.MaxValue
            };
        }

        public ActionResult Index()
        {
            return View("~/Components/Login/Login.cshtml");
        }
        public ActionResult authUser(string username, string password)
        {
            var exists = (from u in m_context.Users
                          where u.user_login.ToLower() == username.ToLower()
                          && u.user_password == HelperMethods.GetSHA1HashData(password)
                          select u.user_id).ToList();

            List<short> userAccessRights = null;
            bool isLogged = false;
            if (exists != null)
            {
                if (exists.Count > 0)
                {
                    isLogged = true;
                    foreach (var item in exists)
                    {
                        if (item > 0)
                        {
                            userAccessRights = (from ua in m_context.UsersAccesses
                                                where ua.user_id == item
                                                select ua.access_id).ToList();
                        }
                        else
                            userAccessRights = null;
                    }
                }
            }

            Dictionary<string, object> Ret = new Dictionary<string, object>();
            Ret.Add("is_logged", isLogged);
            Ret.Add("access_control", userAccessRights);
            if (exists.Count > 0)
            Ret.Add("user_id", exists[0]);
            else
                Ret.Add("user_id", 0);
            return Json(Ret, JsonRequestBehavior.AllowGet);
        }
    }
}
