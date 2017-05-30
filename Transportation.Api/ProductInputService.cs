using Transportation.Api.Framework;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Transportation.Api
{
    public class ProductInputService
    {
        public ProductInputService() { }

        [Route(HttpVerb.Get, "/productInputs")]
        public RestApiResult GetAll()
        {
            var productInputs = ClarityDB.Instance.ProductInputs;

            return new RestApiResult { StatusCode = HttpStatusCode.OK, Json = BuildJsonArray(productInputs) };
        }

        [Route(HttpVerb.Post, "/productInputs")]
        public RestApiResult Create(JObject json)
        {
            if (json == null)
            {
                return new RestApiResult { StatusCode = HttpStatusCode.BadRequest };
            }

            ProductInput productInputs = ProductInput.FromJson(json);
            productInputs.CreatedDate = DateTime.Now;

            ClarityDB.Instance.ProductInputs.Add(productInputs);
            ClarityDB.Instance.SaveChanges();

            return new RestApiResult { StatusCode = HttpStatusCode.OK };
        }

        [Route(HttpVerb.Get, "/productInputs/{id}")]
        public RestApiResult GetProductInputByID(long id)
        {
            ProductInput productInput = ClarityDB.Instance.ProductInputs.FirstOrDefault(x => x.ID == id);

            if (productInput == null)
            {
                return new RestApiResult { StatusCode = HttpStatusCode.NotFound };
            }

            return new RestApiResult { StatusCode = HttpStatusCode.OK, Json = productInput.ToJson() };
        }

        [Route(HttpVerb.Delete, "/productInputs/{id}")]
        public RestApiResult Delete(long id)
        {
            ProductInput productInput = ClarityDB.Instance.ProductInputs.FirstOrDefault(x => x.ID == id);

            if (productInput == null) {
                return new RestApiResult { StatusCode = HttpStatusCode.NotFound };
            }

            ClarityDB.Instance.ProductInputs.Remove(productInput);
            ClarityDB.Instance.SaveChanges();

            return new RestApiResult { StatusCode = HttpStatusCode.OK, Json = productInput.ToJson() };
        }

        [Route(HttpVerb.Put, "/productInputs/{id}")]
        public RestApiResult Update(long id, JObject json)
        {
            ProductInput productInput = ClarityDB.Instance.ProductInputs.FirstOrDefault(x => x.ID == id);

            if (productInput == null) {
                return new RestApiResult { StatusCode = HttpStatusCode.NotFound };
            }

            productInput.ApplyJson(json);

            //if (employee.IsInvalid())
            //    return new RestApiResult { StatusCode = HttpStatusCode.BadRequest };

            ClarityDB.Instance.SaveChanges();
            return new RestApiResult { StatusCode = HttpStatusCode.OK, Json = json};
        }

        private JArray BuildJsonArray(IEnumerable<ProductInput> productInputs)
        {
            JArray array = new JArray();

            foreach (ProductInput productInput in productInputs)
            {
                array.Add(productInput.ToJson());
            }

            return array;
        }

    }
}
