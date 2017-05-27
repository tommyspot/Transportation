using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json.Linq;

namespace Transportation
{
    public class ProductInput : Entity
    {
        public DateTime CreatedDate { get; set; }
        [Required]
        public long ProductID { get; set; }
        public virtual Product Product { get; set; }
        public long Quantity { get; set; }
        public long Price { get; set; }
        public string DateInput { get; set; }

        public ProductInput() {
        }

        public JObject ToJson()
        {
            JObject json = new JObject();
            json["id"] = ID;
            json["productID"] = ProductID;
            json["quantity"] = Quantity;
            json["price"] = Price;
            json["dateInput"] = DateInput;
            return json;
        }

        public static ProductInput FromJson(JObject json)
        {
            ProductInput productInput = new ProductInput();
            productInput.ApplyJson(json);
            return productInput;
        }

        public void ApplyJson(JObject json)
        {
            ID = json.Value<long>("id");
            ProductID = json.Value<long>("productID");
            Quantity = json.Value<long>("quantity");
            Price = json.Value<long>("price");
            DateInput = json.Value<string>("dateInput");
        }
    }
}
