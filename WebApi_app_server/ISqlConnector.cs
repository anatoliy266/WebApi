using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi_app_server
{
    public interface ISqlConnector
    {
        SqlConnection ConnectToDB(string host, string user, string password);
        object GetData(string storedProc, Dictionary<string, object> parameters = null);
    }
}
