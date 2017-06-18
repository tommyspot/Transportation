using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json.Linq;
using System.ComponentModel.DataAnnotations.Schema;

namespace Transportation
{
    public class ProductInput : Entity
    {
        public DateTime CreatedDate { get; set; }
        [Required]
        public long ProductID { get; set; }
        [Required]
        public long InputOrderID { get; set; }
        public virtual InputOrder InputOrder { get; set; }
        public virtual Product Product { get; set; }
        public long Quantity { get; set; }
        public long InputPrice { get; set; }
        public long SalePrice { get; set; }

        public ProductInput() {
        }

        public JObject ToJson()
        {
            JObject json = new JObject();
            json["id"] = ID;
            json["productId"] = ProductID;
            json["inputOrderID"] = InputOrderID;
            json["quantity"] = Quantity;
            json["inputPrice"] = InputPrice;
            json["salePrice"] = SalePrice;
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
            ProductID = json.Value<long>("productId");
            InputOrderID = json.Value<long>("inputOrderID");
            Quantity = json.Value<long>("quantity");
            InputPrice = json.Value<long>("inputPrice");
            SalePrice = json.Value<long>("salePrice");
        }
    }
}
