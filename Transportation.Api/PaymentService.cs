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

namespace Transportation.Api
{
    public class PaymentService
	{
        public PaymentService() { }

        [Route(HttpVerb.Get, "/payments")]
        public RestApiResult GetAll()
        {
            var payments = ClarityDB.Instance.Payments;

            return new RestApiResult { StatusCode = HttpStatusCode.OK, Json = BuildJsonArray(payments) };
        }

        [Route(HttpVerb.Get, "/payments/{code}")]
        public RestApiResult GetPaymentsByWagonSettlemnetID(string code)
        {
			var payments = ClarityDB.Instance.Payments.Where(x => x.WagonSettlementCode == code) ;
			if (payments == null)
			{
				return new RestApiResult { StatusCode = HttpStatusCode.NotFound };
			}
			return new RestApiResult { StatusCode = HttpStatusCode.OK, Json = BuildJsonArray(payments) };
        }


        private JArray BuildJsonArray(IEnumerable<Payment> payments)
        {
            JArray array = new JArray();

            foreach (Payment payment in payments)
            {
                array.Add(payment.ToJson());
            }

            return array;
        }

    }
}
