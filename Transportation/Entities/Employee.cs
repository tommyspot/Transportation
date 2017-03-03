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
    public class Employee : Entity
    {
        [Required]
        public string FullName { get; set; }
        [Required]
        public string CardID { get; set; }
        public string Address { get; set; }
        public string Mobile { get; set; }
        public string DriverLicenseRank { get; set; }
        public string DriverLicenseID { get; set; }
        public string DriverLicenseAddress { get; set; }
        public string DriverLicenseDate { get; set; }
        public string DriverLicenseExpirationDate { get; set; }
        public string StartDate { get; set; }
        public string Violation { get; set; }
        public string Notes { get; set; }
        public string Title { get; set; }
        public DateTime CreatedDate { get; set; }

        public Employee() {
        }

        public JObject ToJson()
        {
            JObject json = new JObject();
            json["id"] = ID;
            json["fullName"] = FullName;
            json["cardId"] = CardID;
            json["address"] = Address;
            json["mobile"] = Mobile;

            json["driverLicenseRank"] = DriverLicenseRank;
            json["driverLicenseId"] = DriverLicenseID;
            json["driverLicenseAddress"] = DriverLicenseAddress;
            json["driverLicenseDate"] = DriverLicenseDate;
            json["driverLicenseExpirationDate"] = DriverLicenseExpirationDate;

            json["startDate"] = StartDate;
            json["violation"] = Violation;
            json["notes"] = Notes;
            json["title"] = Title;

            return json;
        }

        public static Employee FromJson(JObject json)
        {
            Employee employee = new Employee();
            employee.ApplyJson(json);

            return employee;
        }

        public void ApplyJson(JObject json)
        {
            ID = json.Value<long>("id");

            FullName = json.Value<string>("fullName");
            CardID = json.Value<string>("cardId");
            Address = json.Value<string>("address");
            Mobile = json.Value<string>("mobile");

            DriverLicenseRank = json.Value<string>("driverLicenseRank");
            DriverLicenseID = json.Value<string>("driverLicenseId");
            DriverLicenseAddress = json.Value<string>("driverLicenseAddress");
            DriverLicenseDate = json.Value<string>("driverLicenseDate");
            DriverLicenseExpirationDate = json.Value<string>("driverLicenseExpirationDate");

            StartDate = json.Value<string>("startDate");
            Violation = json.Value<string>("violation");
            Notes = json.Value<string>("notes");
            Title = json.Value<string>("title");
        }
    }
}
