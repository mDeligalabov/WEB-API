using HBProducts.WEB.API.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;

namespace HBProducts.WEB.API.Controllers
{   
    /// <summary>
    /// This controler is used for Accessing the Product data.
    /// </summary>
    public class ProductController : ApiController
    {
        private readonly Database obj = new Database();


        /// <summary>
        /// Method for getting a list of all the Products that are in the Database.
        /// </summary>
        /// <returns>Returns a JSON string, which can be deserialized to ProductList.</returns>
        // GET: api/Product
        public string Get()
        {
            return JsonConvert.SerializeObject(obj.GetAll(), Formatting.Indented);
        }

        /// <summary>
        /// Method for getting the data for a single Product.
        /// </summary>
        /// <param name="id">The SQL id that the database generates automatically.</param>
        /// <returns>Returns a JSON string, which can be deserialized to Product.</returns>
        // GET: api/Product/5
        public string Get(int id)
        {
            return JsonConvert.SerializeObject(obj.GetProduct(id), Formatting.Indented);
        }
    }
}
