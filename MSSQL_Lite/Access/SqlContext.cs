using MSSQL_Lite.Execution;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSSQL_Lite.Access
{
    public class SqlContext
    {
        public enum ObjectReceivingData { DataSet, SqlDataReader, Dictionary };
        public SqlContext()
        {

        }

        public async Task<int> QueryAsync(string query)
        {
            return await SqlData.ExecuteNonQueryAsync(query);
        }

        public void Query(string query, ObjectReceivingData objectReceivingData)
        {

        }
    }
}
