using System.Data.Entity.SqlServer;

namespace HistoricalLib.Database
{
    internal class MissingDllHack
    {
        private static SqlProviderServices instance = SqlProviderServices.Instance;
    }
}
