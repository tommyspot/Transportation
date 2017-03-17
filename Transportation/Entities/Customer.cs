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
        public string Area { get; set; }
		[Required]
		public long PhoneNo { get; set; }
		public string EmployeeID { get; set; }
        public string TotalOwned { get; set; }
        public string TotalPay { get; set; }
        public string TotalDebt { get; set; }
		public string Type { get; set; }
		public string Code { get; set; }
		public DateTime CreatedDate { get; set; }
		public Customer() {
        }

        public JObject ToJson()
        {
            JObject json = new JObject();
			json["id"] = ID;
			json["fullName"] = FullName;
			json["area"] = Area;
			json["employeeId"] = EmployeeID;
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
            Area = json.Value<string>("area");
            EmployeeID = json.Value<string>("employeeId");
            TotalOwned = json.Value<string>("totalOwned");

            TotalPay = json.Value<string>("totalPay");
            TotalDebt = json.Value<string>("totalDebt");
			PhoneNo = json.Value<long>("phoneNo");
			Type = json.Value<string>("type");
			Code = json.Value<string>("code");
		}
    }
}
