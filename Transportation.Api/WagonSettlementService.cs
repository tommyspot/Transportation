using Transportation.Api.Framework;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data;
using System.Data.Entity.Validation;
using Newtonsoft.Json;

namespace Transportation.Api
{
    public class WagonSettlementService
    {
        const string formatDate = "dd/MM/yyyy";
        public WagonSettlementService() { }

        [Route(HttpVerb.Get, "/wagonSettlements")]
        public RestApiResult GetAll()
        {
            var wagonSettlements = ClarityDB.Instance.WagonSettlements;

            return new RestApiResult { StatusCode = HttpStatusCode.OK, Json = BuildJsonArray(wagonSettlements) };
        }

        [Route(HttpVerb.Get, "/wagonSettlementReportByDate")]
        public RestApiResult GetWagonSettlementReportByDate(string date)
        {
            var dateJSON = JsonConvert.DeserializeObject<JObject>(date);
            DateTime fromDate = DateTime.ParseExact(dateJSON.Value<string>("fromDate"), formatDate, CultureInfo.InvariantCulture);
            DateTime toDate = DateTime.ParseExact(dateJSON.Value<string>("toDate"), formatDate, CultureInfo.InvariantCulture);

            List<WagonSettlement> wagonSettlements = ClarityDB.Instance.WagonSettlements.ToList();
            List<WagonSettlement> filteredWagonSettlements = new List<WagonSettlement>();

            foreach (WagonSettlement wagonSettlement in wagonSettlements)
            {
                if(wagonSettlement.PaymentDate != null)
                {
                    DateTime paymentDate = DateTime.ParseExact(wagonSettlement.PaymentDate, formatDate, CultureInfo.InvariantCulture);
                    if (DateTime.Compare(paymentDate, fromDate) >= 0 && DateTime.Compare(paymentDate, toDate) <= 0)
                    {
                        filteredWagonSettlements.Add(wagonSettlement);
                    }
                }
            }

            var wagonSettlementReportDataList = filteredWagonSettlements.Select(w => new WagonSettlementReportData
            {
                Code = w.Code,
                TotalAmount = w.Quantity * w.UnitPrice,
                TotalPayment = 100, //waiting confirm
                Profit = 80         //TotalAmount - TotalPayment
            }).ToList();

            return new RestApiResult { StatusCode = HttpStatusCode.OK, Json = BuildJsonReportDataArray(wagonSettlementReportDataList) };
        }

        [Route(HttpVerb.Post, "/wagonSettlements")]
        public RestApiResult Create(JObject json)
        {
            if (json == null)
            {
                return new RestApiResult { StatusCode = HttpStatusCode.BadRequest };
            }

            WagonSettlement wagonSettlement = WagonSettlement.FromJson(json);
            wagonSettlement.CreatedDate = DateTime.Now;

            ClarityDB.Instance.WagonSettlements.Add(wagonSettlement);
            ClarityDB.Instance.SaveChanges();

            return new RestApiResult { StatusCode = HttpStatusCode.OK };
        }

        [Route(HttpVerb.Get, "/wagonSettlements/{id}")]
        public RestApiResult GetWagonByID(long id)
        {
            WagonSettlement wagonSettlement = ClarityDB.Instance.WagonSettlements.FirstOrDefault(x => x.ID == id);

            if (wagonSettlement == null)
            {
                return new RestApiResult { StatusCode = HttpStatusCode.NotFound };
            }

            return new RestApiResult { StatusCode = HttpStatusCode.OK, Json = wagonSettlement.ToJson() };
        }

        [Route(HttpVerb.Delete, "/wagonSettlements/{id}")]
        public RestApiResult Delete(long id)
        {
            WagonSettlement wagonSettlement = ClarityDB.Instance.WagonSettlements.FirstOrDefault(x => x.ID == id);

            if (wagonSettlement == null) {
                return new RestApiResult { StatusCode = HttpStatusCode.NotFound };
            }

            ClarityDB.Instance.WagonSettlements.Remove(wagonSettlement);
            ClarityDB.Instance.SaveChanges();

            return new RestApiResult { StatusCode = HttpStatusCode.OK, Json = wagonSettlement.ToJson() };
        }

        [Route(HttpVerb.Put, "/wagonSettlements/{id}")]
        public RestApiResult Update(long id, JObject json)
        {
            WagonSettlement wagonSettlement = ClarityDB.Instance.WagonSettlements.FirstOrDefault(x => x.ID == id);

            if (wagonSettlement == null) {
                return new RestApiResult { StatusCode = HttpStatusCode.NotFound };
            }

            wagonSettlement.ApplyJson(json);
			var newPayment  = json.Value<long>("newPayment");
			if (newPayment > 0 && !String.IsNullOrEmpty(wagonSettlement.PaymentDate))
			{
				wagonSettlement.Payment += newPayment;
				wagonSettlement.PaymentRemain -= newPayment;
                if (wagonSettlement.PaymentRemain == 0)
                {
                    wagonSettlement.PaymentStatus = "Không Nợ";
                }

                Payment payment = new Payment();
				payment.PaymentDate = wagonSettlement.PaymentDate;
				payment.PaymentAmount = newPayment;
				payment.WagonSettlementCode = wagonSettlement.Code;
				payment.CreatedDate = DateTime.Now;
				ClarityDB.Instance.Payments.Add(payment);

				//Customer customer = ClarityDB.Instance.Customers.FirstOrDefault(x => x.ID == wagonSettlement.CustomerID);
				//customer.NeedUpdatePayment = true;
			}

			ClarityDB.Instance.SaveChanges();
            return new RestApiResult { StatusCode = HttpStatusCode.OK, Json = json};
        }

        private JArray BuildJsonArray(IEnumerable<WagonSettlement> wagonSettlements)
        {
            JArray array = new JArray();

            foreach (WagonSettlement wagonSettlement in wagonSettlements)
            {
                array.Add(wagonSettlement.ToJson());
            }

            return array;
        }

        private JArray BuildJsonReportDataArray(List<WagonSettlementReportData> wagonSettlementReportDataList)
        {
            JArray array = new JArray();

            foreach (WagonSettlementReportData wagonSettlementReport in wagonSettlementReportDataList)
            {
                array.Add(wagonSettlementReport.ToJson());
            }

            return array;
        }

    }
}
