using System;
using Newtonsoft.Json.Linq;
using System.Collections.ObjectModel;

namespace Transportation
{
    public class InputOrder : Entity
    {
        public DateTime CreatedDate { get; set; }
        public string Code { get; set; }
        public string Vendor { get; set; }
        public virtual Collection<ProductInput> ProductInputs { get; set; }
        public string Date { get; set; }
        public long TotalAmount { get; set; }

        public InputOrder() {
            ProductInputs = new Collection<ProductInput>();
        }

        public JObject ToJson()
        {
            JObject json = new JObject();
            json["id"] = ID;
            json["code"] = Code;
            json["vendor"] = Vendor;
            json["date"] = Date;
            json["totalAmount"] = TotalAmount;
            json["productInputs"] = BuildJsonArray(ProductInputs);
            return json;
        }

        public static InputOrder FromJson(JObject json)
        {
            InputOrder inputOrder = new InputOrder();
            inputOrder.ApplyJson(json);
            return inputOrder;
        }

        public void ApplyJson(JObject json)
        {
            ID = json.Value<long>("id");
            Code = json.Value<string>("code");
            Vendor = json.Value<string>("vendor");
            Date = json.Value<string>("date");
            TotalAmount = json.Value<long>("totalAmount");
        }

        private JArray BuildJsonArray(Collection<ProductInput> productInputs)
        {
            JArray jsons = new JArray();

            foreach (ProductInput productInput in productInputs)
            {
                jsons.Add(productInput.ToJson());
            }

            return jsons;
        }
    }
}
