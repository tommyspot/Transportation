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
    public class Polygon : Entity
    {
        public string Name { get; set; }

        public string Coordinates { get; set; }

        public Polygon() {
        }

        public JObject ToJson()
        {
            JObject json = new JObject();
            json["id"] = ID;
            json["name"] = Name;
            json["coordinates"] = JArray.Parse(Coordinates);
            return json;
        }

        public static Polygon FromJson(JToken json)
        {
            Polygon polygon = new Polygon();
            polygon.ApplyJson(json);

            return polygon;
        }

        public void ApplyJson(JToken json)
        {
            ID = json.Value<long>("id");
            Name = json.Value<string>("name");
            Coordinates = JsonConvert.SerializeObject(json["coordinates"]);
        }
    }
}
