using Transportation.Api.Framework;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Transportation.Api
{
    public class InmventoryService
    {
        public InmventoryService() { }

        [Route(HttpVerb.Get, "/inventories")]
        public RestApiResult GetAll()
        {
            var inventories = ClarityDB.Instance.Inventories;

            return new RestApiResult { StatusCode = HttpStatusCode.OK, Json = BuildJsonArray(inventories) };
        }

        [Route(HttpVerb.Get, "/inventories/{id}")]
        public RestApiResult GetInventoryByID(long id)
        {
            Inventory inventory = ClarityDB.Instance.Inventories.FirstOrDefault(x => x.ID == id);

            if (inventory == null)
            {
                return new RestApiResult { StatusCode = HttpStatusCode.NotFound };
            }

            return new RestApiResult { StatusCode = HttpStatusCode.OK, Json = inventory.ToJson() };
        }

        private JArray BuildJsonArray(IEnumerable<Inventory> inventories)
        {
            JArray array = new JArray();

            foreach (Inventory inventory in inventories)
            {
                array.Add(inventory.ToJson());
            }

            return array;
        }

    }
}
