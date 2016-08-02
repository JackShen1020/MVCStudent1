using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Taradel.MVCSample.Web.Models;

namespace Taradel.MVCSample.Web.Api
{
    [RoutePrefix("products")]
    public class ProductController : ApiController
    {
        private static List<Product> _productsDatabase { get; set; }
        static ProductController()
        {
            _productsDatabase = new List<Product>();
            _productsDatabase.Add(new Product()
            {
                Id = 0,
                Name = "Thingy",
                Price = 10.00m
            });
        }
        // GET /products
        [Route("")]
        [HttpGet]
        public IEnumerable<Product> GetAllProducts()
        {
            return _productsDatabase;
        }


        // GET /products/{id}
        [Route("{id}", Name = "ProductInfo")]
        [HttpGet]
        public Product GetProduct(long id)
        {
            var product = _productsDatabase.FirstOrDefault(p => p.Id == id);
            if (product != null) return product;

            var message = new HttpResponseMessage(HttpStatusCode.NotFound) { ReasonPhrase = "YOU MESSED UP" };
            throw new HttpResponseException(message);
        }

        // POST /products
        [Route("")]
        [HttpPost]
        public HttpResponseMessage CreateProduct(Product product)
        {
            if (!ModelState.IsValid) throw new HttpResponseException(HttpStatusCode.BadRequest);

            // TODO: validation
            product.Id = _productsDatabase.Max(p => p.Id) + 1;
            _productsDatabase.Add(product);

            var message = Request.CreateResponse(HttpStatusCode.Created, product);
            message.Headers.Location = new Uri(Url.Link("ProductInfo", new {Id = product.Id}));

            return message;
        }

        // PUT /products
        // DELETE /products
    }
}
