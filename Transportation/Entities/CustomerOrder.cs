using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Data;
using System.Collections.ObjectModel;

namespace Transportation
{
    public class CustomerOrder : Entity
    {
		public string CustomerName { get; set; }
		public string CustomerID { get; set; }
        public string CustomerPhone { get; set; }
        public string CustomerArea { get; set; }
		public string CustomerCode { get; set; }
		public long CreatedUserID { get; set; }
		public string Unit { get; set; }
        public long Quantity { get; set; }
        public string Departure { get; set; }
		public string Destination { get; set; }
		public long UnitPrice { get; set; }
		public long TotalPay { get; set; }
		public DateTime DepartDate { get; set; }
		public DateTime ReturnDate { get; set; }
		public string Notes { get; set; }
		public DateTime CreatedDate { get; set; }
        public CustomerOrder() {
        }

        public JObject ToJson()
        {
            JObject json = new JObject();
			json["id"] = ID;
			json["customerName"] = CustomerName;
            json["customerPhone"] = CustomerPhone;
            json["customerArea"] = CustomerArea;
            json["customerId"] = CustomerID;
			json["unit"] = Unit;
            json["quantity"] = Quantity;
            json["departure"] = Departure;
            json["destination"] = Destination;
            json["unitPrice"] = UnitPrice;
			json["departDate"] = DepartDate;
			json["returnDate"] = ReturnDate;
			json["createdUserId"] = CreatedUserID;
			json["notes"] = Notes;
            json["createdDate"] = CreatedDate;
			json["totalPay"] = TotalPay;
			json["customerCode"] = CustomerCode;
			return json;
        }

        public static CustomerOrder FromJson(JObject json)
        {
            CustomerOrder customerOrder = new CustomerOrder();
            customerOrder.ApplyJson(json);

            return customerOrder;
        }

		public void ApplyJson(JObject json)
		{
			ID = json.Value<long>("id");
			CustomerName = json.Value<string>("customerName");
			CustomerID = json.Value<string>("customerId");
			CustomerCode = json.Value<string>("customerCode");
			CustomerPhone = json.Value<string>("customerPhone");
			CustomerArea = json.Value<string>("customerArea");
			Unit = json.Value<string>("unit");
			Quantity = json.Value<long>("quantity");
			Departure = json.Value<string>("departure");
			Destination = json.Value<string>("destination");
			UnitPrice = json.Value<long>("unitPrice");
			TotalPay = json.Value<long>("totalPay");
			DepartDate = json.Value<DateTime>("departDate");
			ReturnDate = json.Value<DateTime>("returnDate");
			Notes = json.Value<string>("notes");
			
		}
    }
}
