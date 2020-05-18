using System.Data.Entity;

namespace HistoricalLib.Database
{
    public class ModelContext : DbContext
    {
        public ModelContext() : base("Baza")
        {
            //Database.Initialize(false);
            Database.CreateIfNotExists();
        }

        public DbSet<Model> DataModels { get; set; }
    }
}
