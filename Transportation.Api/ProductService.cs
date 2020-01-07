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

        [Route(HttpVerb.Get, "/products/page")]
        public RestApiResult GetPerPage(string pageIndex, string pageSize, string search)
        {
            int index = Int32.Parse(pageIndex);
            int size = Int32.Parse(pageSize);
            int startIndex = index * size;

            var products = ClarityDB.Instance.Products
                .Where(x => String.IsNullOrEmpty(search) || x.Name.IndexOf(search) > -1)
                .OrderByDescending(x => x.ID)
                .Skip(startIndex)
                .Take(size);
            return new RestApiResult { StatusCode = HttpStatusCode.OK, Json = BuildJsonArray(products) };
        }

        [Route(HttpVerb.Get, "/products/numberOfPages")]
        public RestApiResult GetNumberPage(string pageSize, string search)
        {
            int size = Int32.Parse(pageSize);
            var allRecords = ClarityDB.Instance.Products
                .Where(x => String.IsNullOrEmpty(search) || x.Name.IndexOf(search) > -1)
                .Count();
            int numOfPages = allRecords % size == 0
                ? allRecords / size
                : allRecords / size + 1;

            JObject json = JObject.Parse(@"{'pages': '" + numOfPages + "'}");
            return new RestApiResult { StatusCode = HttpStatusCode.OK, Json = json };
        }

        [Route(HttpVerb.Get, "/productInfos")]
        public RestApiResult GetAllProductInfo()
        {
            var products = ClarityDB.Instance.Products.ToList();
            var productInfoList = this.MapToProductInfo(products);

            return new RestApiResult { StatusCode = HttpStatusCode.OK, Json = BuildJsonProductInfoArray(productInfoList) };
        }

        [Route(HttpVerb.Get, "/productInfos/page")]
        public RestApiResult GetProductInfoPerPage(string pageIndex, string pageSize, string search)
        {
            int index = Int32.Parse(pageIndex);
            int size = Int32.Parse(pageSize);
            int startIndex = index * size;

            var products = ClarityDB.Instance.Products
                .Where(x => String.IsNullOrEmpty(search) || x.Name.IndexOf(search) > -1)
                .OrderByDescending(x => x.ID)
                .Skip(startIndex)
                .Take(size)
                .ToList();
            var productInfoList = this.MapToProductInfo(products);

            return new RestApiResult { StatusCode = HttpStatusCode.OK, Json = BuildJsonProductInfoArray(productInfoList) };
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

            if(ClarityDB.Instance.Products.Any(x => x.Name == product.Name))
            {
                string errorJson = "{ 'message': 'Tên sản phẩm đã tồn tại' }";
                return new RestApiResult { StatusCode = HttpStatusCode.Conflict, Json = JObject.Parse(errorJson) };
            }

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

        private JArray BuildJsonProductInfoArray(IEnumerable<ProductInfo> productInfoList)
        {
            JArray array = new JArray();

            foreach (ProductInfo productInfo in productInfoList)
            {
                array.Add(productInfo.ToJson());
            }

            return array;
        }

        private List<ProductInfo> MapToProductInfo(List<Product> products)
        {
            var productInfoList = new List<ProductInfo>();
            foreach (var product in products)
            {
                ProductInfo productInfo = new ProductInfo();
                productInfo.Name = product.Name;
                productInfo.SumOfInput = ClarityDB.Instance.ProductInputs.Any(x => x.ProductID == product.ID) ?
                    ClarityDB.Instance.ProductInputs.Where(x => x.ProductID == product.ID).Sum(x => x.Quantity) : 0;

                productInfo.SumOfInputTotalAmount = ClarityDB.Instance.ProductInputs.Any(x => x.ProductID == product.ID) ?
                    ClarityDB.Instance.ProductInputs.Where(x => x.ProductID == product.ID).Sum(x => x.Quantity * x.InputPrice) : 0;

                productInfo.SumOfSale = ClarityDB.Instance.OrderDetails.Any(x => x.ProductID == product.ID) ?
                    ClarityDB.Instance.OrderDetails.Where(x => x.ProductID == product.ID).Sum(x => x.Quantity) : 0;

                productInfo.SumOfSaleTotalAmount = ClarityDB.Instance.OrderDetails.Any(x => x.ProductID == product.ID) ?
                    ClarityDB.Instance.OrderDetails.Where(x => x.ProductID == product.ID).Sum(x => x.Quantity * x.Price) : 0;

                productInfo.NumOfRemain = ClarityDB.Instance.Inventories.Any(x => x.ProductID == product.ID) ?
                    ClarityDB.Instance.Inventories.Where(x => x.ProductID == product.ID).FirstOrDefault().Quantity : 0;

                productInfo.Profit = productInfo.SumOfInput != 0 ?
                    productInfo.SumOfSaleTotalAmount - (productInfo.SumOfSale * productInfo.SumOfInputTotalAmount / productInfo.SumOfInput) : 0;

                productInfoList.Add(productInfo);
            }

            return productInfoList;
        }

    }
}
