using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HistoricalLib
{
    public class DatabaseConfiguration : DbConfiguration
    {
        public DatabaseConfiguration()
        {
            SetExecutionStrategy("System.Data.SqlClient", () => new DefaultExecutionStrategy());
            SetDefaultConnectionFactory(new LocalDbConnectionFactory("mssqllocaldb"));
        }
    }
}
