using DataSummitDbModels;
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

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