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
using System.Data.Entity.Validation;

namespace Transportation.Api
{
    public class WagonSettlementService
    {
        public WagonSettlementService() { }

        [Route(HttpVerb.Get, "/wagonSettlements")]
        public RestApiResult GetAll()
        {
            var wagonSettlements = ClarityDB.Instance.WagonSettlements;

            return new RestApiResult { StatusCode = HttpStatusCode.OK, Json = BuildJsonArray(wagonSettlements) };
        }

        [Route(HttpVerb.Post, "/wagonSettlements")]
        public RestApiResult Create(JObject json)
        {
            if (json == null)
            {
                return new RestApiResult { StatusCode = HttpStatusCode.BadRequest };
            }

            WagonSettlement wagonSettlement = WagonSettlement.FromJson(json);
            wagonSettlement.CreatedDate = DateTime.Now;

            ClarityDB.Instance.WagonSettlements.Add(wagonSettlement);
            ClarityDB.Instance.SaveChanges();

            return new RestApiResult { StatusCode = HttpStatusCode.OK };
        }

        [Route(HttpVerb.Get, "/wagonSettlements/{id}")]
        public RestApiResult GetWagonByID(long id)
        {
            WagonSettlement wagonSettlement = ClarityDB.Instance.WagonSettlements.FirstOrDefault(x => x.ID == id);

            if (wagonSettlement == null)
            {
                return new RestApiResult { StatusCode = HttpStatusCode.NotFound };
            }

            return new RestApiResult { StatusCode = HttpStatusCode.OK, Json = wagonSettlement.ToJson() };
        }

        [Route(HttpVerb.Delete, "/wagonSettlements/{id}")]
        public RestApiResult Delete(long id)
        {
            WagonSettlement wagonSettlement = ClarityDB.Instance.WagonSettlements.FirstOrDefault(x => x.ID == id);

            if (wagonSettlement == null) {
                return new RestApiResult { StatusCode = HttpStatusCode.NotFound };
            }

            ClarityDB.Instance.WagonSettlements.Remove(wagonSettlement);
            ClarityDB.Instance.SaveChanges();

            return new RestApiResult { StatusCode = HttpStatusCode.OK, Json = wagonSettlement.ToJson() };
        }

        [Route(HttpVerb.Put, "/wagonSettlements/{id}")]
        public RestApiResult Update(long id, JObject json)
        {
            WagonSettlement wagonSettlement = ClarityDB.Instance.WagonSettlements.FirstOrDefault(x => x.ID == id);

            if (wagonSettlement == null) {
                return new RestApiResult { StatusCode = HttpStatusCode.NotFound };
            }

            wagonSettlement.ApplyJson(json);
            ClarityDB.Instance.SaveChanges();

            return new RestApiResult { StatusCode = HttpStatusCode.OK, Json = json};
        }

        private JArray BuildJsonArray(IEnumerable<WagonSettlement> wagonSettlements)
        {
            JArray array = new JArray();

            foreach (WagonSettlement wagonSettlement in wagonSettlements)
            {
                array.Add(wagonSettlement.ToJson());
            }

            return array;
        }

    }
}
