using Transportation.Api.Framework;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Transportation.Api
{
    public class InputOrderService
    {
        public InputOrderService() { }

        [Route(HttpVerb.Get, "/inputOrders")]
        public RestApiResult GetAll()
        {
            var inputOrders = ClarityDB.Instance.InputOrders;

            return new RestApiResult { StatusCode = HttpStatusCode.OK, Json = BuildJsonArray(inputOrders) };
        }

        [Route(HttpVerb.Get, "/inputOrders/page")]
        public RestApiResult GetPerPage(string pageIndex, string pageSize, string search)
        {
            int index = Int32.Parse(pageIndex);
            int size = Int32.Parse(pageSize);
            int startIndex = index * size;

            var inputOrders = ClarityDB.Instance.InputOrders
                .Where(x => String.IsNullOrEmpty(search) || x.Vendor.IndexOf(search) > -1)
                .OrderByDescending(x => x.ID)
                .Skip(startIndex)
                .Take(size);
            return new RestApiResult { StatusCode = HttpStatusCode.OK, Json = BuildJsonArray(inputOrders) };
        }

        [Route(HttpVerb.Get, "/inputOrders/numberOfPages")]
        public RestApiResult GetNumberPage(string pageSize, string search)
        {
            int size = Int32.Parse(pageSize);
            var allRecords = ClarityDB.Instance.InputOrders
                .Where(x => String.IsNullOrEmpty(search) || x.Vendor.IndexOf(search) > -1)
                .Count();
            int numOfPages = allRecords % size == 0
                ? allRecords / size
                : allRecords / size + 1;

            JObject json = JObject.Parse(@"{'pages': '" + numOfPages + "'}");
            return new RestApiResult { StatusCode = HttpStatusCode.OK, Json = json };
        }

        [Route(HttpVerb.Post, "/inputOrders")]
        public RestApiResult Create(JObject json)
        {
            if (json == null)
            {
                return new RestApiResult { StatusCode = HttpStatusCode.BadRequest };
            }

            InputOrder inputOrder = InputOrder.FromJson(json);
            inputOrder.CreatedDate = DateTime.Now;

            ClarityDB.Instance.InputOrders.Add(inputOrder);
            ClarityDB.Instance.SaveChanges();

            AddProductInputsFromJson(inputOrder.ID, json);

            return new RestApiResult { StatusCode = HttpStatusCode.OK };
        }

        [Route(HttpVerb.Get, "/inputOrders/{id}")]
        public RestApiResult GetInputOrderByID(long id)
        {
            InputOrder inputOrder = ClarityDB.Instance.InputOrders.FirstOrDefault(x => x.ID == id);

            if (inputOrder == null)
            {
                return new RestApiResult { StatusCode = HttpStatusCode.NotFound };
            }

            return new RestApiResult { StatusCode = HttpStatusCode.OK, Json = inputOrder.ToJson() };
        }

        [Route(HttpVerb.Delete, "/inputOrders/{id}")]
        public RestApiResult Delete(long id)
        {
            InputOrder inputOrder = ClarityDB.Instance.InputOrders.FirstOrDefault(x => x.ID == id);

            if (inputOrder == null) {
                return new RestApiResult { StatusCode = HttpStatusCode.NotFound };
            }
            //Decrease quantity in Inventory when delete InputOrder
            updateInventoryAfterDeleteInputOrder(id);

            ClarityDB.Instance.InputOrders.Remove(inputOrder);
            ClarityDB.Instance.SaveChanges();

            return new RestApiResult { StatusCode = HttpStatusCode.OK, Json = inputOrder.ToJson() };
        }

        [Route(HttpVerb.Put, "/inputOrders/{id}")]
        public RestApiResult Update(long id, JObject json)
        {
            InputOrder inputOrder = ClarityDB.Instance.InputOrders.FirstOrDefault(x => x.ID == id);

            if (inputOrder == null)
            {
                return new RestApiResult { StatusCode = HttpStatusCode.NotFound };
            }

            inputOrder.ApplyJson(json);
            UpdateProductInputsFromJson(id, json);

            ClarityDB.Instance.SaveChanges();
            return new RestApiResult { StatusCode = HttpStatusCode.OK, Json = json };
        }


        private void AddProductInputsFromJson(long inputOrderID, JObject json)
        {
            InputOrder inputOrder = ClarityDB.Instance.InputOrders.FirstOrDefault(x => x.ID == inputOrderID);

            var productInputsJsons = json.Value<JArray>("productInputs");
            if (productInputsJsons != null)
            {
                foreach (JObject productInputsJson in productInputsJsons)
                {
                    string productName = productInputsJson.Value<string>("productName");
                    Product product = getProductByName(productName);
                    if (product != null)
                    {
                        CreateProductInputForInputOrder(productInputsJson, inputOrder, product.ID);
                    }
                    else {
                        JObject productJson = new JObject();
                        productJson["name"] = productName;
                        ProductService productSevice = new ProductService();
                        productSevice.Create(productJson);

                        Product productAfterCreating = getProductByName(productName);
                        CreateProductInputForInputOrder(productInputsJson, inputOrder, productAfterCreating.ID);
                    }
                }
                ClarityDB.Instance.SaveChanges();
            }
        }

        private void CreateProductInputForInputOrder(JObject json, InputOrder inputOrder, long productId) {
            ProductInput productInput = ProductInput.FromJson(json);
            productInput.ProductID = productId;
            productInput.CreatedDate = DateTime.Now;
            productInput.InputOrderID = inputOrder.ID;
            inputOrder.ProductInputs.Add(productInput);
            // Increase quantity in Inventory
            updateInventory(productInput);
            // Update price back to Product
            updateProductPrice(productInput);
        }

        private void updateInventory(ProductInput productInput) {
            Inventory inventory = ClarityDB.Instance.Inventories.FirstOrDefault(x => x.ProductID == productInput.ProductID);
            inventory.Quantity += productInput.Quantity;
            inventory.LatestPrice = productInput.SalePrice;
        }

        private void updateProductPrice(ProductInput productInput)
        {
            Product product = ClarityDB.Instance.Products.FirstOrDefault(x => x.ID == productInput.ProductID);
            product.Price = productInput.SalePrice;
        }

        private void updateInventoryAfterDeleteInputOrder(long inputOrderID)
        {
            InputOrder inputOrder = ClarityDB.Instance.InputOrders.FirstOrDefault(x => x.ID == inputOrderID);

            if (inputOrder != null && inputOrder.ProductInputs != null && inputOrder.ProductInputs.Count > 0)
            {
                foreach (ProductInput productInput in inputOrder.ProductInputs)
                {
                    Inventory inventory = ClarityDB.Instance.Inventories.FirstOrDefault(x => x.ProductID == productInput.ProductID);
                    inventory.Quantity -= productInput.Quantity;
                }
            }
        }

        private void UpdateProductInputsFromJson(long inputOrderID, JObject json)
        {
            DeleteProductInputs(inputOrderID);
            AddProductInputsFromJson(inputOrderID, json);
        }

        private void DeleteProductInputs(long inputOrderID)
        {
            var productInputs = ClarityDB.Instance.ProductInputs.Where(x => x.InputOrderID == inputOrderID);
            foreach (ProductInput productInput in productInputs)
            {
                //update inventory
                Inventory inventory = ClarityDB.Instance.Inventories.FirstOrDefault(x => x.ProductID == productInput.ProductID);
                inventory.Quantity -= productInput.Quantity;
                ClarityDB.Instance.ProductInputs.Remove(productInput);
            }
            ClarityDB.Instance.SaveChanges();
        }

        private JArray BuildJsonArray(IEnumerable<InputOrder> inputOrders)
        {
            JArray array = new JArray();

            foreach (InputOrder inputOrder in inputOrders)
            {
                array.Add(inputOrder.ToJson());
            }

            return array;
        }

        private Product getProductByName(string name)
        {
            return ClarityDB.Instance.Products.Where(x => x.Name == name).FirstOrDefault();
        }

    }
}
