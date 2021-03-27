using Microsoft.EntityFrameworkCore;

namespace DataSummitDbModels.Context
{
    public partial class DatasummitdbContext : DbContext
    {
        public DatasummitdbContext()
        {
        }

        public DatasummitdbContext(DbContextOptions<DatasummitdbContext> options)
            : base(options)
        {
            
        }
    }
}