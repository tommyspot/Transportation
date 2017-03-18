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
    public class CustomerOrder : Entity
    {
		public string CustomerName { get; set; }
		public string CustomerID { get; set; }
		public string EmployeeID { get; set; }
		public string Unit { get; set; }
        public long Quantity { get; set; }
        public string Departure { get; set; }
		public string Destination { get; set; }
		public long UnitPrice { get; set; }
		public string DepartDate { get; set; }
		public string ReturnDate { get; set; }
		public string Notes { get; set; }
		public DateTime CreatedDate { get; set; }
		public CustomerOrder() {
        }

        public JObject ToJson()
        {
            JObject json = new JObject();
			json["id"] = ID;
			json["customerName"] = CustomerName;
			json["customerId"] = CustomerID;
			json["unit"] = Unit;
            json["quantity"] = Quantity;
            json["departure"] = Departure;
            json["destination"] = Destination;
            json["unitPrice"] = UnitPrice;
			json["departDate"] = DepartDate;
			json["returnDate"] = ReturnDate;
			json["employeeId"] = EmployeeID;
			json["notes"] = Notes;
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
			Unit = json.Value<string>("unit");
			Quantity = json.Value<long>("quantity");

			Departure = json.Value<string>("departure");
			Destination = json.Value<string>("destination");
			UnitPrice = json.Value<long>("unitPrice");
			DepartDate = json.Value<string>("departDate");
			ReturnDate = json.Value<string>("returnDate");
			Notes = json.Value<string>("notes");
			EmployeeID = json.Value<string>("employeeId");
		}
    }
}
