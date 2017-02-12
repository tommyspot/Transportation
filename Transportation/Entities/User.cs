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
    public class User : Entity
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }

        public byte[] Salt { get; set; }

        public Role Role { get; set; }

        public User() {
        }

        public JObject ToJson()
        {
            JObject json = new JObject();
            json["id"] = ID;
            json["username"] = UserName;

            json["lastName"] = LastName;
            json["firstName"] = FirstName;

            json["role"] = Role.ToString();

            return json;
        }

        public static User FromJson(JObject json)
        {
            User user = new User();
            user.ApplyJson(json, false);

            return user;
        }

        public void ApplyJson(JObject json, bool includeGroups)
        {
            ID = json.Value<long>("id");

            FirstName = json.Value<string>("firstName");
            LastName = json.Value<string>("lastName");
            UserName = json.Value<string>("username");
            Password = json.Value<string>("password");

            Role tempRole;

            if (Enum.TryParse<Role>(json.Value<string>("role"), out tempRole))
            {
                Role = tempRole;
            }
        }
    }
}
