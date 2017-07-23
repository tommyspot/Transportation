using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Data;
using System.Collections.ObjectModel;

namespace Transportation
{
    public class Wagon : Entity
    {
        public DateTime CreatedDate { get; set; }
        public string Code { get; set; }
        [Required]
        public string PaymentDate { get; set; }
        [Required]
        public string PaymentPlace { get; set; }
        [Required]
        public string DepartDate { get; set; }
        [Required]
        public string ReturnDate { get; set; }
        [Required]
        public string Departure { get; set; }
        [Required]
        public string Destination { get; set; }
        [Required]
        public long TruckID { get; set; }
        public virtual Truck Truck { get; set; }
        [Required]
        public long EmployeeID { get; set; }
        public virtual Employee Employee{ get; set; }
        public virtual Collection<WagonSettlement> WagonSetlements { get; set; }
        public long CostOfTruck { get; set; }
        public long CostOfService { get; set; }
        public long CostOfTangBoXe { get; set; }
        public long CostOfPenalty { get; set; }
        public long CostOfExtra { get; set; }
        public string TextOfExtra { get; set; }
        public long PaymentOfTruck { get; set; }
        public long PaymentOfRepairing { get; set; }
        public long PaymentOfOil { get; set; }
        public long PaymentOfLuong { get; set; }
        public long PaymentOfService { get; set; }
        public long PaymentOfHangVe { get; set; }
        public long PaymentOf10Percent { get; set; }

        public Wagon() {
            WagonSetlements = new Collection<WagonSettlement>();
        }

        public JObject ToJson()
        {
            JObject json = new JObject();
            json["id"] = ID;
            json["code"] = Code;

            json["paymentDate"] = PaymentDate;
            json["paymentPlace"] = PaymentPlace;
            json["departDate"] = DepartDate;
            json["returnDate"] = ReturnDate;
            json["departure"] = Departure;
            json["destination"] = Destination;
            json["truckId"] = TruckID;
            json["employeeId"] = EmployeeID;

            json["costOfTruck"] = CostOfTruck;
            json["costOfService"] = CostOfService;
            json["costOfTangBoXe"] = CostOfTangBoXe;
            json["costOfPenalty"] = CostOfPenalty;
            json["costOfExtra"] = CostOfExtra;
            json["textOfExtra"] = TextOfExtra;

            json["paymentOfTruck"] = PaymentOfTruck;
            json["paymentOfRepairing"] = PaymentOfRepairing;
            json["paymentOfOil"] = PaymentOfOil;
            json["paymentOfLuong"] = PaymentOfLuong;
            json["paymentOfService"] = PaymentOfService;
            json["paymentOfHangVe"] = PaymentOfHangVe;
            json["paymentOf10Percent"] = PaymentOf10Percent;
            
            json["wagonSettlements"] = BuildJsonArray(WagonSetlements);
            return json;
        }

        public static Wagon FromJson(JObject json)
        {
            Wagon wagon = new Wagon();
            wagon.ApplyJson(json);

            return wagon;
        }

        public void ApplyJson(JObject json)
        {
            ID = json.Value<long>("id");

            Code = json.Value<string>("code");
            DepartDate = json.Value<string>("departDate");
            ReturnDate = json.Value<string>("returnDate");
            Departure = json.Value<string>("departure");
            Destination = json.Value<string>("destination");
            TruckID = json.Value<long>("truckId");
            EmployeeID = json.Value<long>("employeeId");

            PaymentDate = json.Value<string>("paymentDate");
            PaymentPlace = json.Value<string>("paymentPlace");

            CostOfTruck = json.Value<long>("costOfTruck");
            CostOfService = json.Value<long>("costOfService");
            CostOfTangBoXe = json.Value<long>("costOfTangBoXe");
            CostOfPenalty = json.Value<long>("costOfPenalty");
            CostOfExtra = json.Value<long>("costOfExtra");
            TextOfExtra = json.Value<string>("textOfExtra");

            PaymentOfTruck = json.Value<long>("paymentOfTruck");
            PaymentOfRepairing = json.Value<long>("paymentOfRepairing");
            PaymentOfOil = json.Value<long>("paymentOfOil");
            PaymentOfLuong = json.Value<long>("paymentOfLuong");
            PaymentOfService = json.Value<long>("paymentOfService");
            PaymentOfHangVe = json.Value<long>("paymentOfHangVe");
            PaymentOf10Percent = json.Value<long>("paymentOf10Percent");
        }

        private JArray BuildJsonArray(Collection<WagonSettlement> wagonSettlements)
        {
            JArray jsons = new JArray();

            foreach (WagonSettlement wagonSettlement in wagonSettlements)
            {
                jsons.Add(wagonSettlement.ToJson());
            }

            return jsons;
        }
    }
}
