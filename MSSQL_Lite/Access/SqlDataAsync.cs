using MSSQL_Lite.Connection;
using MSSQL_Lite.Execution;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSSQL_Lite.Access
{
    internal partial class SqlData : SqlExecution
    {
        public async Task ExecuteReaderAsync(SqlCommand sqlCommand)
        {
            if (objectReceivingData == ObjectReceivingData.SqlDataReader)
                data = await ExecuteReaderAsync<SqlDataReader>(sqlCommand);
            else
                data = await ExecuteReaderAsync<DataSet>(sqlCommand);
        }
    }
}
