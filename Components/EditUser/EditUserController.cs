using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace FraudMS.Components.EditUser
{
    public class EditUserController : Controller
    {
        RAFMSDataContext m_context;

        public EditUserController()
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
            return View("~/Components/EditUser/EditUser.cshtml");
        }

        [HttpPost]
        public ActionResult CreateUser(User user, List<string> access)
        {
            try
            {
                if (userAlreadyExists(user.user_login))
                {
                    Dictionary<string, object> Ret = new Dictionary<string, object>();
                    Ret.Add("exists", true);
                    return Json(Ret, JsonRequestBehavior.AllowGet);
                }
                else
                {

                    user.user_password = HelperMethods.GetSHA1HashData(user.user_password);
                    user.user_create_date = DateTime.Now;
                    if (access != null)
                    {
                        if (access.Count > 0)
                        {
                            for (int i = 0; i < access.Count; i++)
                                user.UsersAccesses.Add(new UsersAccess { access_id = Convert.ToInt16(access[i]), user_id = user.user_id });
                        }
                    }
                    m_context.Users.InsertOnSubmit(user);
                    m_context.SubmitChanges();
                    Dictionary<string, object> Ret = new Dictionary<string, object>();
                    Ret.Add("exists", false);
                    return Json(Ret, JsonRequestBehavior.AllowGet);
                
                }
            }
            catch (Exception)
            {
                Dictionary<string, object> Ret = new Dictionary<string, object>();
                Ret.Add("exists", false);
                return Json(Ret, JsonRequestBehavior.AllowGet);
            }
           
        }
        public ActionResult UpdateUser(User user, List<string> access)
        {
            var query =  from u in m_context.Users
                        where u.user_id == user.user_id
                        select u;

            for (int i = 0; i < access.Count; i++)
                user.UsersAccesses.Add(new UsersAccess { access_id = Convert.ToInt16(access[i]), user_id = user.user_id });
            var accessQuery = from a in m_context.UsersAccesses
                              where a.user_id == user.user_id
                              select a;

            foreach (var accessDetail in accessQuery)
                m_context.UsersAccesses.DeleteOnSubmit(accessDetail);

            foreach (User u in query)
            {
                u.user_name = user.user_name;
                u.user_login = user.user_login;
                u.user_password = HelperMethods.GetSHA1HashData(user.user_password);
                u.user_active = user.user_active;
                u.UsersAccesses = user.UsersAccesses;
            } 
            try
            {
                m_context.SubmitChanges();
                return Json("true", JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json("false", JsonRequestBehavior.AllowGet);
            } 
        }
        public ActionResult DeleteUser(int user_id)
         {
                var userAccessDetails = from ua in m_context.UsersAccesses
                                     where ua.user_id==user_id
                                     select ua;

                foreach (var accessDetail in userAccessDetails)
                    m_context.UsersAccesses.DeleteOnSubmit(accessDetail);

                var UserDetails =
                                     from user in m_context.Users
                                     where user.user_id == user_id
                                     select user;

                foreach (var userDetail in UserDetails)
                    m_context.Users.DeleteOnSubmit(userDetail);

             try
             {
                 m_context.SubmitChanges();
                 return Json("true", JsonRequestBehavior.AllowGet);
             }
             catch (Exception e)
             {
                 return Json("false", JsonRequestBehavior.AllowGet);
             }
            
         }
        public ActionResult GetAccess()
        {
            var accessQuery = (from a in m_context.Accesses select new { key= a.access_id ,value = a.access_name });
            return Json(accessQuery, JsonRequestBehavior.AllowGet);
        }
        private bool userAlreadyExists(string userlogin)
        {
            List<short> query =( from u in m_context.Users
                        where u.user_login == userlogin
                        select u.user_id).ToList();
            if (query.Count() > 0)
                return true;
            else 
                return false;
        }
        
    }
}
