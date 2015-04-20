using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace ApiPractice.Controllers
{
    [GaAuthorization]
    public class ProductsController : ApiController
    {
        public Product Get(string id)
        {
            Product product = new Product();
            product.Name = "Product Name";
            product.Price = 10.99;
            product.Category = "Product Category";
            return product;
        }
    }

    public class Product
    {
        public string Name { get; set; }
        public double Price { get; set; }
        public string Category { get; set; }
    }
}
