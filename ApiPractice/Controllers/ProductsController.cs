using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace ApiPractice.Controllers
{
    /// <summary>
    /// The main access point to test
    /// </summary>
    [GaAuthorization]
    public class ProductsController : ApiController
    {
        /// <summary>
        /// The GET method.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>An instance of Product</returns>
        public Product Get(string id)
        {
            Product product = new Product();
            product.Name = "Product Name";
            product.Price = 10.99;
            product.Category = "Product Category";
            return product;
        }
    }

    /// <summary>
    /// Product definition
    /// </summary>
    public class Product
    {
        public string Name { get; set; }
        public double Price { get; set; }
        public string Category { get; set; }
    }
}
