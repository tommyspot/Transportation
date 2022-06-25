using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json.Linq;

namespace Transportation
{
    public class Product : Entity
    {
        public DateTime CreatedDate { get; set; }
        [Required]
        public string Name { get; set; }
        public string Origin { get; set; }
        [NotMapped]
        public long InputPrice { get; set; }
        public long Price { get; set; }
        public Product() {
        }

        public JObject ToJson()
        {
            JObject json = new JObject();
            json["id"] = ID;
            json["name"] = Name;
            json["origin"] = Origin;
            json["inputPrice"] = InputPrice;
            json["price"] = Price;
            return json;
        }

        public static Product FromJson(JObject json)
        {
            Product product = new Product();
            product.ApplyJson(json);
            return product;
        }

        public void ApplyJson(JObject json)
        {
            ID = json.Value<long>("id");
            Name = json.Value<string>("name");
            Origin = json.Value<string>("origin");
            InputPrice = json.Value<long>("inputPrice");
            Price = json.Value<long>("price");
        }
    }
}
