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
    public class WagonSettlement : Entity
    {
        public DateTime CreatedDate { get; set; }
        public string Code { get; set; }
        [Required]
        public long CustomerID { get; set; }
        public virtual Customer Customer { get; set; }
        [Required]
        public long WagonID { get; set; }
        public virtual Wagon Wagon { get; set; }
        public long Payment { get; set; }
        public string PaymentPlace { get; set; }
        public long PaymentRemain { get; set; }
		public string PaymentDate { get; set; }
		public string PaymentStatus { get; set; }
		public string Unit { get; set; }
        public long Quantity { get; set; }
        public string Destination { get; set; }
        public long UnitPrice { get; set; }
		public long TotalAmount { get; set; }
        public long PhiPhatSinh { get; set; }
        public string LyDoPhatSinh { get; set; }

        public WagonSettlement() {
        }

        public JObject ToJson()
        {
            JObject json = new JObject();
            json["id"] = ID;
            json["code"] = Code;
            json["customerId"] = CustomerID;
            json["wagonId"] = WagonID;

            json["payment"] = Payment;
            json["paymentPlace"] = PaymentPlace;
			json["paymentDate"] = PaymentDate;
			json["paymentRemain"] = PaymentRemain;
			json["paymentStatus"] = PaymentStatus;
			json["unit"] = Unit;
            json["quantity"] = Quantity;
            json["destination"] = Destination;
			json["unitPrice"] = UnitPrice;
            json["totalAmount"] = TotalAmount;

            json["phiPhatSinh"] = PhiPhatSinh;
            json["lyDoPhatSinh"] = LyDoPhatSinh;
            return json;
        }

        public static WagonSettlement FromJson(JObject json)
        {
            WagonSettlement wagon = new WagonSettlement();
            wagon.ApplyJson(json);
            return wagon;
        }

        public void ApplyJson(JObject json)
        {
            ID = json.Value<long>("id");

            Code = json.Value<string>("code");
            CustomerID = json.Value<long>("customerId");
            WagonID = json.Value<long>("wagonId");

			Payment = json.Value<long>("payment");
            PaymentPlace = json.Value<string>("paymentPlace");
            PaymentRemain = json.Value<long>("paymentRemain");
			PaymentDate = json.Value<string>("paymentDate");
            PaymentStatus = json.Value<string>("paymentStatus");

            Unit = json.Value<string>("unit");
            Quantity = json.Value<long>("quantity");
            Destination = json.Value<string>("destination");
			
			UnitPrice = json.Value<long>("unitPrice");
            TotalAmount = json.Value<long>("totalAmount");
            PhiPhatSinh = json.Value<long>("phiPhatSinh");
            LyDoPhatSinh = json.Value<string>("lyDoPhatSinh");
        }
    }
}
