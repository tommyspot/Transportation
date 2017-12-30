using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json.Linq;
using System.Collections.ObjectModel;

namespace Transportation
{
    public class Customer : Entity
    {
        public DateTime CreatedDate { get; set; }
        [Required]
		public string FullName { get; set; }
		[Required]
        public virtual Collection<Payment> Payments { get; set; }
        public string PhoneNo { get; set; }
        public string Code { get; set; }
        public string Type { get; set; }
        [NotMapped]
        public long TotalOwned { get; set; }
        [NotMapped]
        public long TotalPay { get; set; }
        [NotMapped]
        public long TotalDebt { get; set; }

        public Customer() {
            Payments = new Collection<Payment>();
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
            json["payments"] = BuildJsonArray(Payments);
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
            Code = FullName.Replace(" ", "") + "_" + PhoneNo;
        }

        private JArray BuildJsonArray(Collection<Payment> payments)
        {
            JArray jsons = new JArray();

            foreach (Payment payment in payments)
            {
                jsons.Add(payment.ToJson());
            }

            return jsons;
        }
    }
}
