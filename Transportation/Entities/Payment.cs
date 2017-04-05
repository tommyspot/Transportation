using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Data;

namespace Transportation
{
    public class Payment : Entity
    {
		public string PaymentDate { get; set; }
		public long PaymentAmount { get; set; }

		[Required]
		public string WagonSettlementCode { get; set; }
		public DateTime CreatedDate { get; set; }

		public Payment(){}

		public JObject ToJson()
        {
            JObject json = new JObject();
            json["id"] = ID;
            json["paymentDate"] = PaymentDate;
            json["paymentAmount"] = PaymentAmount;
            json["wagonSettlementID"] = WagonSettlementCode;
           
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

			PaymentDate = json.Value<string>("paymentDate");
			PaymentAmount = json.Value<long>("paymentAmount");
			WagonSettlementCode = json.Value<string>("wagonSettlementID");
            
        }
    }
}
