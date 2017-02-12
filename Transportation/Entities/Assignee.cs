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
    public class Assignee : Entity
    {
        [Required]
        public string UserName { get; set; }

        public Assignee()
        {
        }

        public JObject ToJson()
        {
            JObject json = new JObject();
            json["id"] = ID;
            json["username"] = UserName;

            return json;
        }

        public static Assignee FromJson(JObject json)
        {
            Assignee user = new Assignee();
            user.ApplyJson(json, false);

            return user;
        }

        public void ApplyJson(JObject json, bool includeGroups)
        {
            ID = json.Value<long>("id");
            UserName = json.Value<string>("username");
        }
    }
}
