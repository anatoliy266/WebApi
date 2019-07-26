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
            builder.DataSource = host;
            builder.UserID = user;
            builder.Password = password;
            builder.InitialCatalog = "WalletDataBase";
            return new SqlConnection(builder.ConnectionString);
        }

        private async void GetData(string storedProc, object result, Dictionary<string, object> parameters = null)
        {
            var res = await Task.Run(() =>
            {
                var conn = ConnectToDB("(localhost\\SQLEXPRESS)", "sa", "123456");
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
                var reader = command.ExecuteReader();
                string[] data = new string[100];

                while (reader.Read())
                {
                    data.Append(reader["result"]);
                }
                conn.Close();
                return data;
            });
            result = res;
        }

        // GET: api/Products
        [HttpGet]
        public IEnumerable<string> Get()
        {
            object result = null;
            GetData("GetProduct", result);
            return (IEnumerable<string>)result;
        }

        // GET: api/Products/5
        [HttpGet("{id}", Name = "Get")]
        public IEnumerable<string> Get(int id)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters["key"] = 1;
            object result = null;
            GetData("GetProduct", result, parameters);
            return (IEnumerable<string>)result;
        }

        // POST: api/Products
        [HttpPost]
        public string Post([FromBody] string value)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("key", value);
            object result = null;
            GetData("InsertProduct", result, parameters);
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
            object result = null;
            GetData("InsertProduct", result, parameters);
            return (string)result;
        }
    }
}
