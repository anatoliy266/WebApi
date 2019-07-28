using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi_app_server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        //SqlConnector conn = new SqlConnector();
        ISqlConnector conn;
        public OrdersController(ISqlConnector iConnector)
        {
            conn = iConnector;
        }
        // GET: api/Orders
        [HttpGet]
        public IEnumerable<string> Get()
        {
            object result = conn.GetData("GetOrders");
            if (result != null)
            {
                return (IEnumerable<string>)result;
            }
            else return new string[] { "Nothing" };
        }
        /*
        // GET: api/Orders/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Orders
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/Orders/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }*/
    }
}
