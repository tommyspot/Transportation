using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json.Linq;

namespace Transportation
{
    public class OrderDetail : Entity
    {
        public DateTime CreatedDate { get; set; }
        [Required]
        public long OrderID { get; set; }
        public virtual Order Order { get; set; }
        public long ProductID { get; set; }
        public long Price { get; set; }
        public long Quantity { get; set; }
        public long TotalAmount { get; set; }
        public string Date { get; set; }

        public OrderDetail() {
        }

        public JObject ToJson()
        {
            JObject json = new JObject();
            json["id"] = ID;
            json["orderID"] = OrderID;
            json["productID"] = ProductID;
            json["price"] = Price;
            json["quantity"] = Quantity;
            json["totalAmount"] = TotalAmount;
            json["date"] = Date;
            return json;
        }

        public static OrderDetail FromJson(JObject json)
        {
            OrderDetail orderDetail = new OrderDetail();
            orderDetail.ApplyJson(json);
            return orderDetail;
        }

        public void ApplyJson(JObject json)
        {
            ID = json.Value<long>("id");
            OrderID = json.Value<long>("orderID");
            ProductID = json.Value<long>("productID");
            Price = json.Value<long>("rice");
            Quantity = json.Value <long>("quantity");
            TotalAmount = json.Value<long>("totalAmount");
            Date = json.Value<string>("date");
        }
    }
}
