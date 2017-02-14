using Transportation.Api.Framework;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data;

namespace Transportation.Api
{
    public class WagonService
    {
        public WagonService() { }

        [Route(HttpVerb.Get, "/wagons")]
        public RestApiResult GetAll()
        {
            var wagons = ClarityDB.Instance.Wagons;

            return new RestApiResult { StatusCode = HttpStatusCode.OK, Json = BuildJsonArray(wagons) };
        }

        [Route(HttpVerb.Post, "/wagons")]
        public RestApiResult Create(JObject json)
        {
            if (json == null)
            {
                return new RestApiResult { StatusCode = HttpStatusCode.BadRequest };
            }

            Wagon wagon = Wagon.FromJson(json);

            ClarityDB.Instance.Wagons.Add(wagon);
            ClarityDB.Instance.SaveChanges();

            return new RestApiResult { StatusCode = HttpStatusCode.OK };
        }

        [Route(HttpVerb.Get, "/wagons/{id}")]
        public RestApiResult GetWagonByID(long id)
        {
            Wagon wagon = ClarityDB.Instance.Wagons.FirstOrDefault(x => x.ID == id);

            if (wagon == null)
            {
                return new RestApiResult { StatusCode = HttpStatusCode.NotFound };
            }

            return new RestApiResult { StatusCode = HttpStatusCode.OK, Json = wagon.ToJson() };
        }

        [Route(HttpVerb.Delete, "/wagons/{id}")]
        public RestApiResult Delete(long id)
        {
            Wagon wagon = ClarityDB.Instance.Wagons.FirstOrDefault(x => x.ID == id);

            if (wagon == null) {
                return new RestApiResult { StatusCode = HttpStatusCode.NotFound };
            }

            ClarityDB.Instance.Wagons.Remove(wagon);
            ClarityDB.Instance.SaveChanges();

            return new RestApiResult { StatusCode = HttpStatusCode.OK, Json = wagon.ToJson() };
        }

        [Route(HttpVerb.Put, "/wagons/{id}")]
        public RestApiResult Update(long id, JObject json)
        {
            Wagon wagon = ClarityDB.Instance.Wagons.FirstOrDefault(x => x.ID == id);

            if (wagon == null) {
                return new RestApiResult { StatusCode = HttpStatusCode.NotFound };
            }

            wagon.ApplyJson(json);
            ClarityDB.Instance.SaveChanges();

            return new RestApiResult { StatusCode = HttpStatusCode.OK, Json = json};
        }

        private JArray BuildJsonArray(IEnumerable<Wagon> wagons)
        {
            JArray array = new JArray();

            foreach (Wagon wagon in wagons)
            {
                array.Add(wagon.ToJson());
            }

            return array;
        }

    }
}
