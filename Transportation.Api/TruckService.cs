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

        [Route(HttpVerb.Get, "/trucks")]
        public RestApiResult GetAll()
        {
            var trucks = ClarityDB.Instance.Trucks;

            return new RestApiResult { StatusCode = HttpStatusCode.OK, Json = BuildJsonArray(trucks) };
        }

        [Route(HttpVerb.Get, "/trucks/curtail")]
        public RestApiResult GetAllCurtail()
        {
            var trucks = ClarityDB.Instance.Trucks;

            return new RestApiResult { StatusCode = HttpStatusCode.OK, Json = BuildJsonArrayCurtail(trucks) };
        }

        [Route(HttpVerb.Get, "/trucks/page")]
        public RestApiResult GetPerPage(string pageIndex, string pageSize, string search)
        {
            int index = Int32.Parse(pageIndex);
            int size = Int32.Parse(pageSize);
            int startIndex = index * size;

            var trucks = ClarityDB.Instance.Trucks
                .Where(x => String.IsNullOrEmpty(search) || x.LicensePlate.IndexOf(search) > -1)
                .OrderByDescending(x => x.ID)
                .Skip(startIndex)
                .Take(size);
            return new RestApiResult { StatusCode = HttpStatusCode.OK, Json = BuildJsonArray(trucks) };
        }

        [Route(HttpVerb.Get, "/trucks/numberOfPages")]
        public RestApiResult GetNumberPage(string pageSize, string search)
        {
            int size = Int32.Parse(pageSize);
            var allRecords = ClarityDB.Instance.Trucks
                .Where(x => String.IsNullOrEmpty(search) || x.LicensePlate.IndexOf(search) > -1)
                .Count();
            int numOfPages = allRecords % size == 0
                ? allRecords / size
                : allRecords / size + 1;

            JObject json = JObject.Parse(@"{'pages': '" + numOfPages + "'}");
            return new RestApiResult { StatusCode = HttpStatusCode.OK, Json = json };
        }

        [Route(HttpVerb.Post, "/trucks")]
        public RestApiResult Create(JObject json)
        {
            if (json == null)
            {
                return new RestApiResult { StatusCode = HttpStatusCode.BadRequest };
            }

            Truck truck = Truck.FromJson(json);
			truck.CreatedDate = DateTime.Now;
            ClarityDB.Instance.Trucks.Add(truck);
            ClarityDB.Instance.SaveChanges();

            return new RestApiResult { StatusCode = HttpStatusCode.OK };
        }

		[Route(HttpVerb.Get, "/trucks/{id}")]
		public RestApiResult GetTruckByID(long id)
		{
			Truck truck = ClarityDB.Instance.Trucks.FirstOrDefault(x => x.ID == id);

			if (truck == null)
			{
				return new RestApiResult { StatusCode = HttpStatusCode.NotFound };
			}

			return new RestApiResult { StatusCode = HttpStatusCode.OK, Json = truck.ToJson() };
		}

		[Route(HttpVerb.Delete, "/trucks/{id}")]
		public RestApiResult Delete(long id)
		{
			Truck truck = ClarityDB.Instance.Trucks.FirstOrDefault(x => x.ID == id);

			if (truck == null)
			{
				return new RestApiResult { StatusCode = HttpStatusCode.NotFound };
			}

            //ClarityDB.Instance.Trucks.Remove(truck);
            truck.IsDeleted = true;
			ClarityDB.Instance.SaveChanges();

			return new RestApiResult { StatusCode = HttpStatusCode.OK, Json = truck.ToJson() };
		}

		[Route(HttpVerb.Put, "/trucks/{id}")]
		public RestApiResult Update(long id, JObject json)
		{
			Truck truck = ClarityDB.Instance.Trucks.FirstOrDefault(x => x.ID == id);

			if (truck == null)
			{
				return new RestApiResult { StatusCode = HttpStatusCode.NotFound };
			}

			truck.ApplyJson(json);
			truck.CreatedDate = DateTime.Now;
			ClarityDB.Instance.SaveChanges();

			return new RestApiResult { StatusCode = HttpStatusCode.OK, Json = json };
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
        private JArray BuildJsonArrayCurtail(IEnumerable<Truck> trucks)
        {
            JArray jArray = new JArray();
            foreach (Truck truck in trucks)
            {
                JObject json = new JObject();
                json["id"] = truck.ID;
                json["licensePlate"] = truck.LicensePlate;
                jArray.Add(json);
            }
            return jArray;
        }
    }
}
