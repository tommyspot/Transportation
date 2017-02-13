//using Transportation.Api.Framework;
//using Newtonsoft.Json.Linq;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net;
//using System.Text;
//using System.Threading.Tasks;

//namespace Transportation.Api
//{
//    public class PolygonService
//    {
//        public PolygonService() { }

//        [Route(HttpVerb.Get, "/polygons")]
//        public RestApiResult GetAll()
//        {
//            return new RestApiResult
//            {
//                Json = BuildJsonArray(ClarityDB.Instance.Polygons)
//            };
//        }

//        private JArray BuildJsonArray(IEnumerable<Polygon> polygons)
//        {
//            JArray array = new JArray();

//            foreach (Polygon polygon in polygons)
//            {
//                array.Add(polygon.ToJson());
//            }

//            return array;
//        }

//        [Route(HttpVerb.Post, "/importPolygons")]
//        public RestApiResult ImportPolygons(JObject json)
//        {
//            if (json == null)
//            {
//                return new RestApiResult { StatusCode = HttpStatusCode.BadRequest };
//            }

//			var polygonListOld = ClarityDB.Instance.Polygons.Where(x => x.Name != "" || 1 == 1);
//			foreach (Polygon polygon in polygonListOld)
//			{
//				ClarityDB.Instance.Polygons.Remove(polygon);
//			}

//			var polygons = json.Value<JArray>("polygons");

//            List<Polygon> polygonList = new List<Polygon>();

//            foreach (var polygon in polygons)
//            {
//                Polygon newPolygon = new Polygon();
//                newPolygon.ApplyJson(polygon);
//				ClarityDB.Instance.Polygons.Add(newPolygon);
//				polygonList.Add(newPolygon);
//			}

//            ClarityDB.Instance.SaveChanges();
//            return new RestApiResult { StatusCode = HttpStatusCode.OK, Json = BuildJsonArray(polygonList) };
//        }
//    }
//}