using Transportation.Api.Framework;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Data;

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

        [Route(HttpVerb.Get, "/payments/{id}")]
        public RestApiResult GetPaymentsByCustomerID(long id)
        {
			var payments = ClarityDB.Instance.Payments.Where(x => x.CustomerID == id) ;
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
