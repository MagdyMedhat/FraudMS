using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace FraudMS.Components.Users
{
    public class UsersController : Controller
    {

        RAFMSDataContext m_context;

        public UsersController()
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
            return View("~/Components/Users/usersList.cshtml");
        }
        public JsonResult GetUsers()
        {
            var usersQuery = from c in m_context.Users select c;
            return Json(m_context.Users.Select(u => new
            {
                user_id = u.user_id,
                user_name = u.user_name,
                user_login = u.user_login,
                user_password = u.user_password,
                user_access = (from ac in m_context.Accesses join ua in m_context.UsersAccesses
                                                             on ac.access_id equals ua.access_id
                                                             where ua.user_id == u.user_id select ac.access_id).ToList(),
                                                             
                user_active=u.user_active,
                u.user_create_date,
                u.user_last_login
            }), JsonRequestBehavior.AllowGet); 
        }
    }
}
