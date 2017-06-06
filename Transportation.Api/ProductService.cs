using Transportation.Api.Framework;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Transportation.Api
{
    public class ProductService
    {
        public ProductService() { }

        [Route(HttpVerb.Get, "/products")]
        public RestApiResult GetAll()
        {
            var products = ClarityDB.Instance.Products;

            return new RestApiResult { StatusCode = HttpStatusCode.OK, Json = BuildJsonArray(products) };
        }

        [Route(HttpVerb.Post, "/products")]
        public RestApiResult Create(JObject json)
        {
            if (json == null)
            {
                return new RestApiResult { StatusCode = HttpStatusCode.BadRequest };
            }

            Product product = Product.FromJson(json);
            product.CreatedDate = DateTime.Now;

            ClarityDB.Instance.Products.Add(product);
            ClarityDB.Instance.SaveChanges();
            //Create inventory
            CreateInventory(product.ID);

            return new RestApiResult { StatusCode = HttpStatusCode.OK };
        }

        [Route(HttpVerb.Get, "/products/{id}")]
        public RestApiResult GetProductByID(long id)
        {
            Product product = ClarityDB.Instance.Products.FirstOrDefault(x => x.ID == id);

            if (product == null)
            {
                return new RestApiResult { StatusCode = HttpStatusCode.NotFound };
            }

            return new RestApiResult { StatusCode = HttpStatusCode.OK, Json = product.ToJson() };
        }

        [Route(HttpVerb.Delete, "/products/{id}")]
        public RestApiResult Delete(long id)
        {
            Product product = ClarityDB.Instance.Products.FirstOrDefault(x => x.ID == id);

            if (product == null) {
                return new RestApiResult { StatusCode = HttpStatusCode.NotFound };
            }

            ClarityDB.Instance.Products.Remove(product);
            ClarityDB.Instance.SaveChanges();

            return new RestApiResult { StatusCode = HttpStatusCode.OK, Json = product.ToJson() };
        }

        [Route(HttpVerb.Put, "/products/{id}")]
        public RestApiResult Update(long id, JObject json)
        {
            Product product = ClarityDB.Instance.Products.FirstOrDefault(x => x.ID == id);

            if (product == null) {
                return new RestApiResult { StatusCode = HttpStatusCode.NotFound };
            }

            product.ApplyJson(json);
            ClarityDB.Instance.SaveChanges();
            return new RestApiResult { StatusCode = HttpStatusCode.OK, Json = json };
        }

        private void CreateInventory(long productId) {
            Inventory inventory = new Inventory();
            inventory.ProductID = productId;
            inventory.Quantity = 0;
            inventory.CreatedDate = DateTime.Now;
            ClarityDB.Instance.Inventories.Add(inventory);
            ClarityDB.Instance.SaveChanges();
        }

        private JArray BuildJsonArray(IEnumerable<Product> products)
        {
            JArray array = new JArray();

            foreach (Product product in products)
            {
                array.Add(product.ToJson());
            }

            return array;
        }

    }
}
