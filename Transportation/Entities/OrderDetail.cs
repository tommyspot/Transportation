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
        public string Unit { get; set; }

        public OrderDetail() {
        }

        public JObject ToJson()
        {
            JObject json = new JObject();
            json["id"] = ID;
            json["orderId"] = OrderID;
            json["productId"] = ProductID;
            json["price"] = Price;
            json["quantity"] = Quantity;
            json["unit"] = Unit;
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
            OrderID = json.Value<long>("orderId");
            ProductID = json.Value<long>("productId");
            Price = json.Value<long>("price");
            Quantity = json.Value <long>("quantity");
            Unit = json.Value<string>("unit");
        }
    }
}
