using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Data;
using Newtonsoft.Json;

namespace Transportation
{
    public class TrashInformation : Entity
    {
        public long TrashID { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Continent { get; set; }
        public string Country { get; set; }
        public string AdministrativeArea1 { get; set; }
        public string AdministrativeArea2 { get; set; }
        public string AdministrativeArea3 { get; set; }
        public string Locality { get; set; }
        public string SubLocality { get; set; }
        public string Description { get; set; }
        public string Comment { get; set; }
        public long Status { get; set; }
        public string Url { get; set; }
        public string Images { get; set; }
        public string Size { get; set; }
        public string Type { get; set; }
        public long? AssigneeID { get; set; }
        public DateTime ModifiedDate { get; set; }
        public long? SectionID { get; set; }
		public bool IsDisabled { get; set; }

		public void ApplyJson(JToken json)
        {
            TrashID = json.Value<long>("trashId");
            Latitude = json.Value<double>("latitude");
            Longitude = json.Value<double>("longitude");

            Continent = json.Value<string>("continent");
            Country = json.Value<string>("country");

            AdministrativeArea1 = json.Value<string>("administrativeArea1");
            AdministrativeArea2 = json.Value<string>("administrativeArea2");
            AdministrativeArea3 = json.Value<string>("AdministrativeArea3");

            Locality = json.Value<string>("locality");
            SubLocality = json.Value<string>("subLocality");
            Description = json.Value<string>("description");
            Comment = json.Value<string>("comment");
			Status = json.Value<long>("status");
			Size = json.Value<string>("size");
            Url = json.Value<string>("url");
			Images = JsonConvert.SerializeObject(json["images"]);
			Type = JsonConvert.SerializeObject(json["types"]);
			AssigneeID = json.Value<long?>("assigneeId");
            SectionID = json.Value<long?>("sectionId");
        }

        public JObject ToJson()
        {
            JObject json = new JObject();
            json["id"] = ID;
            json["trashId"] = TrashID;
            json["latitude"] = Latitude;
            json["longitude"] = Longitude;

            json["continent"] = Continent;
            json["country"] = Country;
            json["administrativeArea1"] = AdministrativeArea1;
            json["administrativeArea2"] = AdministrativeArea2;
            json["administrativeArea3"] = AdministrativeArea3;

            json["locality"] = Locality;
            json["subLocality"] = SubLocality;
            json["description"] = Description;
            json["comment"] = Comment;
            json["modifiedDate"] = ModifiedDate.ToString("MM/dd/yyyy HH:mm:ss");

            json["status"] = Status;
            json["size"] = Size;

            json["url"] = Url;
			json["images"] = JArray.Parse(Images);
			json["types"] = JArray.Parse(Type);
			json["assigneeId"] = AssigneeID;
            json["sectionId"] = SectionID;
            return json;
        }

	}
}
