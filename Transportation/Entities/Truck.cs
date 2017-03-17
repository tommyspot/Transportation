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
        public string LicensePlate { get; set; }
        public string Vin { get; set; }
        public string EngineNo { get; set; }
        public long YearOfManufacture { get; set; }
        public string Brand { get; set; }
        public long Weight { get; set; }
        public string StartUsingDate { get; set; }
        public string EmployeeId { get; set; }
        public long Stock { get; set; }
        public string BuyingDate { get; set; }
        public long MonthlyPayment { get; set; }
        public string CheckDate { get; set; }
		public string InsuranceDate { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedDate { get; set; }
		public Truck() {
        }

        public JObject ToJson()
        {
            JObject json = new JObject();
            json["id"] = ID;
			json["licensePlate"] = LicensePlate;
            json["vin"] = Vin;
            json["engineNo"] = EngineNo;
            json["yearOfManufacture"] = YearOfManufacture;

            json["brand"] = Brand;
            json["weight"] = Weight;
            json["startUsingDate"] = StartUsingDate;
            json["employeeId"] = EmployeeId;
            json["stock"] = Stock;

            json["buyingDate"] = BuyingDate;
            json["monthlyPayment"] = MonthlyPayment;
            json["checkDate"] = CheckDate;
            json["insuranceDate"] = InsuranceDate;
            json["isDeleted"] = IsDeleted;

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

            LicensePlate = json.Value<string>("licensePlate");
            Vin = json.Value<string>("vin");
            EngineNo = json.Value<string>("engineNo");

            YearOfManufacture = json.Value<long>("yearOfManufacture");
            Brand = json.Value<string>("brand");

            Weight = json.Value<long>("weight");

            if (!String.IsNullOrEmpty(json.Value<string>("startUsingDate")))
            {
                StartUsingDate = json.Value<string>("startUsingDate");
            }
            
            EmployeeId = json.Value<string>("employeeId");

            Stock = json.Value<long>("stock");

            if (!String.IsNullOrEmpty( json.Value<string>("buyingDate")))
            {
                BuyingDate = json.Value<string>("buyingDate");
            }
            MonthlyPayment = json.Value<long>("monthlyPayment");

            if (!String.IsNullOrEmpty( json.Value<string>("checkDate")))
            {
                CheckDate = json.Value<string>("checkDate");
            }
            if (!String.IsNullOrEmpty( json.Value<string>("insuranceDate")))
            {
                InsuranceDate = json.Value<string>("insuranceDate");
            }
			
		}
    }
}
