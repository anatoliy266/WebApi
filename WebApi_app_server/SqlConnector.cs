using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi_app_server
{
    public class SqlConnector : ISqlConnector
    {
        public SqlConnector()
        {

        }

        public SqlConnection ConnectToDB(string host, string user, string password)
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.DataSource = "localhost\\SQL";
            builder.InitialCatalog = "WalletDataBase";
            builder.UserID = user;
            builder.Password = password;

            return new SqlConnection(builder.ConnectionString);
        }

        public object GetData(string storedProc, Dictionary<string, object> parameters = null)
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
    }
}
