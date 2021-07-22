using MSSQL_Lite.Config;
using MSSQL_Lite.Execution;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace MSSQL_Lite.Access
{
    internal partial class SqlData : SqlExecution
    {
        public async Task ExecuteReaderAsync(SqlCommand sqlCommand)
        {
            if (SqlConfig.objectReceivingData == ObjectReceivingData.SqlDataReader)
                data = await ExecuteReaderAsync<SqlDataReader>(sqlCommand);
            else
                data = await ExecuteReaderAsync<DataSet>(sqlCommand);
        }
    }
}
