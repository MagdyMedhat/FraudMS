using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FraudMS.Components.Common
{
    public class FraudApiController : Controller
    {
        RAFMSDataContext m_context;

        public FraudApiController()
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

        public JsonResult GetCase(int? case_id)
        {
            var values = from c in m_context.Case_vws
                         where c.case_id.ToString() == case_id.ToString()
                         select c;

            return Json(values, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCaseLine(int? case_id)
        {
            var values = from cl in m_context.case_line_vws
                         where cl.case_id.ToString() == case_id.ToString()
                         select cl;

            return Json(values, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetConnectionDomains()
        {
            var values = from c in m_context.ConnectionDomains_vws
                         select c;
            return Json(values, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetRegions()
        {
            var values = from c in m_context.Regions_vws
                         select c;
            return Json(values, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCities()
        {
            var values = from c in m_context.Cities_vws
                         select c;
            return Json(values, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetFraudStatus()
        {
            var values = from c in m_context.FraudStatus_vws
                         select c;
            return Json(values, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetFraudTypes()
        {
            var values = from c in m_context.FraudTypes_vws
                         select c;
            return Json(values, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetNationalities()
        {
            var values = from c in m_context.Nationality_vws
                         select c;
            return Json(values, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetGSMTypes()
        {
            var values = from c in m_context.GSM_vws
                         select c;
            return Json(values, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetUsers()
        {
            var values = from c in m_context.Users_vws
                         select c;
            return Json(values, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCaseStatus()
        {
            var values = from c in m_context.CaseStatus_vws
                         select c;
            return Json(values, JsonRequestBehavior.AllowGet);
        }

        public JsonResult checkAccessRight(string username,string pageID)
        {
            try
            {
                bool allowed = false;
                var userID = (from u in m_context.Users
                              where u.user_login == username
                              select u.user_id).FirstOrDefault();

                var hasAccess = (from ua in m_context.UsersAccesses
                                 where ua.user_id == userID
                                 select ua.access_id).ToList();

                foreach (short id in hasAccess)
                {
                    if (id == Convert.ToInt16(pageID))
                        allowed = true;
                    else
                        allowed = false;
                }
                return Json(allowed, JsonRequestBehavior.AllowGet);

            }
            catch (Exception)
            {
                return Json("false", JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Error()
        {
            return View("~/Views/error.cshtml");
        }

    }
}
