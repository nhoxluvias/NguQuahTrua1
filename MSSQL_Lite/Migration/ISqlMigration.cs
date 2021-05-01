using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSSQL_Lite.Migration
{
    public interface ISqlMigration
    {
        void AddDataAndRun();
    }
}
