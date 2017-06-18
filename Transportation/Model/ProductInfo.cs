using System;
using Newtonsoft.Json.Linq;

namespace Transportation
{
    public class ProductInfo
    {
        public string Name { get; set; }
        public long SumOfInput { get; set; }
        public long SumOfInputTotalAmount { get; set; }
        public long SumOfSale { get; set; }
        public long SumOfSaleTotalAmount { get; set; }
        public long NumOfRemain { get; set; }
        public long Profit { get; set; }
        public ProductInfo() {
        }

        public JObject ToJson()
        {
            JObject json = new JObject();
            json["name"] = Name;
            json["sumOfInput"] = SumOfInput;
            json["sumOfInputTotalAmount"] = SumOfInputTotalAmount;
            json["sumOfSale"] = SumOfSale;
            json["sumOfSaleTotalAmount"] = SumOfSaleTotalAmount;
            json["numOfRemain"] = NumOfRemain;
            json["profit"] = Profit;
            return json;
        }

        public static ProductInfo FromJson(JObject json)
        {
            ProductInfo productInfo = new ProductInfo();
            productInfo.ApplyJson(json);
            return productInfo;
        }

        public void ApplyJson(JObject json)
        {
            Name = json.Value<string>("name");
            SumOfInput = json.Value <long>("sumOfInput");
            SumOfInputTotalAmount = json.Value<long>("sumOfInputTotalAmount");
            SumOfSale = json.Value<long>("sumOfSale");
            SumOfSaleTotalAmount = json.Value<long>("sumOfSaleTotalAmount");
            NumOfRemain = json.Value<long>("numOfRemain");
            Profit = json.Value<long>("profit");
        }
    }
}
