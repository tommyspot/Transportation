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
        public DateTime PaymentDate { get; set; }
        [Required]
        public string PaymentPlace { get; set; }
        [Required]
        public DateTime DepartDate { get; set; }
        [Required]
        public DateTime ReturnDate { get; set; }
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

        public long PaymentOfTruck { get; set; }
        public long PaymentOfRepairing { get; set; }
        public long PaymentOfOil { get; set; }
        public long PaymentOfLuong { get; set; }
        public long PaymentOfService { get; set; }
        public long PaymentOfHangVe { get; set; }
        public long PaymentOf10Percent { get; set; }
        public long PaymentOfOthers { get; set; }
		public string ReasonForPhiPhatXinh { get; set; }
		public string Notes { get; set; }

        public Wagon() {
            WagonSetlements = new Collection<WagonSettlement>();
        }

        public JObject ToJson()
        {
            JObject json = new JObject();
            json["id"] = ID;

            json["code"] = Code;
            json["departDate"] = DepartDate;
            json["returnDate"] = ReturnDate;
            json["truckId"] = TruckID;
            json["employeeId"] = EmployeeID;

            json["paymentDate"] = PaymentDate;
            json["paymentPlace"] = PaymentPlace;

            json["costOfTruck"] = CostOfTruck;
            json["costOfService"] = CostOfService;
            json["costOfTangBoXe"] = CostOfTangBoXe;
            json["costOfPenalty"] = CostOfPenalty;
            json["costOfExtra"] = CostOfExtra;

            json["paymentOfTruck"] = PaymentOfTruck;
            json["paymentOfRepairing"] = PaymentOfRepairing;
            json["paymentOfOil"] = PaymentOfOil;
            json["paymentOfLuong"] = PaymentOfLuong;
            json["paymentOfService"] = PaymentOfService;
            json["paymentOfHangVe"] = PaymentOfHangVe;
            json["paymentOf10Percent"] = PaymentOf10Percent;
            json["paymentOfOthers"] = PaymentOfOthers;
            json["wagonSettlements"] = BuildJsonArray(WagonSetlements);
			json["reasonForPhiPhatXinh"] = ReasonForPhiPhatXinh;
			json["notes"] = Notes;

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
            DepartDate = json.Value<DateTime>("departDate");
            ReturnDate = json.Value<DateTime>("returnDate");
            TruckID = json.Value<long>("truckId");
            EmployeeID = json.Value<long>("employeeId");

            PaymentDate = json.Value<DateTime>("paymentDate");
            PaymentPlace = json.Value<string>("paymentPlace");

            CostOfTruck = json.Value<long>("costOfTruck");
            CostOfService = json.Value<long>("costOfService");
            CostOfTangBoXe = json.Value<long>("costOfTangBoXe");
            CostOfPenalty = json.Value<long>("costOfPenalty");
            CostOfExtra = json.Value<long>("costOfExtra");

            PaymentOfTruck = json.Value<long>("paymentOfTruck");
            PaymentOfRepairing = json.Value<long>("paymentOfRepairing");
            PaymentOfOil = json.Value<long>("paymentOfOil");
            PaymentOfLuong = json.Value<long>("paymentOfLuong");
            PaymentOfService = json.Value<long>("paymentOfService");
            PaymentOfHangVe = json.Value<long>("paymentOfHangVe");
            PaymentOf10Percent = json.Value<long>("paymentOf10Percent");
            PaymentOfOthers = json.Value<long>("paymentOfOthers");

			ReasonForPhiPhatXinh = json.Value<string>("reasonForPhiPhatXinh");
			Notes = json.Value<string>("notes");
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
