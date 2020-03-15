using DataSummitModels.DB.Paper;
using Microsoft.EntityFrameworkCore;

namespace DataSummitModels.DB
{
    public class DataSummitDbContext : DbContext
    {
        //private string connDevString = @"Data Source=(localdb)\ProjectsV13;Initial Catalog=DataSummitDB;Integrated Security=True";
        private string connProdString = @"Server=tcp:datasummit.database.windows.net,1433;Initial Catalog=DataSummitDB;Persist Security Info=False;User ID=TomJames;Password=!Aa1234567;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
        private bool OnConfigureIsProductionDb = false;

        public DataSummitDbContext(DbContextOptions<DataSummitDbContext> options)
            : base(options)
        {
        }

        public DataSummitDbContext()
        {
            //A parameterless DBContext constructor is required for MockDbContext
            var optionsBuilder = new DbContextOptionsBuilder<DataSummitDbContext>();
        }

        public DataSummitDbContext(bool IsProdEnvironment = false)
        {
            //Manually added by TJ. May not be the correct method to initialise the database/context connection
            var optionsBuilder = new DbContextOptionsBuilder<DataSummitDbContext>();
            if (IsProdEnvironment)
            {
                // This internal bool is required as "protected override void OnConfiguring" cannot pass 
                // "bool IsProdEnvironment = false" parameter
                OnConfigureIsProductionDb = true;
                optionsBuilder.UseSqlServer(connProdString);
            }
            else
            { optionsBuilder.UseSqlServer(connProdString); }

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // Uses local bool to determine whether development or production environment
                // is actvie
                if (OnConfigureIsProductionDb)
                { optionsBuilder.UseSqlServer(connProdString); }
                else
                { optionsBuilder.UseSqlServer(connProdString); }
            }
        }

        public virtual DbSet<Addresses> Addresses { get; set; }
        public virtual DbSet<AspNetRoleClaims> AspNetRoleClaims { get; set; }
        public virtual DbSet<AspNetRoles> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUserClaims> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogins> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUserRoles> AspNetUserRoles { get; set; }
        public virtual DbSet<AspNetUserTokens> AspNetUserTokens { get; set; }
        public virtual DbSet<AspNetUsers> AspNetUsers { get; set; }
        public virtual DbSet<AzureCompanyResourceUrls> AzureCompanyResourceUrls { get; set; }
        public virtual DbSet<BlockPositions> BlockPositions { get; set; }
        public virtual DbSet<Categories> Categories { get; set; }
        public virtual DbSet<Companies> Companies { get; set; }
        public virtual DbSet<Countries> Countries { get; set; }
        public virtual DbSet<Currencies> Currencies { get; set; }
        public virtual DbSet<DrawingFeatures> DrawingFeatures { get; set; }
        public virtual DbSet<DrawingLayers> DrawingLayers { get; set; }
        public virtual DbSet<Drawings> Drawings { get; set; }
        public virtual DbSet<EmployeeTerritories> EmployeeTerritories { get; set; }
        public virtual DbSet<Employees> Employees { get; set; }
        public virtual DbSet<Genders> Genders { get; set; }
        public virtual DbSet<GoogleLanguages> GoogleLanguages { get; set; }
        public virtual DbSet<ImageGrids> ImageGrids { get; set; }
        public virtual DbSet<OrderDetails> OrderDetails { get; set; }
        public virtual DbSet<Orders> Orders { get; set; }
        public virtual DbSet<PaperOrientations> PaperOrientations { get; set; }
        public virtual DbSet<PaperSizes> PaperSizes { get; set; }
        public virtual DbSet<Products> Products { get; set; }
        public virtual DbSet<ProfileAttributes> ProfileAttributes { get; set; }
        public virtual DbSet<ProfileVersions> ProfileVersions { get; set; }
        public virtual DbSet<Projects> Projects { get; set; }
        public virtual DbSet<Properties> Properties { get; set; }
        public virtual DbSet<Sentences> Sentences { get; set; }
        public virtual DbSet<StandardAttributes> StandardAttributes { get; set; }
        public virtual DbSet<Tasks> Tasks { get; set; }
        public virtual DbSet<UserInfo> UserInfo { get; set; }
        public virtual DbSet<UserInfoTypes> UserInfoTypes { get; set; }
        public virtual DbSet<UserTypes> UserTypes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.6-servicing-10079");

            modelBuilder.Entity<Addresses>(entity =>
            {
                entity.HasKey(e => e.AddressId);

                entity.Property(e => e.County).HasMaxLength(31);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.NumberName).HasMaxLength(15);

                entity.Property(e => e.PostCode).HasMaxLength(10);

                entity.Property(e => e.Street).HasMaxLength(63);

                entity.Property(e => e.Street2).HasMaxLength(63);

                entity.Property(e => e.Street3).HasMaxLength(63);

                entity.Property(e => e.TownCity).HasMaxLength(31);

                entity.HasOne(d => d.Company)
                    .WithMany(p => p.Addresses)
                    .HasForeignKey(d => d.CompanyId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_Addresses_Company");

                entity.HasOne(d => d.Country)
                    .WithMany(p => p.Addresses)
                    .HasForeignKey(d => d.CountryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Addresses_Country");

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.Addresses)
                    .HasForeignKey(d => d.ProjectId)
                    .HasConstraintName("FK_Addresses_Project");
            });

            modelBuilder.Entity<AspNetRoleClaims>(entity =>
            {
                entity.HasIndex(e => e.RoleId);

                entity.Property(e => e.RoleId)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AspNetRoleClaims)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("FK_AspNetRoleClaim_AspNetRole_RoleId");
            });

            modelBuilder.Entity<AspNetRoles>(entity =>
            {
                entity.HasIndex(e => e.NormalizedName)
                    .HasName("RoleNameIndex");

                entity.Property(e => e.Id)
                    .HasMaxLength(128)
                    .ValueGeneratedNever();

                entity.Property(e => e.Name).HasMaxLength(256);

                entity.Property(e => e.NormalizedName).HasMaxLength(256);
            });

            modelBuilder.Entity<AspNetUserClaims>(entity =>
            {
                entity.HasIndex(e => e.UserId);

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserClaims)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_AspNetUserClaim_AspNetUser_UserId");
            });

            modelBuilder.Entity<AspNetUserLogins>(entity =>
            {
                entity.HasKey(e => new { e.LoginProvider, e.ProviderKey })
                    .HasName("PK_AspNetUserLogin");

                entity.HasIndex(e => e.UserId);

                entity.Property(e => e.LoginProvider).HasMaxLength(128);

                entity.Property(e => e.ProviderKey).HasMaxLength(128);

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserLogins)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_AspNetUserLogin_AspNetUser_UserId");
            });

            modelBuilder.Entity<AspNetUserRoles>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.RoleId })
                    .HasName("PK_AspNetUserRole");

                entity.HasIndex(e => e.RoleId);

                entity.HasIndex(e => e.UserId);

                entity.Property(e => e.UserId).HasMaxLength(50);

                entity.Property(e => e.RoleId).HasMaxLength(128);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AspNetUserRoles)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("FK_AspNetUserRole_AspNetRole_RoleId");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserRoles)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_AspNetUserRole_AspNetUser_UserId");
            });

            modelBuilder.Entity<AspNetUserTokens>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name })
                    .HasName("PK_AspNetUserToken");

                entity.Property(e => e.UserId).HasMaxLength(150);

                entity.Property(e => e.LoginProvider).HasMaxLength(150);

                entity.Property(e => e.Name).HasMaxLength(150);
            });

            modelBuilder.Entity<AspNetUsers>(entity =>
            {
                entity.HasIndex(e => e.NormalizedEmail)
                    .HasName("EmailIndex");

                entity.HasIndex(e => e.NormalizedUserName)
                    .HasName("UserNameIndex")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasMaxLength(50)
                    .ValueGeneratedNever();

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.DateOfBirth).HasColumnType("datetime");

                entity.Property(e => e.Email).HasMaxLength(256);

                entity.Property(e => e.FirstName).HasMaxLength(50);

                entity.Property(e => e.GenderId).HasDefaultValueSql("((1))");

                entity.Property(e => e.IsTrial).HasDefaultValueSql("((1))");

                entity.Property(e => e.MiddleNames).HasMaxLength(63);

                entity.Property(e => e.NormalizedEmail).HasMaxLength(256);

                entity.Property(e => e.NormalizedUserName).HasMaxLength(256);

                entity.Property(e => e.Photo).HasColumnType("image");

                entity.Property(e => e.PhotoPath).HasMaxLength(255);

                entity.Property(e => e.PositionTitle).HasMaxLength(50);

                entity.Property(e => e.Surname).HasMaxLength(50);

                entity.Property(e => e.Title).HasMaxLength(30);

                entity.Property(e => e.UserName).HasMaxLength(256);

                entity.HasOne(d => d.Company)
                    .WithMany(p => p.AspNetUsers)
                    .HasForeignKey(d => d.CompanyId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_AspNetUsers_Companies");

                entity.HasOne(d => d.Gender)
                    .WithMany(p => p.AspNetUsers)
                    .HasForeignKey(d => d.GenderId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_AspNetUsers_Genders");

                entity.HasOne(d => d.UserType)
                    .WithMany(p => p.AspNetUsers)
                    .HasForeignKey(d => d.UserTypeId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_AspNetUsers_UserTypes");
            });

            modelBuilder.Entity<AzureCompanyResourceUrls>(entity =>
            {
                entity.HasKey(e => e.AzureCompanyResourceUrlId)
                    .HasName("PK_AzureCompanyResourceUrlId");

                entity.Property(e => e.Key)
                    .IsRequired()
                    .HasMaxLength(511);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.ResourceType)
                    .IsRequired()
                    .HasMaxLength(63);

                entity.Property(e => e.Url)
                    .IsRequired()
                    .HasColumnName("URL")
                    .HasMaxLength(511);

                entity.HasOne(d => d.Company)
                    .WithMany(p => p.AzureCompanyResourceUrls)
                    .HasForeignKey(d => d.CompanyId)
                    .HasConstraintName("FK_AzureCompanyResourceUrls_Companies");
            });

            modelBuilder.Entity<BlockPositions>(entity =>
            {
                entity.HasKey(e => e.BlockPositionId);

                entity.Property(e => e.BlockPositionId).ValueGeneratedOnAdd();

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(15);
            });

            modelBuilder.Entity<Categories>(entity =>
            {
                entity.HasKey(e => e.CategoryId);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Description).HasColumnType("ntext");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(15);

                entity.Property(e => e.Picture).HasColumnType("image");
            });

            modelBuilder.Entity<Companies>(entity =>
            {
                entity.HasKey(e => e.CompanyId);

                entity.Property(e => e.CompanyNumber)
                    .HasMaxLength(8)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(40);

                entity.Property(e => e.Vatnumber)
                    .HasMaxLength(9)
                    .IsUnicode(false);

                entity.Property(e => e.Website).HasMaxLength(2083);
            });

            modelBuilder.Entity<Countries>(entity =>
            {
                entity.HasKey(e => e.CountryId);

                entity.Property(e => e.CountryId).ValueGeneratedNever();

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Iso)
                    .IsRequired()
                    .HasColumnName("ISO")
                    .HasMaxLength(3)
                    .IsUnicode(false);

                entity.Property(e => e.Iso3)
                    .HasColumnName("ISO3")
                    .HasMaxLength(3)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(127);

                entity.Property(e => e.Numcode).HasColumnName("numcode");

                entity.Property(e => e.Phonecode).HasColumnName("phonecode");

                entity.Property(e => e.SentenceCaseName)
                    .IsRequired()
                    .HasMaxLength(127);
            });

            modelBuilder.Entity<Currencies>(entity =>
            {
                entity.HasKey(e => e.CurrencyId);

                entity.Property(e => e.AlphabeticCode)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Entity)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.MinorUnit)
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.NumericCode)
                    .HasMaxLength(3)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<DrawingFeatures>(entity =>
            {
                entity.HasKey(e => e.DrawingFeatureId)
                    .HasName("PK_DrawingFeatureId");

                entity.Property(e => e.Value)
                    .IsRequired()
                    .HasMaxLength(1023);

                entity.Property(e => e.Vendor)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.HasOne(d => d.Drawing)
                    .WithMany(p => p.DrawingFeatures)
                    .HasForeignKey(d => d.DrawingId)
                    .HasConstraintName("FK_DrawingFeatures_Drawings");
            });

            modelBuilder.Entity<DrawingLayers>(entity =>
            {
                entity.HasKey(e => e.DrawingLayerId)
                    .HasName("PK_DrawingLayerId");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(1023);

                entity.HasOne(d => d.Drawing)
                    .WithMany(p => p.Layers)
                    .HasForeignKey(d => d.DrawingId)
                    .HasConstraintName("FK_DrawingLayers_Drawings");
            });

            modelBuilder.Entity<Drawings>(entity =>
            {
                entity.HasKey(e => e.DrawingId)
                    .HasName("PK_Drawing");

                entity.Property(e => e.AmazonConfidence).HasColumnType("decimal(3, 2)");

                entity.Property(e => e.AzureConfidence).HasColumnType("decimal(3, 2)");

                entity.Property(e => e.BlobUrl)
                    .IsRequired()
                    .HasMaxLength(1023);

                entity.Property(e => e.ContainerName)
                    .IsRequired()
                    .HasMaxLength(36);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.FileName)
                    .IsRequired()
                    .HasMaxLength(1023);

                entity.Property(e => e.GoogleConfidence).HasColumnType("decimal(3, 2)");

                entity.Property(e => e.Type).HasMaxLength(7);

                entity.Property(e => e.UserId).HasMaxLength(50);

                entity.HasOne(d => d.PaperOrientation)
                    .WithMany(p => p.Drawings)
                    .HasForeignKey(d => d.PaperOrientationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Drawings_PaperOrientations");

                entity.HasOne(d => d.PaperSize)
                    .WithMany(p => p.Drawings)
                    .HasForeignKey(d => d.PaperSizeId)
                    .HasConstraintName("FK_Drawings_PaperSizes");

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.Drawings)
                    .HasForeignKey(d => d.ProjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Drawings_Projects");
            });

            modelBuilder.Entity<EmployeeTerritories>(entity =>
            {
                entity.HasKey(e => new { e.EmployeeId, e.TerritoryId });

                entity.Property(e => e.TerritoryId).HasMaxLength(20);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<Employees>(entity =>
            {
                entity.HasKey(e => e.EmployeeId);

                entity.Property(e => e.EmployeeId).ValueGeneratedNever();

                entity.Property(e => e.BirthDate).HasColumnType("datetime");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(31);

                entity.Property(e => e.HireDate).HasColumnType("datetime");

                entity.Property(e => e.JobTitle).HasMaxLength(63);

                entity.Property(e => e.MiddleNames).HasMaxLength(63);

                entity.Property(e => e.Notes).HasColumnType("ntext");

                entity.Property(e => e.Photo).HasColumnType("image");

                entity.Property(e => e.PhotoPath).HasMaxLength(255);

                entity.Property(e => e.Surname)
                    .IsRequired()
                    .HasMaxLength(31);

                entity.Property(e => e.Title).HasMaxLength(51);

                entity.Property(e => e.TitleOfCourtesy).HasMaxLength(31);

                entity.HasOne(d => d.Gender)
                    .WithMany(p => p.Employees)
                    .HasForeignKey(d => d.GenderId)
                    .HasConstraintName("FK_Employees_Genders");
            });

            modelBuilder.Entity<Genders>(entity =>
            {
                entity.HasKey(e => e.GenderId);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasMaxLength(15);

                entity.Property(e => e.UserId).HasMaxLength(50);
            });

            modelBuilder.Entity<GoogleLanguages>(entity =>
            {
                entity.HasKey(e => e.GoogleLanguageId);

                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(3);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(15);

                entity.Property(e => e.Notes).HasMaxLength(127);
            });

            modelBuilder.Entity<ImageGrids>(entity =>
            {
                entity.HasKey(e => e.ImageGridId);

                entity.Property(e => e.BlobUrl)
                    .IsRequired()
                    .HasColumnName("BlobURL")
                    .HasMaxLength(2083);

                entity.Property(e => e.Name).IsRequired();

                entity.HasOne(d => d.Drawing)
                    .WithMany(p => p.ImageGrids)
                    .HasForeignKey(d => d.DrawingId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ImageGrids_Drawings");
            });

            modelBuilder.Entity<OrderDetails>(entity =>
            {
                entity.HasKey(e => new { e.OrderId, e.ProductId })
                    .HasName("PK_Order_Detail");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Quantity).HasDefaultValueSql("((1))");

                entity.Property(e => e.UnitPrice).HasColumnType("money");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.OrderDetails)
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OrderDetail_Order");
            });

            modelBuilder.Entity<Orders>(entity =>
            {
                entity.HasKey(e => e.OrderId);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Freight)
                    .HasColumnType("money")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.OrderDate).HasColumnType("datetime");

                entity.Property(e => e.RequiredDate).HasColumnType("datetime");

                entity.Property(e => e.ShipAddress).HasMaxLength(60);

                entity.Property(e => e.ShipCity).HasMaxLength(15);

                entity.Property(e => e.ShipCountry).HasMaxLength(15);

                entity.Property(e => e.ShipName).HasMaxLength(40);

                entity.Property(e => e.ShipPostalCode).HasMaxLength(10);

                entity.Property(e => e.ShipRegion).HasMaxLength(15);

                entity.Property(e => e.ShippedDate).HasColumnType("datetime");

                entity.HasOne(d => d.Company)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.CompanyId)
                    .HasConstraintName("FK_Orders_Companies");
            });

            modelBuilder.Entity<PaperOrientations>(entity =>
            {
                entity.HasKey(e => e.PaperOrientationId)
                    .HasName("PK__PaperOri__22B4482474AC5E87");

                entity.Property(e => e.Orientation)
                    .IsRequired()
                    .HasMaxLength(9);
            });

            modelBuilder.Entity<PaperSizes>(entity =>
            {
                entity.HasKey(e => e.PaperSizeId);

                entity.Property(e => e.PaperSizeId).ValueGeneratedOnAdd();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(31);

                entity.Property(e => e.PhysicalHeight).HasColumnType("decimal(6, 1)");

                entity.Property(e => e.PhysicalWidth).HasColumnType("decimal(6, 1)");
            });

            modelBuilder.Entity<Products>(entity =>
            {
                entity.HasKey(e => e.ProductId);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(63);

                entity.Property(e => e.QuantityPerUnit).HasMaxLength(31);

                entity.Property(e => e.UnitPrice).HasColumnType("money");
            });

            modelBuilder.Entity<ProfileAttributes>(entity =>
            {
                entity.HasKey(e => e.ProfileAttributeId);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.HasOne(d => d.BlockPosition)
                    .WithMany(p => p.ProfileAttributes)
                    .HasForeignKey(d => d.BlockPositionId)
                    .HasConstraintName("FK_ProfileAttributes_BlockPositions");

                entity.HasOne(d => d.PaperSize)
                    .WithMany(p => p.ProfileAttributes)
                    .HasForeignKey(d => d.PaperSizeId)
                    .HasConstraintName("FK_ProfileAttributes_PaperSizes");

                entity.HasOne(d => d.ProfileVersion)
                    .WithMany(p => p.ProfileAttributes)
                    .HasForeignKey(d => d.ProfileVersionId)
                    .HasConstraintName("FK_ProfileAttributes_ProfileVersions");

                entity.HasOne(d => d.StandardAttribute)
                    .WithMany(p => p.ProfileAttributes)
                    .HasForeignKey(d => d.StandardAttributeId)
                    .HasConstraintName("FK_ProfileAttributes_StandardAttributes");
            });

            modelBuilder.Entity<ProfileVersions>(entity =>
            {
                entity.HasKey(e => e.ProfileVersionId)
                    .HasName("PK_ProfileVersionId");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Image).HasColumnType("image");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(1023);

                entity.HasOne(d => d.Company)
                    .WithMany(p => p.ProfileVersions)
                    .HasForeignKey(d => d.CompanyId)
                    .HasConstraintName("FK_ProfileVersions_Companies");
            });

            modelBuilder.Entity<Projects>(entity =>
            {
                entity.HasKey(e => e.ProjectId)
                    .HasName("PK_ProjectId");

                entity.Property(e => e.BlobStorageKey)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.BlobStorageName)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(1024);

                entity.HasOne(d => d.Company)
                    .WithMany(p => p.Projects)
                    .HasForeignKey(d => d.CompanyId)
                    .HasConstraintName("FK_Projects_Companies");
            });

            modelBuilder.Entity<Properties>(entity =>
            {
                entity.HasKey(e => e.PropertyId);

                entity.HasOne(d => d.ProfileAttribute)
                    .WithMany(p => p.Properties)
                    .HasForeignKey(d => d.ProfileAttributeId)
                    .HasConstraintName("FK_Properties_ProfileAttributes");

                entity.HasOne(d => d.Sentence)
                    .WithMany(p => p.Properties)
                    .HasForeignKey(d => d.SentenceId)
                    .HasConstraintName("FK_Properties_Sentences");
            });

            modelBuilder.Entity<Sentences>(entity =>
            {
                entity.HasKey(e => e.SentenceId)
                    .HasName("PK_SentenceId");

                entity.Property(e => e.SentenceId).ValueGeneratedNever();

                entity.Property(e => e.Confidence).HasColumnType("decimal(3, 2)");

                entity.Property(e => e.SlendernessRatio)
                    .HasColumnType("decimal(18, 0)")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Vendor)
                    .IsRequired()
                    .HasMaxLength(63);

                entity.Property(e => e.Words).IsRequired();

                entity.HasOne(d => d.Drawing)
                    .WithMany(p => p.Sentences)
                    .HasForeignKey(d => d.DrawingId)
                    .HasConstraintName("FK_Sentences_Drawings");
            });

            modelBuilder.Entity<StandardAttributes>(entity =>
            {
                entity.HasKey(e => e.StandardAttributeId);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<Tasks>(entity =>
            {
                entity.HasKey(e => e.TaskId)
                    .HasName("PK_TaskId");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.TimeStamp).HasColumnType("datetime");

                entity.HasOne(d => d.Drawing)
                    .WithMany(p => p.Tasks)
                    .HasForeignKey(d => d.DrawingId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Tasks_Drawing");
            });

            modelBuilder.Entity<UserInfo>(entity =>
            {
                entity.HasKey(e => e.UserInfoId);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Id).HasMaxLength(50);

                entity.Property(e => e.Value)
                    .IsRequired()
                    .HasMaxLength(63);

                entity.HasOne(d => d.IdNavigation)
                    .WithMany(p => p.UserInfo)
                    .HasForeignKey(d => d.Id)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_UserInfo_AspNetUsers");

                entity.HasOne(d => d.UserInfoType)
                    .WithMany(p => p.UserInfo)
                    .HasForeignKey(d => d.UserInfoTypeId)
                    .HasConstraintName("FK_UserInfo_UserInfoTypes");
            });

            modelBuilder.Entity<UserInfoTypes>(entity =>
            {
                entity.HasKey(e => e.UserInfoTypeId);

                entity.Property(e => e.UserInfoTypeId).ValueGeneratedOnAdd();

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasMaxLength(63);
            });

            modelBuilder.Entity<UserTypes>(entity =>
            {
                entity.HasKey(e => e.UserTypeId);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Value)
                    .IsRequired()
                    .HasMaxLength(15);
            });
        }
    }
}