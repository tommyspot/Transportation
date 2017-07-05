using System;
using Newtonsoft.Json.Linq;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace Transportation
{
    public class Order : Entity
    {
        public DateTime CreatedDate { get; set; }
        [Required]
        public string LicensePlate { get; set; }
        public string CustomerName { get; set; }
        public string Address { get; set; }
        public virtual Collection<OrderDetail> OrderDetails { get; set; }
        public string Date { get; set; }
        public long SaleOff { get; set; }
        public long TotalAmount { get; set; }
        public string Note { get; set; }
        public bool Status { get; set; }

        public Order() {
            OrderDetails = new Collection<OrderDetail>();
        }

        public JObject ToJson()
        {
            JObject json = new JObject();
            json["id"] = ID;
            json["customerName"] = CustomerName;
            json["licensePlate"] = LicensePlate;
            json["address"] = Address;
            json["date"] = Date;
            json["saleOff"] = SaleOff;
            json["totalAmount"] = TotalAmount;
            json["orderDetails"] = BuildJsonArray(OrderDetails);
            json["note"] = Note;
            json["status"] = Status;
            return json;
        }

        public static Order FromJson(JObject json)
        {
            Order order = new Order();
            order.ApplyJson(json);
            return order;
        }

        public void ApplyJson(JObject json)
        {
            ID = json.Value<long>("id");
            CustomerName = json.Value<string>("customerName");
            LicensePlate = json.Value<string>("licensePlate");
            Address = json.Value<string>("address");
            Date = json.Value<string>("date");
            SaleOff = json.Value <long>("saleOff");
            TotalAmount = json.Value<long>("totalAmount");
            Note = json.Value<string>("note");
            Status = json.Value<bool>("status");
        }

        private JArray BuildJsonArray(Collection<OrderDetail> orderDetails)
        {
            JArray jsons = new JArray();

            foreach (OrderDetail orderDetail in orderDetails)
            {
                jsons.Add(orderDetail.ToJson());
            }

            return jsons;
        }
    }
}
