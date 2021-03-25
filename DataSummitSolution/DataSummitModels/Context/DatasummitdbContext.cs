﻿using Microsoft.EntityFrameworkCore;

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

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {

#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Data Source=(localdb)\\ProjectsV13;Initial Catalog=DataSummitDB;Integrated Security=True;");
            }
        }
    }
}