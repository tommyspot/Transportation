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
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public long CustomerID { get; set; }
        public virtual Customer Customer { get; set; }
        [Required]
        public long WagonID { get; set; }
        public virtual Wagon Wagon { get; set; }
        [Required]
        public long EmployeeID { get; set; }
        public virtual Employee Employee { get; set; }

        public long Payment { get; set; }

        public string Unit { get; set; }
        public long Quantity { get; set; }
        public string Departure { get; set; }
        public string Destination { get; set; }
        public long UnitPrice { get; set; }
        public long TotalAmount { get; set; }
        public string Notes { get; set; }

        public WagonSettlement() {
        }

        public JObject ToJson()
        {
            JObject json = new JObject();
            json["id"] = ID;
            json["customerId"] = CustomerID;
            json["wagonId"] = WagonID;
            json["date"] = Date;
            json["employeeId"] = EmployeeID;

            json["payment"] = Payment;

            json["unit"] = Unit;
            json["quantity"] = Quantity;
            json["departure"] = Departure;
            json["destination"] = Destination;

            json["unitPrice"] = UnitPrice;
            json["totalAmount"] = TotalAmount;
            json["notes"] = Notes;
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

            CustomerID = json.Value<long>("customerId");
            WagonID = json.Value<long>("wagonId");
            Date = json.Value<DateTime>("date");
            EmployeeID = json.Value<long>("employeeId");

            Payment = json.Value<long>("payment");
            Unit = json.Value<string>("unit");
            Quantity = json.Value<long>("quantity");
            Departure = json.Value<string>("departure");
            Destination = json.Value<string>("destination");

            UnitPrice = json.Value<long>("unitPrice");
            TotalAmount = json.Value<long>("totalAmount");
            Notes = json.Value<string>("notes");
        }
    }
}
