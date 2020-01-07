﻿using Transportation.Api.Framework;
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
using Newtonsoft.Json;

namespace Transportation.Api
{
    public class WagonService
    {
        const string formatDate = "dd/MM/yyyy";
        public WagonService() { }

        [Route(HttpVerb.Get, "/wagons")]
        public RestApiResult GetAll()
        {
            var wagons = ClarityDB.Instance.Wagons;

            return new RestApiResult { StatusCode = HttpStatusCode.OK, Json = BuildJsonArray(wagons) };
        }

        [Route(HttpVerb.Get, "/wagons/page")]
        public RestApiResult GetPerPage(string pageIndex, string pageSize, string search)
        {
            int index = Int32.Parse(pageIndex);
            int size = Int32.Parse(pageSize);
            int startIndex = index * size;

            var wagons = ClarityDB.Instance.Wagons
                .Where(x => String.IsNullOrEmpty(search) || x.Code.IndexOf(search) > -1)
                .OrderByDescending(x => x.ID)
                .Skip(startIndex)
                .Take(size);
            return new RestApiResult { StatusCode = HttpStatusCode.OK, Json = BuildJsonArray(wagons) };
        }

        [Route(HttpVerb.Get, "/wagons/numberOfPages")]
        public RestApiResult GetNumberPage(string pageSize, string search)
        {
            int size = Int32.Parse(pageSize);
            var allRecords = ClarityDB.Instance.Wagons
                .Where(x => String.IsNullOrEmpty(search) || x.Code.IndexOf(search) > -1)
                .Count();
            int numOfPages = allRecords % size == 0
                ? allRecords / size
                : allRecords / size + 1;

            JObject json = JObject.Parse(@"{'pages': '" + numOfPages + "'}");
            return new RestApiResult { StatusCode = HttpStatusCode.OK, Json = json };
        }

        [Route(HttpVerb.Post, "/wagons")]
        public RestApiResult Create(JObject json)
        {
            if (json == null)
            {
                return new RestApiResult { StatusCode = HttpStatusCode.BadRequest };
            }

            Wagon wagon = Wagon.FromJson(json);
            wagon.CreatedDate = DateTime.Now;

            ClarityDB.Instance.Wagons.Add(wagon);
            ClarityDB.Instance.SaveChanges();

            AddWagonSettlementsFromJson(wagon.ID, json);         

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
            UpdateWagonSettlementsFromJson(wagon.ID, json);

            ClarityDB.Instance.SaveChanges();

            return new RestApiResult { StatusCode = HttpStatusCode.OK, Json = json};
        }

        [Route(HttpVerb.Get, "/wagonReportByDate")]
        public RestApiResult GetWagonReportByDate(string date)
        {
            var dateJSON = JsonConvert.DeserializeObject<JObject>(date);
            DateTime fromDate = DateTime.ParseExact(dateJSON.Value<string>("fromDate"), formatDate, CultureInfo.InvariantCulture);
            DateTime toDate = DateTime.ParseExact(dateJSON.Value<string>("toDate"), formatDate, CultureInfo.InvariantCulture);

            List<Wagon> wagons = ClarityDB.Instance.Wagons.ToList();
            List<Wagon> filteredWagons = new List<Wagon>();

            foreach (Wagon wagon in wagons)
            {
                DateTime paymentDate = DateTime.ParseExact(wagon.PaymentDate, formatDate, CultureInfo.InvariantCulture);
                if (DateTime.Compare(paymentDate, fromDate) >= 0 && DateTime.Compare(paymentDate, toDate) <= 0)
                {
                    filteredWagons.Add(wagon);
                }
            }

            var wagonReportDataList = filteredWagons.Select(w => new WagonReportData
            {
                Code = w.Code,
                TotalAmount = w.WagonSetlements.Sum(x => x.Quantity * x.UnitPrice),
                TotalPayment = 100, //waiting confirm
                Profit = 80         //TotalAmount - TotalPayment
            }).ToList();

            return new RestApiResult { StatusCode = HttpStatusCode.OK, Json = BuildJsonReportDataArray(wagonReportDataList) };
        }

        [Route(HttpVerb.Get, "/allDepartureAndDesination")]
        public RestApiResult GetAllDepartureAndDesination()
        {
            var wagons = ClarityDB.Instance.Wagons;

            return new RestApiResult { StatusCode = HttpStatusCode.OK, Json = BuildJsonData(wagons) };
        }

        private void AddWagonSettlementsFromJson(long wagonID, JObject json) {
            Wagon wagon = ClarityDB.Instance.Wagons.FirstOrDefault(x => x.ID == wagonID);

            var wagonSettlementJsons = json.Value<JArray>("wagonSettlements");
            if (wagonSettlementJsons != null)
            {
				foreach (JObject wagonSettlementJson in wagonSettlementJsons)
				{
					WagonSettlement wagonSettlement = WagonSettlement.FromJson(wagonSettlementJson);
                    wagonSettlement.ID = 0; // prevent wrong order of WagonSettlements in Wagon
                    wagonSettlement.CreatedDate = DateTime.Now;
                    wagonSettlement.PaymentDate = wagon.PaymentDate;
                    wagonSettlement.PaymentPlace = wagon.PaymentPlace;
					wagonSettlement.WagonID = wagonID;
					wagonSettlement.Code = wagonSettlement.WagonID + "_" + wagonSettlement.CustomerID;
					wagon.WagonSetlements.Add(wagonSettlement);
				}
                ClarityDB.Instance.SaveChanges();
            }
        }

        private void UpdateWagonSettlementsFromJson(long wagonID, JObject json)
        {
            DeleteWagonSettements(wagonID);
            AddWagonSettlementsFromJson(wagonID, json);
        }

        private void DeleteWagonSettements(long wagonID)
        {
            var wagonSettlements = ClarityDB.Instance.WagonSettlements.Where(x => x.WagonID == wagonID);
            foreach(WagonSettlement wagonSettlement in wagonSettlements) {
                ClarityDB.Instance.WagonSettlements.Remove(wagonSettlement);
            }
            ClarityDB.Instance.SaveChanges();
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

        private JArray BuildJsonReportDataArray(List<WagonReportData> wagonReportDataList)
        {
            JArray array = new JArray();

            foreach (WagonReportData wagonReport in wagonReportDataList)
            {
                array.Add(wagonReport.ToJson());
            }

            return array;
        }

        private JObject BuildJsonData(IEnumerable<Wagon> wagons)
        {
            JObject json = new JObject();
            List<string> depatures = wagons.Select(wagon => wagon.Departure).Distinct().ToList();
            json["departures"] = new JArray(depatures);
            List<string> destinations = wagons.Select(wagon => wagon.Destination).Distinct().ToList();
            json["destinations"] = new JArray(destinations);
            return json;
        }
    }
}
