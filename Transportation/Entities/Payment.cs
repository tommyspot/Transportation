using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json.Linq;

namespace Transportation
{
    public class Payment : Entity
    {
        public DateTime CreatedDate { get; set; }
        [Required]
        public long CustomerID { get; set; }
        public virtual Customer Customer { get; set; }
        public int PaymentMonth { get; set; }
        public int PaymentYear { get; set; }
        public long PaymentAmount { get; set; }

		public Payment(){}

		public JObject ToJson()
        {
            JObject json = new JObject();
            json["id"] = ID;
            json["customerID"] = CustomerID;
            json["paymentMonth"] = PaymentMonth;
            json["paymentYear"] = PaymentYear;
            json["paymentAmount"] = PaymentAmount;
           
			return json;
        }

        public static Payment FromJson(JObject json)
        {
            Payment payment = new Payment();
			payment.ApplyJson(json);

            return payment;
        }

        public void ApplyJson(JObject json)
        {
            ID = json.Value<long>("id");
            CustomerID = json.Value<long>("customerID");
            PaymentMonth = json.Value<int>("paymentMonth");
            PaymentYear = json.Value<int>("paymentYear");
            PaymentAmount = json.Value<long>("paymentAmount");
        }
    }
}
