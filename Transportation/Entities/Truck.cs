using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Data;

namespace Transportation
{
    public class Truck : Entity
    {
        [Required]
        public string Code { get; set; }
        [Required]
        public string LicensePlate { get; set; }
        public string Vin { get; set; }
        public string EngineNo { get; set; }
        public string YearOfManufacture { get; set; }
        public string Brand { get; set; }
        public string Weight { get; set; }
        public string StartUsingDate { get; set; }
        public string DriverId { get; set; }
        public string Stock { get; set; }
        public string BuyingDate { get; set; }
        public string MonthlyPayment { get; set; }
        public string CheckDate { get; set; }
		public string InsuranceDate { get; set; }

		public Truck() {
        }

        public JObject ToJson()
        {
            JObject json = new JObject();
            json["id"] = ID;
			json["code"] = Code;
			json["licensePlate"] = LicensePlate;
            json["vin"] = Vin;
            json["engineNo"] = EngineNo;
            json["yearOfManufacture"] = YearOfManufacture;

            json["brand"] = Brand;
            json["weight"] = Weight;
            json["startUsingDate"] = StartUsingDate;
            json["driverId"] = DriverId;
            json["stock"] = Stock;

            json["buyingDate"] = BuyingDate;
            json["monthlyPayment"] = MonthlyPayment;
            json["checkDate"] = CheckDate;
            json["insuranceDate"] = InsuranceDate;

            return json;
        }

        public static Truck FromJson(JObject json)
        {
            Truck truck = new Truck();
            truck.ApplyJson(json);

            return truck;
        }

        public void ApplyJson(JObject json)
        {
            ID = json.Value<long>("id");

            Code = json.Value<string>("code");
            LicensePlate = json.Value<string>("licensePlate");
            Vin = json.Value<string>("vin");
            EngineNo = json.Value<string>("engineNo");

            YearOfManufacture = json.Value<string>("yearOfManufacture");
            Brand = json.Value<string>("brand");
            Weight = json.Value<string>("weight");
            StartUsingDate = json.Value<string>("startUsingDate");
            DriverId = json.Value<string>("driverId");

            Stock = json.Value<string>("stock");
            BuyingDate = json.Value<string>("buyingDate");
            MonthlyPayment = json.Value<string>("monthlyPayment");
            CheckDate = json.Value<string>("checkDate");
			InsuranceDate = json.Value<string>("insuranceDate");
		}
    }
}
