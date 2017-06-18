using System;
using Newtonsoft.Json.Linq;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace Transportation
{
    public class Order : Entity
    {
        public DateTime CreatedDate { get; set; }
        public string Code { get; set; }
        [Required]
        public string CustomerName { get; set; }
        [Required]
        public string Mobile { get; set; }
        [Required]
        public string LicensePlate { get; set; }
        public string CardID { get; set; }
        public string Address { get; set; }
        public string EmployeeName { get; set; }
        public virtual Collection<OrderDetail> OrderDetails { get; set; }
        public string Date { get; set; }
        public long SaleOff { get; set; }
        public long TotalAmount { get; set; }
        public string Note { get; set; }

        public Order() {
            OrderDetails = new Collection<OrderDetail>();
        }

        public JObject ToJson()
        {
            JObject json = new JObject();
            json["id"] = ID;
            json["code"] = Code;
            json["customerName"] = CustomerName;
            json["mobile"] = Mobile;
            json["licensePlate"] = LicensePlate;
            json["cardId"] = CardID;
            json["address"] = Address;
            json["employeeName"] = EmployeeName;
            json["date"] = Date;
            json["saleOff"] = SaleOff;
            json["totalAmount"] = TotalAmount;
            json["orderDetails"] = BuildJsonArray(OrderDetails);
            json["note"] = Note;
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
            Code = json.Value<string>("code");
            CustomerName = json.Value<string>("customerName");
            Mobile = json.Value<string>("mobile");
            LicensePlate = json.Value<string>("licensePlate");
            CardID = json.Value<string>("cardId");
            Address = json.Value<string>("address");
            EmployeeName = json.Value<string>("employeeName");
            Date = json.Value<string>("date");
            SaleOff = json.Value <long>("saleOff");
            TotalAmount = json.Value<long>("totalAmount");
            Note = json.Value<string>("note");
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
