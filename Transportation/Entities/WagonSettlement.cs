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
        public string Date { get; set; }
        [Required]
        public long CustomerOrderID { get; set; }
        public virtual CustomerOrder CustomerOrder { get; set; }
        [Required]
        public long CustomerID { get; set; }
        public virtual Customer Customer { get; set; }
        [Required]
        public long WagonID { get; set; }
        public virtual Wagon Wagon { get; set; }
        //[Required]
        //public long EmployeeID { get; set; }
        //public virtual Employee Employee { get; set; }
        public long Payment { get; set; }
        public string PaymentPlace { get; set; }
        public long PaymentRemain { get; set; }
		public string PaymentDate { get; set; }
		public string PaymentStatus { get; set; }
		public string Unit { get; set; }
        public long Quantity { get; set; }
        public string Departure { get; set; }
        public string Destination { get; set; }
        public long UnitPrice { get; set; }
		public long Discount { get; set; }
		public long TotalAmount { get; set; }
        public string Notes { get; set; }
		public string WagonCode { get; set; }

		public WagonSettlement() {
        }

        public JObject ToJson()
        {
            JObject json = new JObject();
            json["id"] = ID;
            json["code"] = Code;
            json["customerOrderId"] = CustomerOrderID;
            json["customerId"] = CustomerID;
            json["wagonId"] = WagonID;
            json["date"] = Date;
            //json["employeeId"] = EmployeeID;

            json["payment"] = Payment;
            json["paymentPlace"] = PaymentPlace;
			json["paymentDate"] = PaymentDate;
			json["paymentRemain"] = PaymentRemain;
			json["paymentStatus"] = PaymentStatus;

			json["unit"] = Unit;
            json["quantity"] = Quantity;
            json["departure"] = Departure;
            json["destination"] = Destination;
			json["discount"] = Discount;
			json["unitPrice"] = UnitPrice;
            json["totalAmount"] = TotalAmount;
            json["notes"] = Notes;
			json["wagonCode"] = WagonCode;
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
            CustomerOrderID = json.Value<long>("customerOrderId");
            CustomerID = json.Value<long>("customerId");
            WagonID = json.Value<long>("wagonId");
            Date = json.Value<string>("date");
			//EmployeeID = json.Value<long>("employeeId");

			Payment = json.Value<long>("payment");
            PaymentPlace = json.Value<string>("paymentPlace");
            PaymentRemain = json.Value<long>("paymentRemain");
			PaymentDate = json.Value<string>("paymentDate");

			Unit = json.Value<string>("unit");
            Quantity = json.Value<long>("quantity");
            Departure = json.Value<string>("departure");
            Destination = json.Value<string>("destination");

			Discount = json.Value<long>("discount");
			UnitPrice = json.Value<long>("unitPrice");
            TotalAmount = json.Value<long>("totalAmount");
            Notes = json.Value<string>("notes");
			PaymentStatus = json.Value<string>("paymentStatus");
			WagonCode = json.Value<string>("wagonCode");
		}
    }
}
