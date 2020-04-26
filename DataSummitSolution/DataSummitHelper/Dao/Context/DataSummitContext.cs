using Microsoft.EntityFrameworkCore;
using DsDb = DataSummitModels.DB;
namespace DataSummitHelper.Dao.Context
{
    public class DataSummitContext : DbContext
    {
        public DataSummitContext()
        { }

        // Required when using DI
        public DataSummitContext(DbContextOptions<DataSummitContext> dbContextOptions)
            : base(dbContextOptions)
        { }

        public virtual DbSet<DsDb.Drawings> Drawings { get; set; }
        public virtual DbSet<DsDb.ProfileVersions> ProfileVersions { get; set; }
    }
}