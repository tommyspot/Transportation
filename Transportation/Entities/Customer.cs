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
    public class Customer : Entity
    {
        [Required]
		public string FullName { get; set; }
		[Required]
		public string PhoneNo { get; set; }
        public long TotalOwned { get; set; }
        public long TotalPay { get; set; }
        public long TotalDebt { get; set; }
		public string Type { get; set; }
		public string Code { get; set; }
		public bool NeedUpdatePayment { get; set; }
		public DateTime CreatedDate { get; set; }
		public Customer() {
        }

        public JObject ToJson()
        {
            JObject json = new JObject();
			json["id"] = ID;
			json["fullName"] = FullName;
            json["totalOwned"] = TotalOwned;
            json["totalPay"] = TotalPay;
            json["totalDebt"] = TotalDebt;
			json["phoneNo"] = PhoneNo;
			json["type"] = Type;
			json["code"] = Code;
			return json;
        }

        public static Customer FromJson(JObject json)
        {
            Customer customer = new Customer();
            customer.ApplyJson(json);
            return customer;
        }

        public void ApplyJson(JObject json)
        {
            ID = json.Value<long>("id");
            FullName = json.Value<string>("fullName");
            TotalOwned = json.Value<long>("totalOwned");
            TotalPay = json.Value<long>("totalPay");
            TotalDebt = json.Value<long>("totalDebt");
			PhoneNo = json.Value<string>("phoneNo");
			Type = json.Value<string>("type");
			Code = json.Value<string>("code");
		}
    }
}
