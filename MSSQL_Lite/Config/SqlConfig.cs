using MSSQL_Lite.Access;
using MSSQL_Lite.Connection;

namespace MSSQL_Lite.Config
{
    public static class SqlConfig
    {
        public static ObjectReceivingData objectReceivingData = ObjectReceivingData.DataSet;
        public static ConnectionType connectionType = ConnectionType.ManuallyDisconnect;
    }
}
