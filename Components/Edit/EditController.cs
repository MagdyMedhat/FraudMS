using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FraudMS.Components.Edit
{
    public class EditController : Controller
    {
        RAFMSDataContext m_context;

        public EditController()
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
            //ViewBag.id = id;
            return View("~/Components/Edit/EditView.cshtml");
        }

        [HttpPost]
        public ActionResult Update(Case retcase, List<case_line> case_lines)
        { 
            //one case only
            var cases  = (from cc in m_context.Cases
                       where cc.case_id.ToString() == retcase.case_id.ToString()
                       select cc);

            foreach (var c in cases)
            {
                c.case_billing_user2 = retcase.case_billing_user2;
                c.case_customer_name = retcase.case_customer_name;
                c.case_customer_number = Convert.ToInt64(retcase.case_customer_number);
                c.case_ID_number = retcase.case_ID_number;
                c.case_nationality_id = Convert.ToInt16(retcase.case_nationality_id.ToString());
                c.case_region = retcase.case_region;
                c.case_city = retcase.case_city;
                c.case_Activate_date = retcase.case_Activate_date;
                //c.case_connection_domain = retcase.case_connection_domain;
                //byte bTemp;
                //if (byte.TryParse(case_gsm_id.ToString(), out bTemp))
                //    c.case_gsm_id = bTemp;
                //else
                //    c.case_gsm_id = null;
                c.case_fraudStatus_id = retcase.case_fraudStatus_id;
                //c.case_fraudType_id = short.Parse(case_fraudType_id.ToString());
                c.case_bill_cycle = retcase.case_bill_cycle;
                c.case_bill_date = retcase.case_bill_date;
                c.case_bill_hdate = retcase.case_bill_hdate;
                c.case_source = retcase.case_source;
                //c.case_user_id = short.Parse(case_user_id.ToString());
                //c.case_billing_user = case_billing_user;
                c.case_date = retcase.case_date;
                c.case_hijri = retcase.case_hijri;
                c.case_status = retcase.case_status;
                c.case_disconnected_numbers_count = retcase.case_disconnected_numbers_count;
                c.case_credit_limit = retcase.case_credit_limit;
                c.case_estimated_loss = retcase.case_estimated_loss;
                c.case_actual_loss = retcase.case_actual_loss;
                //c.case_note_intractive = retcase.case_note_intractive;
                c.case_comment = retcase.case_comment;
                //c.case_descreption = retcase.case_descreption; 
                
            }
                

            m_context.SubmitChanges();

            List<long> tobedeletedIDs = (from cl in m_context.case_lines
                                         where cl.case_id == Convert.ToInt64(retcase.case_id)
                                         select cl.case_line_id).ToList();

            var tobedeleted = from cl in m_context.case_lines
                              where tobedeletedIDs.Contains(cl.case_line_id)
                              select cl;

            m_context.case_lines.DeleteAllOnSubmit(tobedeleted);
            m_context.SubmitChanges();
            foreach (var case_line in case_lines)
            {
                case_line.case_id = retcase.case_id;
                m_context.case_lines.InsertOnSubmit(case_line); 
            } 
            m_context.SubmitChanges();

            Dictionary<string, string> Ret = new Dictionary<string, string>();
            Ret.Add("result", "true");
            return Json(Ret, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCase(int id)
        {
            var returnedCase = (from c in m_context.Case_vws where c.case_id == id select c).FirstOrDefault();
            var returnedCaselines = from c in m_context.case_line_vws where c.case_id == id select c;
            return Json(new object[] { returnedCase, returnedCaselines }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Delete(int id)
        {
            var returnedCase = (from c in m_context.Cases where c.case_id == id select c);
            m_context.Cases.DeleteAllOnSubmit(returnedCase);
            m_context.SubmitChanges();
            return Json("success", JsonRequestBehavior.AllowGet);
        }

    }
}
