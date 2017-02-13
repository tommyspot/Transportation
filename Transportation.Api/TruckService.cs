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
    public class TruckService
    {

        public TruckService() { }

        [Route(HttpVerb.Get, "/truck")]
        public RestApiResult GetAll()
        {
            var trucks = ClarityDB.Instance.Trucks;

            return new RestApiResult { StatusCode = HttpStatusCode.OK, Json = BuildJsonArray(trucks) };
        }

        [Route(HttpVerb.Post, "/truck")]
        public RestApiResult Create(JObject json)
        {
            if (json == null)
            {
                return new RestApiResult { StatusCode = HttpStatusCode.BadRequest };
            }

            Truck truck = Truck.FromJson(json);

            ClarityDB.Instance.Trucks.Add(truck);
            ClarityDB.Instance.SaveChanges();

            return new RestApiResult { StatusCode = HttpStatusCode.OK };
        }

        private JArray BuildJsonArray(IEnumerable<Truck> trucks)
        {
            JArray array = new JArray();

            foreach (Truck truck in trucks)
            {
                array.Add(truck.ToJson());
            }

            return array;
        }

    }
}
