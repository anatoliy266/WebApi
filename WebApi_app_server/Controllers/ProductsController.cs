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
        private SqlConnection ConnectToDB(string host, string user, string password)
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.DataSource = "localhost\\SQL";
            builder.InitialCatalog = "WalletDataBase";
            builder.UserID = user;
            builder.Password = password;
            
            return new SqlConnection(builder.ConnectionString);
        }

        private object GetData(string storedProc, Dictionary<string, object> parameters = null)
        {
            var conn = ConnectToDB("", "sa", "256532");

            conn.Open();
            SqlCommand command = new SqlCommand(storedProc, conn);
            command.CommandType = CommandType.StoredProcedure;
            if (parameters != null)
            {
                foreach (var parameter in parameters)
                {
                    command.Parameters.AddWithValue("@" + parameter.Key, parameter.Value);
                }
            }
            command.Parameters.Add("@result", SqlDbType.VarChar, 1000);
            command.Parameters["@result"].Direction = ParameterDirection.Output;
            var i = command.ExecuteScalar();
            List<string> data = new List<String>();

            data.Add(Convert.ToString(command.Parameters["@result"].Value));
            conn.Close();
            return data.ToArray();
        }

        // GET: api/Products
        [HttpGet]
        public IEnumerable<string> Get()
        {
            object result = GetData("GetProduct");
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
            parameters["key"] = 1;
            object result = GetData("GetProduct", parameters);
            return (IEnumerable<string>)result;
        }

        // POST: api/Products
        [HttpPost]
        public string Post([FromBody] string value)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("key", value);
            object result = GetData("InsertProduct", parameters);
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
            object result = GetData("InsertProduct", parameters);
            return (string)result;
        }
    }
}
