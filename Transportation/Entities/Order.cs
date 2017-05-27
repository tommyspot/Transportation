using System;
using Newtonsoft.Json.Linq;
using System.Collections.ObjectModel;

namespace Transportation
{
    public class Order : Entity
    {
        public DateTime CreatedDate { get; set; }
        public string Code { get; set; }
        public string CustomerName { get; set; }
        public string EmployeeName { get; set; }
        public virtual Collection<OrderDetail> OrderDetails { get; set; }
        public string Date { get; set; }
        public long SaleOff { get; set; }
        public long TotalAmount { get; set; }

        public Order() {
            OrderDetails = new Collection<OrderDetail>();
        }

        public JObject ToJson()
        {
            JObject json = new JObject();
            json["id"] = ID;
            json["code"] = Code;
            json["customerName"] = CustomerName;
            json["employeeName"] = EmployeeName;
            json["date"] = Date;
            json["saleOff"] = SaleOff;
            json["totalAmount"] = TotalAmount;
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
            EmployeeName = json.Value<string>("employeeName");
            Date = json.Value<string>("date");
            SaleOff = json.Value <long>("saleOff");
            TotalAmount = json.Value<long>("totalAmount");
        }
    }
}
