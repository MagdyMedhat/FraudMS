using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FraudMS.Components.Stats
{
    public class StatsController : Controller
    {
        RAFMSDataContext m_context;

        public StatsController()
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
            return View("~/Components/Stats/Stats.cshtml");
        }

        //public JsonResult Search(int? case_connection_domain, int? case_user_id, string case_date, string case_date2)
        public JsonResult Search(int? case_connection_domain, int? case_user_id, string case_date, string case_date2)
        {
            //inferes the type IQueryable<Cases_vw>
            var casesSearchQuery = from c in m_context.Case_vws select c;

            //if (case_connection_domain != null)
            //    casesSearchQuery = casesSearchQuery.Where(c => c.case_connection_domain.Equals(case_connection_domain));



            if (!String.IsNullOrEmpty(case_date) && String.IsNullOrEmpty(case_date2))
            {
                DateTime date = DateTime.ParseExact(case_date, "dd/MM/yyyy", null);
                casesSearchQuery = casesSearchQuery.Where(c => c.case_date.Value.Date == date);
            }

            else if (!String.IsNullOrEmpty(case_date) && !String.IsNullOrEmpty(case_date2))
            {
                DateTime start_date = DateTime.ParseExact(case_date, "dd/MM/yyyy", null);
                DateTime end_date = DateTime.ParseExact(case_date2, "dd/MM/yyyy", null);
                casesSearchQuery = casesSearchQuery.Where(c => c.case_date >= start_date && c.case_date <= end_date);
            }

            if (case_user_id != null)
            {
                casesSearchQuery = from c in casesSearchQuery
                                   join cl in m_context.case_lines on
                                   c.case_id equals cl.case_id
                                   where cl.case_user_id == case_user_id
                                   select c;
            }

            /* case_line_connection_domain has no data after migration*/

            var x = casesSearchQuery.ToList();
            if (case_connection_domain != null)
            {
                casesSearchQuery = from c in casesSearchQuery
                                   join c1 in m_context.case_lines on
                                   c.case_id equals c1.case_id
                                   where c1.case_line_connection_domain == case_connection_domain
                                   select c;
            }
            int casesCount = casesSearchQuery.Count();
            decimal? totalActualMoney = 0;
            decimal? totalEstimatedMoney = 0;
            int totalDisconnectedNumbers = 0;
            int totalOpenedCases = 0;
            int totalClosedCases = 0;

            if (casesCount > 0)
            {
                totalActualMoney = casesSearchQuery.Sum(t => t.case_actual_loss);
                totalEstimatedMoney = casesSearchQuery.Sum(t => t.case_estimated_loss);
                totalDisconnectedNumbers = Convert.ToInt32(casesSearchQuery.Sum(t => t.case_disconnected_numbers_count));
                totalOpenedCases = casesSearchQuery.Count(t => t.case_status == 1);
                totalClosedCases = casesCount - totalOpenedCases;
            }

            if (totalActualMoney == null)
                totalActualMoney = 0;

            if (totalEstimatedMoney == null)
                totalEstimatedMoney = 0;

            if (totalDisconnectedNumbers == null)
                totalDisconnectedNumbers = 0;

            if (totalOpenedCases == null)
                totalOpenedCases = 0;

            if (totalClosedCases == null)
                totalClosedCases = 0;


            Dictionary<string, object> Ret = new Dictionary<string, object>();
            Ret.Add("cases_count", casesCount);
            Ret.Add("actual_loss", totalActualMoney);
            Ret.Add("estimated_loss", totalEstimatedMoney);
            Ret.Add("disconnected_numbers", totalDisconnectedNumbers);
            Ret.Add("opened_cases", totalOpenedCases);
            Ret.Add("closed_cases", totalClosedCases);

            var fraudStatusQuery = from cases in casesSearchQuery
                                   group cases by cases.fraudStatus_name into grouping
                                   select new { fraud_status = grouping.Key, count = grouping.Count() };

            var fraudTypeQuery = from cl in m_context.case_line_vws
                                 join c in casesSearchQuery
                                 on cl.case_id equals c.case_id
                                 group cl by cl.fraudType_name
                                     into fraud
                                     select new { fraud_type = fraud.Key, count = fraud.Count() };

            Ret.Add("fraud_status_stats", fraudStatusQuery);
            Ret.Add("fraud_type_stats", fraudTypeQuery);

            return Json(Ret, JsonRequestBehavior.AllowGet);
        }
    }
}
