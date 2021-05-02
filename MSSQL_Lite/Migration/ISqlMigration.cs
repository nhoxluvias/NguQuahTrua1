using System.Threading.Tasks;

namespace MSSQL_Lite.Migration
{
    public interface ISqlMigration
    {
        Task AddDataAndRunAsync();
        void AddDataAndRun();
    }
}
