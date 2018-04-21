using Transportation.Api.Framework;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Data;

namespace Transportation.Api
{
    public class CustomerService
    {

        public CustomerService() { }

        [Route(HttpVerb.Get, "/customers")]
        public RestApiResult GetAll()
        {
            var customers = ClarityDB.Instance.Customers;

            return new RestApiResult { StatusCode = HttpStatusCode.OK, Json = BuildJsonArray(customers) };
        }

        [Route(HttpVerb.Get, "/customers/curtail")]
        public RestApiResult GetAllCurtail()
        {
            var customers = ClarityDB.Instance.Customers;

            return new RestApiResult { StatusCode = HttpStatusCode.OK, Json = BuildJsonArrayCurtail(customers) };
        }

        [Route(HttpVerb.Get, "/customers/page")]
        public RestApiResult GetPerPage(string pageIndex, string pageSize, string search)
        {
            int index = Int32.Parse(pageIndex);
            int size = Int32.Parse(pageSize);
            int startIndex = index * size;

            var customers = ClarityDB.Instance.Customers
                .Where(x => String.IsNullOrEmpty(search)
                            || x.FullName.IndexOf(search) > -1
                            || x.PhoneNo.IndexOf(search) > -1
                            || x.Code.IndexOf(search) > -1)
                .OrderByDescending(x => x.ID)
                .Skip(startIndex)
                .Take(size);
            return new RestApiResult { StatusCode = HttpStatusCode.OK, Json = BuildJsonArray(customers) };
        }

        [Route(HttpVerb.Get, "/customers/numberOfPages")]
        public RestApiResult GetNumberPage(string pageSize, string search)
        {
            int size = Int32.Parse(pageSize);
            var allRecords = ClarityDB.Instance.Customers
                .Where(x => String.IsNullOrEmpty(search)
                            || x.FullName.IndexOf(search) > -1
                            || x.PhoneNo.IndexOf(search) > -1
                            || x.Code.IndexOf(search) > -1)
                .Count();
            int numOfPages = allRecords % size == 0
                ? allRecords / size
                : allRecords / size + 1;

            JObject json = JObject.Parse(@"{'pages': '" + numOfPages + "'}");
            return new RestApiResult { StatusCode = HttpStatusCode.OK, Json = json };
        }

        [Route(HttpVerb.Post, "/customers")]
        public RestApiResult Create(JObject json)
        {
            if (json == null)
            {
                return new RestApiResult { StatusCode = HttpStatusCode.BadRequest };
            }

            Customer customer = Customer.FromJson(json);
            customer.CreatedDate = DateTime.Now;

            ClarityDB.Instance.Customers.Add(customer);
            ClarityDB.Instance.SaveChanges();

            return new RestApiResult { StatusCode = HttpStatusCode.OK, Json = customer.ToJson() };
        }

        [Route(HttpVerb.Get, "/customers/{id}")]
        public RestApiResult GetCustomerByID(long id)
        {
            Customer customer = ClarityDB.Instance.Customers.FirstOrDefault(x => x.ID == id);

            if (customer == null)
            {
                return new RestApiResult { StatusCode = HttpStatusCode.NotFound };
            }

            return new RestApiResult { StatusCode = HttpStatusCode.OK, Json = BuildJsonCustomer(customer) };
        }

        [Route(HttpVerb.Delete, "/customers/{id}")]
        public RestApiResult Delete(long id)
        {
            Customer customer = ClarityDB.Instance.Customers.FirstOrDefault(x => x.ID == id);

            if (customer == null)
            {
                return new RestApiResult { StatusCode = HttpStatusCode.NotFound };
            }

            ClarityDB.Instance.Customers.Remove(customer);
            ClarityDB.Instance.SaveChanges();

            return new RestApiResult { StatusCode = HttpStatusCode.OK, Json = customer.ToJson() };
        }

        [Route(HttpVerb.Put, "/customers/{id}")]
        public RestApiResult Update(long id, JObject json)
        {
            Customer customer = ClarityDB.Instance.Customers.FirstOrDefault(x => x.ID == id);

            if (customer == null)
            {
                return new RestApiResult { StatusCode = HttpStatusCode.NotFound };
            }

            customer.ApplyJson(json);
            ClarityDB.Instance.SaveChanges();
            // Add new payment for Customer
            AddNewPayment(id, json);

            return new RestApiResult { StatusCode = HttpStatusCode.OK, Json = json };
        }

        [Route(HttpVerb.Post, "/customers/fastPayment/{id}")]
        public RestApiResult ProccessFastPayment(long id)
        {
            Customer customer = ClarityDB.Instance.Customers.FirstOrDefault(x => x.ID == id);

            if (customer == null)
            {
                return new RestApiResult { StatusCode = HttpStatusCode.NotFound };
            }

            long totalOwned = GetTotalOwnedByCustomerID(id);
            long totalPay = GetTotalPaymentByCustomerID(id);
            long totalDebt = totalOwned - totalPay;

            if (totalDebt <= 0) {
                string errorJson = "{ 'message': 'Khách hàng không còn nợ' }";
                return new RestApiResult { StatusCode = HttpStatusCode.Conflict, Json = JObject.Parse(errorJson) };
            }

            JObject json = new JObject();
            json["newPayment"] = totalDebt;
            json["paymentMonth"] = DateTime.Now.Month;
            json["paymentYear"] = DateTime.Now.Year;
            AddNewPayment(id, json);

            return new RestApiResult { StatusCode = HttpStatusCode.OK, Json = json };
        }

        private void AddNewPayment(long cusomerId, JObject json)
        {
            long newPayment = json.Value<long>("newPayment");
            int? month = json.Value<int?>("paymentMonth");
            int? year = json.Value<int?>("paymentYear");
            if (newPayment > 0 && month != null && year != null) {
                Payment payment = new Payment();
                payment.CustomerID = cusomerId;
                payment.CreatedDate = DateTime.Now;
                payment.PaymentMonth = month.Value;
                payment.PaymentYear = year.Value;
                payment.PaymentAmount = newPayment;
                ClarityDB.Instance.Payments.Add(payment);
                ClarityDB.Instance.SaveChanges();
            }
        }

        private JObject BuildJsonCustomer(Customer customer)
        {
            customer.TotalOwned = GetTotalOwnedByCustomerID(customer.ID);
            customer.TotalPay = GetTotalPaymentByCustomerID(customer.ID);
            customer.TotalDebt = customer.TotalOwned - customer.TotalPay;
            return customer.ToJson();
        }

        private long GetTotalOwnedByCustomerID(long id)
        {
            var wagonSettlements = ClarityDB.Instance.WagonSettlements.Where(x => x.CustomerID == id);
            return wagonSettlements.Count() > 0 ? wagonSettlements.Sum(x => x.Quantity * x.UnitPrice + x.PhiPhatSinh) : 0;
        }
        private long GetTotalPaymentByCustomerID(long id)
        {
            // Get from WagonSettlement
            var wagonSettlements = ClarityDB.Instance.WagonSettlements.Where(x => x.CustomerID == id);
            long totalPaymentFromWagonSettlement = wagonSettlements.Count() > 0 ? wagonSettlements.Sum(x => x.Payment) : 0;
            // Get from Payment
            var payments = ClarityDB.Instance.Payments.Where(x => x.CustomerID == id);
            long totalPaymentFromPayment = payments.Count() > 0 ? payments.Sum(x => x.PaymentAmount) : 0;
            return (totalPaymentFromWagonSettlement + totalPaymentFromPayment);
        }

        private JArray BuildJsonArray(IEnumerable<Customer> customers)
        {
            JArray jArray = new JArray();

            foreach (Customer customer in customers)
            {
                JObject json = BuildJsonCustomer(customer);
                jArray.Add(json);
            }

            return jArray;
        }
        private JArray BuildJsonArrayCurtail(IEnumerable<Customer> customers)
        {
            JArray jArray = new JArray();
            foreach (Customer customer in customers)
            {
                JObject json = new JObject();
                json["id"] = customer.ID;
                json["fullName"] = customer.FullName;
                jArray.Add(json);
            }
            return jArray;
        }
    }
}
