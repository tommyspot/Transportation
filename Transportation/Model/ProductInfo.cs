using System;
using Newtonsoft.Json.Linq;

namespace Transportation
{
    public class ProductInfo
    {
        public string Name { get; set; }
        public double Price { get; set; }
        public double SumOfInput { get; set; }
        public double SumOfInputTotalAmount { get; set; }
        public double SumOfSale { get; set; }
        public double SumOfSaleTotalAmount { get; set; }
        public double NumOfRemain { get; set; }
        public double Profit { get; set; }
        public ProductInfo() {
        }

        public JObject ToJson()
        {
            JObject json = new JObject();
            json["name"] = Name;
            json["price"] = Price;
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
            Price = json.Value<double>("price");
            SumOfInput = json.Value <double>("sumOfInput");
            SumOfInputTotalAmount = json.Value<double>("sumOfInputTotalAmount");
            SumOfSale = json.Value<double>("sumOfSale");
            SumOfSaleTotalAmount = json.Value<double>("sumOfSaleTotalAmount");
            NumOfRemain = json.Value<double>("numOfRemain");
            Profit = json.Value<double>("profit");
        }
    }
}
