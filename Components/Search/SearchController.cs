using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FraudMS.Components.Search
{
    public class SearchController : Controller
    {
        RAFMSDataContext m_context;

        public SearchController()
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
            return View("~/Components/Search/SearchView.cshtml");
        }

        public JsonResult Search(int? case_connection_domain, int? case_region, string case_city,
                    int? case_fraudStatus_id, int? case_fraudType_id, string case_customer_name,
                    int? case_customer_number, int? service_no, int? account_no, string case_ID_number,
                    int? case_nationality_id, int? case_gsm_id, int? case_bill_cycle, string case_bill_date,
                    string case_bill_hdate, string case_source, int? case_user_id, string case_date,
                    string case_date2, string case_line_date, string case_line_date2, string case_hijri, int? case_id, string case_billing_user2,
                    string case_billing_user, int? case_status, int? case_disconnected_numbers_count,
                    string case_Activate_date, int? current_page, int? page_size)
        {

            //inferes the type IQueryable<Cases_vw>
            var casesSearchQuery = from c in m_context.Case_vws select c;
            var caseLinesSearchQuery = from cl in m_context.case_line_vws
                                       select cl;







            //if (case_connection_domain != null)
            //    casesSearchQuery = casesSearchQuery.Where(c => c.case_connection_domain.Equals(case_connection_domain));

            if (case_connection_domain != null)
                caseLinesSearchQuery = caseLinesSearchQuery.Where(c => c.case_line_connection_domain.Equals(case_connection_domain));

            if (case_region != null)
                casesSearchQuery = casesSearchQuery.Where(c => c.case_region.Equals(case_region));

            if (case_city != null)
                casesSearchQuery = casesSearchQuery.Where(c => c.case_city.Equals(case_city));

            if (case_fraudStatus_id != null)
                casesSearchQuery = casesSearchQuery.Where(c => c.case_fraudStatus_id.Equals(case_fraudStatus_id));

            if (case_fraudType_id != null)
            {
                caseLinesSearchQuery = caseLinesSearchQuery.Where(cl => cl.case_fraudType_id.Equals(case_fraudType_id));

                //casesSearchQuery = from c in casesSearchQuery
                //                   join cl in m_context.case_line_vws on
                //                   c.case_id equals cl.case_id
                //                   where cl.case_fraudType_id == case_fraudType_id
                //                   select c;
            }

            if (!String.IsNullOrEmpty(case_customer_name))
                casesSearchQuery = casesSearchQuery.Where(c => c.case_customer_name.Equals(case_customer_name));

            if (case_customer_number != null)
                casesSearchQuery = casesSearchQuery.Where(c => c.case_customer_number.Equals(case_customer_number));

            if (service_no != null)
            {
                caseLinesSearchQuery = caseLinesSearchQuery.Where(cl => cl.service_no.Equals(service_no));

                //var q = from cl in m_context.case_line_vws where cl.service_no.ToString() == service_no.ToString() select cl.case_id;
                //List<long?> records = q.ToList();

                //if (records.Count > 0)
                //    casesSearchQuery = casesSearchQuery.Where(c => records.Contains(Convert.ToInt64( c.case_id)));

            }

            if (account_no != null)
            {
                caseLinesSearchQuery = caseLinesSearchQuery.Where(cl => cl.account_no.Equals(account_no));

                //var q = from cl in m_context.case_line_vws where cl.account_no.ToString() == account_no.ToString() select cl.case_id;
                //List<long?> records = q.ToList();

                //if (records.Count > 0)
                //    casesSearchQuery = casesSearchQuery.Where(c => records.Contains(Convert.ToInt32(c.case_id)));
            }

            if (case_ID_number != null)
                casesSearchQuery = casesSearchQuery.Where(c => c.case_ID_number.Equals(case_ID_number));

            if (case_nationality_id != null)
                casesSearchQuery = casesSearchQuery.Where(c => c.case_nationality_id.Equals(case_nationality_id));

            if (case_gsm_id != null)
            {
                caseLinesSearchQuery = caseLinesSearchQuery.Where(cl => cl.case_gsm_id.Equals(case_gsm_id));

                //var q = from cl in m_context.case_line_vws where cl.case_gsm_id.ToString() == case_gsm_id.ToString() select cl.case_id;
                //List<long?> records = q.ToList();

                //if (records.Count > 0)
                //    casesSearchQuery = casesSearchQuery.Where(c => records.Contains(Convert.ToInt32(c.case_id)));
            }

            if (case_bill_cycle != null)
                casesSearchQuery = casesSearchQuery.Where(c => c.case_bill_cycle.Equals(case_bill_cycle));

            if (!String.IsNullOrEmpty(case_bill_date))
            {
                DateTime date = DateTime.ParseExact(case_bill_date, "dd/MM/yyyy", null);
                casesSearchQuery = casesSearchQuery.Where(c => c.case_bill_date.Value.Date == date);
            }

            if (!String.IsNullOrEmpty(case_bill_hdate))
                casesSearchQuery = casesSearchQuery.Where(c => c.case_bill_hdate.Equals(case_bill_hdate));

            if (!String.IsNullOrEmpty(case_source))
                casesSearchQuery = casesSearchQuery.Where(c => c.case_source.Equals(case_source));


            if (!String.IsNullOrEmpty(case_date) && String.IsNullOrEmpty(case_date2))
            {

                DateTime date = DateTime.ParseExact(case_date, "dd/MM/yyyy", null);
                //var dateString = case_date.ToString(System.Globalization.CultureInfo.CurrentCulture);
                casesSearchQuery = casesSearchQuery.Where(c => c.case_date.Value.Date == date);
            }

            else if (!String.IsNullOrEmpty(case_date) && !String.IsNullOrEmpty(case_date2))
            {
                DateTime start_date = DateTime.ParseExact(case_date, "dd/MM/yyyy", null);
                DateTime end_date = DateTime.ParseExact(case_date2, "dd/MM/yyyy", null);
                casesSearchQuery = casesSearchQuery.Where(c => c.case_date >= start_date.Date && c.case_date.Value.Date <= end_date.Date);
            }

            if (!String.IsNullOrEmpty(case_line_date) && String.IsNullOrEmpty(case_line_date2))
            {

                DateTime date = DateTime.ParseExact(case_line_date, "dd/MM/yyyy", null);
                //var dateString = case_date.ToString(System.Globalization.CultureInfo.CurrentCulture);
                caseLinesSearchQuery = caseLinesSearchQuery.Where(c => c.case_line_date.Value.Date == date);
            }

            else if (!String.IsNullOrEmpty(case_line_date) && !String.IsNullOrEmpty(case_line_date2))
            {
                DateTime start_date = DateTime.ParseExact(case_line_date, "dd/MM/yyyy", null);
                DateTime end_date = DateTime.ParseExact(case_line_date2, "dd/MM/yyyy", null);
                caseLinesSearchQuery = caseLinesSearchQuery.Where(c => c.case_line_date >= start_date.Date && c.case_line_date.Value.Date <= end_date.Date);
            }

            if (!String.IsNullOrEmpty(case_hijri))
                casesSearchQuery = casesSearchQuery.Where(c => c.case_hijri.Equals(case_hijri));

            if (case_id != null)
                casesSearchQuery = casesSearchQuery.Where(c => c.case_id.Equals(case_id));

            if (!String.IsNullOrEmpty(case_billing_user2))
                casesSearchQuery = casesSearchQuery.Where(c => c.case_billing_user2.Equals(case_billing_user2));

            //if (!String.IsNullOrEmpty(case_billing_user))
            //{
            //    caseLinesSearchQuery = caseLinesSearchQuery.Where(cl => cl.case_billing_user.Equals(case_billing_user));

            //var q = from cl in m_context.case_line_vws where cl.case_billing_user.ToString() == case_billing_user.ToString() select cl.case_id;
            //List<long?> records = q.ToList();

            //if (records.Count > 0)
            //    casesSearchQuery = casesSearchQuery.Where(c => records.Contains(Convert.ToInt32(c.case_id)));
            //}

            if (case_status != null)
                casesSearchQuery = casesSearchQuery.Where(c => c.case_status.Equals(case_status));

            if (case_disconnected_numbers_count != null)
                casesSearchQuery = casesSearchQuery.Where(c => c.case_disconnected_numbers_count.Equals(case_disconnected_numbers_count));

            if (!String.IsNullOrEmpty(case_Activate_date))
            {
                DateTime date = DateTime.ParseExact(case_Activate_date, "dd/MM/yyyy", null);
                casesSearchQuery = casesSearchQuery.Where(c => c.case_Activate_date.Value.Date == date);
            }

            if (case_user_id != null)
            {
                caseLinesSearchQuery = caseLinesSearchQuery.Where(cl => cl.case_user_id.Equals(case_user_id));

                //casesSearchQuery = from c in casesSearchQuery
                //                   join cl in m_context.case_line_vws on
                //                   c.case_id equals cl.case_id
                //                   where cl.case_user_id == case_user_id
                //                   select c;
            }
            var results = (from c in casesSearchQuery
                           join cl in caseLinesSearchQuery
                          on c.case_id equals cl.case_id
                           select c).Distinct();


            int casesCount = results.Count();
            decimal? totalActualMoney = 0;
            decimal? totalEstimatedMoney = 0;
            int totalDisconnectedNumbers = 0;

            if (casesCount > 0)
            {
                totalActualMoney = results.Sum(t => t.case_actual_loss);
                totalEstimatedMoney = results.Sum(t => t.case_estimated_loss);
                totalDisconnectedNumbers = Convert.ToInt32(results.Sum(t => t.case_disconnected_numbers_count));

                if (totalActualMoney == null)
                    totalActualMoney = 0;

                if (totalEstimatedMoney == null)
                    totalEstimatedMoney = 0;

                if (totalDisconnectedNumbers == null)
                    totalDisconnectedNumbers = 0;

            }

            Dictionary<string, object> Ret = new Dictionary<string, object>();
            Ret.Add("cases_count", casesCount);
            Ret.Add("acutal_loss", totalActualMoney);
            Ret.Add("estimated_loss", totalEstimatedMoney);
            Ret.Add("disconnected_numbers", totalDisconnectedNumbers);

            return Json(Ret, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPage(int? case_connection_domain, int? case_region, string case_city,
            int? case_fraudStatus_id, int? case_fraudType_id, string case_customer_name,
            int? case_customer_number, int? service_no, int? account_no, string case_ID_number,
            int? case_nationality_id, int? case_gsm_id, int? case_bill_cycle, string case_bill_date,
            string case_bill_hdate, string case_source, int? case_user_id, string case_date,
            string case_date2, string case_line_date, string case_line_date2, string case_hijri, int? case_id, string case_billing_user2,
            string case_billing_user, int? case_status, int? case_disconnected_numbers_count,
            string case_Activate_date, int? current_page, int? page_size)
        {
            //inferes the type IQueryable<Cases_vw>
            var casesSearchQuery = from c in m_context.Case_vws select c;




            var caseLinesSearchQuery = from cl in m_context.case_line_vws select cl;

            if (case_connection_domain != null)
                caseLinesSearchQuery = caseLinesSearchQuery.Where(c => c.case_line_connection_domain.Equals(case_connection_domain));

            if (case_region != null)
                casesSearchQuery = casesSearchQuery.Where(c => c.case_region.Equals(case_region));

            if (case_city != null)
                casesSearchQuery = casesSearchQuery.Where(c => c.case_city.Equals(case_city));

            if (case_fraudStatus_id != null)
                casesSearchQuery = casesSearchQuery.Where(c => c.case_fraudStatus_id.Equals(case_fraudStatus_id));

            if (case_fraudType_id != null)
            {
                caseLinesSearchQuery = caseLinesSearchQuery.Where(cl => cl.case_fraudType_id.Equals(case_fraudType_id));

                //casesSearchQuery = from c in casesSearchQuery
                //                   join cl in m_context.case_line_vws on
                //                   c.case_id equals cl.case_id
                //                   where cl.case_fraudType_id == case_fraudType_id
                //                   select c;
            }

            if (!String.IsNullOrEmpty(case_customer_name))
                casesSearchQuery = casesSearchQuery.Where(c => c.case_customer_name.Equals(case_customer_name));

            if (case_customer_number != null)
                casesSearchQuery = casesSearchQuery.Where(c => c.case_customer_number.Equals(case_customer_number));

            if (service_no != null)
            {
                caseLinesSearchQuery = caseLinesSearchQuery.Where(cl => cl.service_no.Equals(service_no));

                //var q = from cl in m_context.case_line_vws where cl.service_no.ToString() == service_no.ToString() select cl.case_id;
                //List<long?> records = q.ToList();

                //if (records.Count > 0)
                //    casesSearchQuery = casesSearchQuery.Where(c => records.Contains(Convert.ToInt64( c.case_id)));

            }

            if (account_no != null)
            {
                caseLinesSearchQuery = caseLinesSearchQuery.Where(cl => cl.account_no.Equals(account_no));

                //var q = from cl in m_context.case_line_vws where cl.account_no.ToString() == account_no.ToString() select cl.case_id;
                //List<long?> records = q.ToList();

                //if (records.Count > 0)
                //    casesSearchQuery = casesSearchQuery.Where(c => records.Contains(Convert.ToInt32(c.case_id)));
            }

            if (case_ID_number != null)
                casesSearchQuery = casesSearchQuery.Where(c => c.case_ID_number.Equals(case_ID_number));

            if (case_nationality_id != null)
                casesSearchQuery = casesSearchQuery.Where(c => c.case_nationality_id.Equals(case_nationality_id));

            if (case_gsm_id != null)
            {
                caseLinesSearchQuery = caseLinesSearchQuery.Where(cl => cl.case_gsm_id.Equals(case_gsm_id));

                //var q = from cl in m_context.case_line_vws where cl.case_gsm_id.ToString() == case_gsm_id.ToString() select cl.case_id;
                //List<long?> records = q.ToList();

                //if (records.Count > 0)
                //    casesSearchQuery = casesSearchQuery.Where(c => records.Contains(Convert.ToInt32(c.case_id)));
            }

            if (case_bill_cycle != null)
                casesSearchQuery = casesSearchQuery.Where(c => c.case_bill_cycle.Equals(case_bill_cycle));
            if (!String.IsNullOrEmpty(case_bill_date))
            {
                DateTime date = DateTime.ParseExact(case_bill_date, "dd/MM/yyyy", null);
                casesSearchQuery = casesSearchQuery.Where(c => c.case_bill_date.Value.Date == date);

            }

            if (!String.IsNullOrEmpty(case_bill_hdate))
                casesSearchQuery = casesSearchQuery.Where(c => c.case_bill_hdate.Equals(case_bill_hdate));

            if (!String.IsNullOrEmpty(case_source))
                casesSearchQuery = casesSearchQuery.Where(c => c.case_source.Equals(case_source));

            if (!String.IsNullOrEmpty(case_date) && String.IsNullOrEmpty(case_date2))
            {
                DateTime date = DateTime.ParseExact(case_date, "dd/MM/yyyy", null);
                casesSearchQuery = casesSearchQuery.Where(c => c.case_date.Value.Date == date);
            }

            else if (!String.IsNullOrEmpty(case_date) && !String.IsNullOrEmpty(case_date2))
            {
                DateTime start_date = DateTime.ParseExact(case_date, "dd/MM/yyyy", null);
                DateTime end_date = DateTime.ParseExact(case_date2, "dd/MM/yyyy", null);
                casesSearchQuery = casesSearchQuery.Where(c => c.case_date >= start_date.Date && c.case_date.Value.Date <= end_date.Date);
            }

            if (!String.IsNullOrEmpty(case_line_date) && String.IsNullOrEmpty(case_line_date2))
            {

                DateTime date = DateTime.ParseExact(case_line_date, "dd/MM/yyyy", null);
                //var dateString = case_date.ToString(System.Globalization.CultureInfo.CurrentCulture);
                caseLinesSearchQuery = caseLinesSearchQuery.Where(c => c.case_line_date.Value.Date == date);
            }

            else if (!String.IsNullOrEmpty(case_line_date) && !String.IsNullOrEmpty(case_line_date2))
            {
                DateTime start_date = DateTime.ParseExact(case_line_date, "dd/MM/yyyy", null);
                DateTime end_date = DateTime.ParseExact(case_line_date2, "dd/MM/yyyy", null);
                caseLinesSearchQuery = caseLinesSearchQuery.Where(c => c.case_line_date >= start_date.Date && c.case_line_date.Value.Date <= end_date.Date);
            }

            if (!String.IsNullOrEmpty(case_hijri))
                casesSearchQuery = casesSearchQuery.Where(c => c.case_hijri.Equals(case_hijri));

            if (case_id != null)
                casesSearchQuery = casesSearchQuery.Where(c => c.case_id.Equals(case_id));

            if (!String.IsNullOrEmpty(case_billing_user2))
                casesSearchQuery = casesSearchQuery.Where(c => c.case_billing_user2.Equals(case_billing_user2));

            //if (!String.IsNullOrEmpty(case_billing_user))
            //{
            //    caseLinesSearchQuery = caseLinesSearchQuery.Where(cl => cl.case_billing_user.Equals(case_billing_user));

            //var q = from cl in m_context.case_line_vws where cl.case_billing_user.ToString() == case_billing_user.ToString() select cl.case_id;
            //List<long?> records = q.ToList();

            //if (records.Count > 0)
            //    casesSearchQuery = casesSearchQuery.Where(c => records.Contains(Convert.ToInt32(c.case_id)));
            //}

            if (case_status != null)
                casesSearchQuery = casesSearchQuery.Where(c => c.case_status.Equals(case_status));

            if (case_disconnected_numbers_count != null)
                casesSearchQuery = casesSearchQuery.Where(c => c.case_disconnected_numbers_count.Equals(case_disconnected_numbers_count));

            if (!String.IsNullOrEmpty(case_Activate_date))
            {
                DateTime date = DateTime.ParseExact(case_Activate_date, "dd/MM/yyyy", null);
                casesSearchQuery = casesSearchQuery.Where(c => c.case_Activate_date.Value.Date == date);
            }

            if (case_user_id != null)
            {
                caseLinesSearchQuery = caseLinesSearchQuery.Where(cl => cl.case_user_id.Equals(case_user_id));

                //casesSearchQuery = from c in casesSearchQuery
                //                   join cl in m_context.case_line_vws on
                //                   c.case_id equals cl.case_id
                //                   where cl.case_user_id == case_user_id
                //                   select c;
            }

            var results = (from c in casesSearchQuery
                           join cl in caseLinesSearchQuery
                           on c.case_id equals cl.case_id
                           select c).Distinct();


            int pageSize = page_size.Value;
            int startIndex = (current_page.Value - 1) * pageSize;

            int skip = startIndex <= 0 ? 0 : startIndex;
            results = (from c in results
                       orderby c.case_id descending
                       select c).Skip(skip).Take(pageSize);
            int casesCount = results.Count();
            List<object> caseLines = null;

            if (casesCount > 0)
            {
                caseLines = new List<object>();
                foreach (var item in results)
                {

                    var values = from cl in m_context.case_line_vws
                                 where cl.case_id.ToString() == item.case_id.ToString()
                                 select cl;

                    caseLines.Add(values);
                }
            }

            Dictionary<string, object> Ret = new Dictionary<string, object>();
            Ret.Add("cases", results);
            Ret.Add("case_lines", caseLines);

            //var x = results.ToList();
            //var y = caseLines.ToList();

            return Json(Ret, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ExportAll(int? case_connection_domain, int? case_region, string case_city,
                    int? case_fraudStatus_id, int? case_fraudType_id, string case_customer_name,
                    int? case_customer_number, int? service_no, int? account_no, string case_ID_number,
                    int? case_nationality_id, int? case_gsm_id, int? case_bill_cycle, string case_bill_date,
                    string case_bill_hdate, string case_source, int? case_user_id, string case_date,
                    string case_date2, string case_line_date, string case_line_date2, string case_hijri, int? case_id, string case_billing_user2,
                    string case_billing_user, int? case_status, int? case_disconnected_numbers_count,
                    string case_Activate_date)
        {
            //inferes the type IQueryable<Cases_vw>
            var casesSearchQuery = from c in m_context.Case_vws select c;




            var caseLinesSearchQuery = from cl in m_context.case_line_vws select cl;

            if (case_connection_domain != null)
                caseLinesSearchQuery = caseLinesSearchQuery.Where(c => c.case_line_connection_domain.Equals(case_connection_domain));

            if (case_region != null)
                casesSearchQuery = casesSearchQuery.Where(c => c.case_region.Equals(case_region));

            if (case_city != null)
                casesSearchQuery = casesSearchQuery.Where(c => c.case_city.Equals(case_city));

            if (case_fraudStatus_id != null)
                casesSearchQuery = casesSearchQuery.Where(c => c.case_fraudStatus_id.Equals(case_fraudStatus_id));

            if (case_fraudType_id != null)
            {
                caseLinesSearchQuery = caseLinesSearchQuery.Where(cl => cl.case_fraudType_id.Equals(case_fraudType_id));


            }

            if (!String.IsNullOrEmpty(case_customer_name))
                casesSearchQuery = casesSearchQuery.Where(c => c.case_customer_name.Equals(case_customer_name));

            if (case_customer_number != null)
                casesSearchQuery = casesSearchQuery.Where(c => c.case_customer_number.Equals(case_customer_number));

            if (service_no != null)
            {
                caseLinesSearchQuery = caseLinesSearchQuery.Where(cl => cl.service_no.Equals(service_no));



            }

            if (account_no != null)
            {
                caseLinesSearchQuery = caseLinesSearchQuery.Where(cl => cl.account_no.Equals(account_no));


            }

            if (case_ID_number != null)
                casesSearchQuery = casesSearchQuery.Where(c => c.case_ID_number.Equals(case_ID_number));

            if (case_nationality_id != null)
                casesSearchQuery = casesSearchQuery.Where(c => c.case_nationality_id.Equals(case_nationality_id));

            if (case_gsm_id != null)
            {
                caseLinesSearchQuery = caseLinesSearchQuery.Where(cl => cl.case_gsm_id.Equals(case_gsm_id));


            }

            if (case_bill_cycle != null)
                casesSearchQuery = casesSearchQuery.Where(c => c.case_bill_cycle.Equals(case_bill_cycle));
            if (!String.IsNullOrEmpty(case_bill_date))
            {
                DateTime date = DateTime.ParseExact(case_bill_date, "dd/MM/yyyy", null);
                casesSearchQuery = casesSearchQuery.Where(c => c.case_bill_date.Value.Date == date);

            }

            if (!String.IsNullOrEmpty(case_bill_hdate))
                casesSearchQuery = casesSearchQuery.Where(c => c.case_bill_hdate.Equals(case_bill_hdate));

            if (!String.IsNullOrEmpty(case_source))
                casesSearchQuery = casesSearchQuery.Where(c => c.case_source.Equals(case_source));

            if (!String.IsNullOrEmpty(case_date) && String.IsNullOrEmpty(case_date2))
            {
                DateTime date = DateTime.ParseExact(case_date, "dd/MM/yyyy", null);
                casesSearchQuery = casesSearchQuery.Where(c => c.case_date.Value.Date == date);
            }

            else if (!String.IsNullOrEmpty(case_date) && !String.IsNullOrEmpty(case_date2))
            {
                DateTime start_date = DateTime.ParseExact(case_date, "dd/MM/yyyy", null);
                DateTime end_date = DateTime.ParseExact(case_date2, "dd/MM/yyyy", null);
                casesSearchQuery = casesSearchQuery.Where(c => c.case_date >= start_date.Date && c.case_date.Value.Date <= end_date.Date);
            }

            if (!String.IsNullOrEmpty(case_line_date) && String.IsNullOrEmpty(case_line_date2))
            {

                DateTime date = DateTime.ParseExact(case_line_date, "dd/MM/yyyy", null);

                caseLinesSearchQuery = caseLinesSearchQuery.Where(c => c.case_line_date.Value.Date == date);
            }

            else if (!String.IsNullOrEmpty(case_line_date) && !String.IsNullOrEmpty(case_line_date2))
            {
                DateTime start_date = DateTime.ParseExact(case_line_date, "dd/MM/yyyy", null);
                DateTime end_date = DateTime.ParseExact(case_line_date2, "dd/MM/yyyy", null);
                caseLinesSearchQuery = caseLinesSearchQuery.Where(c => c.case_line_date >= start_date.Date && c.case_line_date.Value.Date <= end_date.Date);
            }

            if (!String.IsNullOrEmpty(case_hijri))
                casesSearchQuery = casesSearchQuery.Where(c => c.case_hijri.Equals(case_hijri));

            if (case_id != null)
                casesSearchQuery = casesSearchQuery.Where(c => c.case_id.Equals(case_id));

            if (!String.IsNullOrEmpty(case_billing_user2))
                casesSearchQuery = casesSearchQuery.Where(c => c.case_billing_user2.Equals(case_billing_user2));



            if (case_status != null)
                casesSearchQuery = casesSearchQuery.Where(c => c.case_status.Equals(case_status));

            if (case_disconnected_numbers_count != null)
                casesSearchQuery = casesSearchQuery.Where(c => c.case_disconnected_numbers_count.Equals(case_disconnected_numbers_count));

            if (!String.IsNullOrEmpty(case_Activate_date))
            {
                DateTime date = DateTime.ParseExact(case_Activate_date, "dd/MM/yyyy", null);
                casesSearchQuery = casesSearchQuery.Where(c => c.case_Activate_date.Value.Date == date);
            }

            if (case_user_id != null)
            {
                caseLinesSearchQuery = caseLinesSearchQuery.Where(cl => cl.case_user_id.Equals(case_user_id));


            }

            var results = (from c in casesSearchQuery
                           join cl in caseLinesSearchQuery
                           on c.case_id equals cl.case_id
                           select c).Distinct();

            var cases = results.ToList();
            var casesNew = new List<object>();
            var cases_w_caselines = new List<object[]>();
            var headers = new List<string>();
            foreach (Case_vw c in cases)
            {
                var relatedCaseLines = caseLinesSearchQuery.Where(cl => cl.case_id.Equals(c.case_id));
                foreach (case_line_vw cl in relatedCaseLines)
                {
                    var obj = new object[2];
                    obj[0] = c;
                    obj[1] = cl;
                    cases_w_caselines.Add(obj);
                }


                //cases_w_caselines.Add(c);
                //var relatedCaseLines = caseLinesSearchQuery.Where(cl => cl.case_id.Equals(c.case_id));
                //cases_w_caselines.AddRange(relatedCaseLines.ToList());
            }

            return Json(cases_w_caselines, JsonRequestBehavior.AllowGet);
        }
    }
}
