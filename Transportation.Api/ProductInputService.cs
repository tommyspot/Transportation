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

        [Route(HttpVerb.Get, "/productInputs/page")]
        public RestApiResult GetPerPage(string pageIndex, string pageSize)
        {
            int index = Int32.Parse(pageIndex);
            int size = Int32.Parse(pageSize);
            int startIndex = index * size;

            var productInputs = ClarityDB.Instance.ProductInputs
                .OrderByDescending(x => x.ID)
                .Skip(startIndex)
                .Take(size);
            return new RestApiResult { StatusCode = HttpStatusCode.OK, Json = BuildJsonArray(productInputs) };
        }

        [Route(HttpVerb.Get, "/productInputs/pageSize/{pageSize}")]
        public RestApiResult GetNumberPage(int pageSize)
        {
            var allRecords = ClarityDB.Instance.ProductInputs.Count();
            int numOfPages = allRecords % pageSize == 0
                ? allRecords / pageSize
                : allRecords / pageSize + 1;

            JObject json = JObject.Parse(@"{'pages': '" + numOfPages + "'}");
            return new RestApiResult { StatusCode = HttpStatusCode.OK, Json = json };
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

        [Route(HttpVerb.Post, "/productInputs/list")]
        public RestApiResult CreateList(JArray jsonList)
        {
            if (jsonList == null)
            {
                return new RestApiResult { StatusCode = HttpStatusCode.BadRequest };
            }

            foreach (JObject json in jsonList) {
                ProductInput productInputs = ProductInput.FromJson(json);
                productInputs.CreatedDate = DateTime.Now;
                ClarityDB.Instance.ProductInputs.Add(productInputs);
                //update Inventory and Product
                long productId = json.Value<long>("productId");
                long quantity = json.Value<long>("quantity");
                long price = json.Value<long>("price");
                UpdateInventory(productId, quantity);
            }
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

            UpdateInventoryAfterDeleteProductInput(productInput.ProductID, productInput.Quantity);
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

            long newQuantity = json.Value<long>("quantity");
            long oldQuantity = productInput.Quantity;
            long deltaQuantity = newQuantity - oldQuantity;
            UpdateInventoryAfterUpdateProductInput(productInput.ProductID, deltaQuantity);

            productInput.ApplyJson(json);
            ClarityDB.Instance.SaveChanges();
            
            return new RestApiResult { StatusCode = HttpStatusCode.OK, Json = json};
        }

        private void UpdateInventory(long productId, long quantity) {
            Inventory inventory = ClarityDB.Instance.Inventories.FirstOrDefault(x => x.ProductID == productId);
            inventory.Quantity += quantity;
            ClarityDB.Instance.SaveChanges();
        }

        private void UpdateInventoryAfterDeleteProductInput(long productId, long quantity)
        {
            Inventory inventory = ClarityDB.Instance.Inventories.FirstOrDefault(x => x.ProductID == productId);
            inventory.Quantity -= quantity;
            ClarityDB.Instance.SaveChanges();
        }

        private void UpdateInventoryAfterUpdateProductInput(long productId, long quantity)
        {
            Inventory inventory = ClarityDB.Instance.Inventories.FirstOrDefault(x => x.ProductID == productId);
            inventory.Quantity += quantity;
            ClarityDB.Instance.SaveChanges();
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
