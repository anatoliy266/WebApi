using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;

namespace WebApi_app_server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        //SqlConnector conn = new SqlConnector();
        ISqlConnector conn;
        public ProductsController(ISqlConnector iConnector)
        {
            conn = iConnector;
        }

        // GET: api/Products
        [HttpGet]
        public IEnumerable<string> Get()
        {
            object result = conn.GetData("GetProduct");
            if (result != null)
            {
                return (IEnumerable<string>)result;
            }
            else return new string[] { "Nothing" };
            
        }

        // GET: api/Products/5
        [HttpGet("{id}", Name = "Get")]
        public IEnumerable<string> Get(int id)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters["key"] = id;
            object result = conn.GetData("GetProduct", parameters);
            return (IEnumerable<string>)result;
        }

        // POST: api/Products
        [HttpPost]
        public string Post([FromBody] string value)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("key", value);
            object result = conn.GetData("InsertProduct", parameters);
            return (string)result;
        }

        // PUT: api/Products/5
        /*[HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {

        }*/

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public string Delete(int id)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("key", 0-id);
            object result = conn.GetData("InsertProduct", parameters);
            return (string)result;
        }
    }
}
