using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace DataSummitModels.DB
{
    public partial class DataSummitDbContext : DbContext
    {
        // Required when using DI
        // This allows us to inject the context and pass the connection string in via the calling service's Startup.cs
        public DataSummitDbContext(DbContextOptions<DataSummitDbContext> options)
            : base(options)
        { }

        public DataSummitDbContext()
        {
            //A parameterless DBContext constructor is required for MockDbContext
            var optionsBuilder = new DbContextOptionsBuilder<DataSummitDbContext>();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        { }

        public virtual DbSet<Address> Addresses { get; set; }
        public virtual DbSet<AspNetRole> AspNetRoles { get; set; }
        public virtual DbSet<AspNetRoleClaim> AspNetRoleClaims { get; set; }
        public virtual DbSet<AspNetUser> AspNetUsers { get; set; }
        public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUserRole> AspNetUserRoles { get; set; }
        public virtual DbSet<AspNetUserToken> AspNetUserTokens { get; set; }
        public virtual DbSet<AzureCompanyResourceUrl> AzureCompanyResourceUrls { get; set; }
        public virtual DbSet<BlockPosition> BlockPositions { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Company> Companies { get; set; }
        public virtual DbSet<Country> Countries { get; set; }
        public virtual DbSet<Currency> Currencies { get; set; }
        public virtual DbSet<Document> Documents { get; set; }
        public virtual DbSet<DocumentFeature> DocumentFeatures { get; set; }
        public virtual DbSet<DocumentLayer> DocumentLayers { get; set; }
        public virtual DbSet<DocumentTemplate> DocumentTemplates { get; set; }
        public virtual DbSet<DocumentType> DocumentTypes { get; set; }
        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<EmployeeTerritory> EmployeeTerritories { get; set; }
        public virtual DbSet<Gender> Genders { get; set; }
        public virtual DbSet<GoogleLanguage> GoogleLanguages { get; set; }
        public virtual DbSet<ImageGrid> ImageGrids { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<OrderDetail> OrderDetails { get; set; }
        public virtual DbSet<PaperOrientation> PaperOrientations { get; set; }
        public virtual DbSet<PaperSize> PaperSizes { get; set; }
        public virtual DbSet<Point> Points { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Project> Projects { get; set; }
        public virtual DbSet<Property> Properties { get; set; }
        public virtual DbSet<Sentence> Sentences { get; set; }
        public virtual DbSet<StandardAttribute> StandardAttributes { get; set; }
        public virtual DbSet<Task> Tasks { get; set; }
        public virtual DbSet<TemplateAttribute> TemplateAttributes { get; set; }
        public virtual DbSet<TemplateVersion> TemplateVersions { get; set; }
        public virtual DbSet<UserInfo> UserInfos { get; set; }
        public virtual DbSet<UserInfoType> UserInfoTypes { get; set; }
        public virtual DbSet<UserType> UserTypes { get; set; }

        //        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //        {
        //            if (!optionsBuilder.IsConfigured)
        //            {
        //#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        //                optionsBuilder.UseSqlServer("Data Source=(localdb)\\ProjectsV13;Initial Catalog=DataSummitDB;Integrated Security=True");
        //            }
        //        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Address>(entity =>
            {
                entity.Property(e => e.County)
                    .HasMaxLength(31)
                    .IsFixedLength(true);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.NumberName).HasMaxLength(15);

                entity.Property(e => e.PostCode).HasMaxLength(10);

                entity.Property(e => e.Street).HasMaxLength(63);

                entity.Property(e => e.Street2).HasMaxLength(63);

                entity.Property(e => e.Street3).HasMaxLength(63);

                entity.Property(e => e.TownCity)
                    .HasMaxLength(31)
                    .IsFixedLength(true);

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

            modelBuilder.Entity<AspNetRole>(entity =>
            {
                //entity.HasIndex(e => e.NormalizedName, "RoleNameIndex");

                entity.Property(e => e.Id).HasMaxLength(128);

                entity.Property(e => e.Name).HasMaxLength(256);

                entity.Property(e => e.NormalizedName).HasMaxLength(256);
            });

            modelBuilder.Entity<AspNetRoleClaim>(entity =>
            {
                //entity.HasIndex(e => e.RoleId, "IX_AspNetRoleClaims_RoleId");

                entity.Property(e => e.RoleId)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AspNetRoleClaims)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("FK_AspNetRoleClaim_AspNetRole_RoleId");
            });

            modelBuilder.Entity<AspNetUser>(entity =>
            {
                //entity.HasIndex(e => e.NormalizedEmail, "EmailIndex");

                //entity.HasIndex(e => e.NormalizedUserName, "UserNameIndex").IsUnique();

                entity.Property(e => e.Id).HasMaxLength(50);

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

            modelBuilder.Entity<AspNetUserClaim>(entity =>
            {
                //entity.HasIndex(e => e.UserId, "IX_AspNetUserClaims_UserId");

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserClaims)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_AspNetUserClaim_AspNetUser_UserId");
            });

            modelBuilder.Entity<AspNetUserLogin>(entity =>
            {
                entity.HasKey(e => new { e.LoginProvider, e.ProviderKey })
                    .HasName("PK_AspNetUserLogin");

                //entity.HasIndex(e => e.UserId, "IX_AspNetUserLogins_UserId");

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

            modelBuilder.Entity<AspNetUserRole>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.RoleId })
                    .HasName("PK_AspNetUserRole");

                //entity.HasIndex(e => e.RoleId, "IX_AspNetUserRoles_RoleId");

                //entity.HasIndex(e => e.UserId, "IX_AspNetUserRoles_UserId");

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

            modelBuilder.Entity<AspNetUserToken>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name })
                    .HasName("PK_AspNetUserToken");

                entity.Property(e => e.UserId).HasMaxLength(150);

                entity.Property(e => e.LoginProvider).HasMaxLength(150);

                entity.Property(e => e.Name).HasMaxLength(150);
            });

            modelBuilder.Entity<AzureCompanyResourceUrl>(entity =>
            {
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
                    .HasMaxLength(511)
                    .HasColumnName("URL");

                entity.HasOne(d => d.Company)
                    .WithMany(p => p.AzureCompanyResourceUrls)
                    .HasForeignKey(d => d.CompanyId)
                    .HasConstraintName("FK_AzureCompanyResourceUrls_Companies");
            });

            modelBuilder.Entity<BlockPosition>(entity =>
            {
                entity.Property(e => e.BlockPositionId).ValueGeneratedOnAdd();

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(15);
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Description).HasColumnType("ntext");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(15);

                entity.Property(e => e.Picture).HasColumnType("image");
            });

            modelBuilder.Entity<Company>(entity =>
            {
                entity.Property(e => e.CompanyNumber)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(63);

                entity.Property(e => e.Region).HasMaxLength(31);

                entity.Property(e => e.ResourceGroup).HasMaxLength(63);

                entity.Property(e => e.Vatnumber)
                    .HasMaxLength(9)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.Website).HasMaxLength(2083);
            });

            modelBuilder.Entity<Country>(entity =>
            {
                entity.Property(e => e.CountryId).ValueGeneratedNever();

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Iso)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("ISO")
                    .IsFixedLength(true);

                entity.Property(e => e.Iso3)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("ISO3")
                    .IsFixedLength(true);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(127);

                entity.Property(e => e.Numcode).HasColumnName("numcode");

                entity.Property(e => e.Phonecode).HasColumnName("phonecode");

                entity.Property(e => e.SentenceCaseName)
                    .IsRequired()
                    .HasMaxLength(127);
            });

            modelBuilder.Entity<Currency>(entity =>
            {
                entity.Property(e => e.AlphabeticCode)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Entity)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.MinorUnit)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.NumericCode)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .IsFixedLength(true);
            });

            modelBuilder.Entity<Document>(entity =>
            {
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

                entity.HasOne(d => d.DocumentType)
                    .WithMany(p => p.Documents)
                    .HasForeignKey(d => d.DocumentTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Documents_DocumentTypes");

                entity.HasOne(d => d.PaperOrientation)
                    .WithMany(p => p.Documents)
                    .HasForeignKey(d => d.PaperOrientationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Documents_PaperOrientations");

                entity.HasOne(d => d.PaperSize)
                    .WithMany(p => p.Documents)
                    .HasForeignKey(d => d.PaperSizeId)
                    .HasConstraintName("FK_Documents_PaperSizes");

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.Documents)
                    .HasForeignKey(d => d.ProjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Documents_Projects");

                entity.HasOne(d => d.TemplateVersion)
                    .WithMany(p => p.Documents)
                    .HasForeignKey(d => d.TemplateVersionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Documents_TemplateVersions");
            });

            modelBuilder.Entity<DocumentFeature>(entity =>
            {
                entity.Property(e => e.Value)
                    .IsRequired()
                    .HasMaxLength(1023);

                entity.Property(e => e.Vendor)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.HasOne(d => d.Document)
                    .WithMany(p => p.DocumentFeatures)
                    .HasForeignKey(d => d.DocumentId)
                    .HasConstraintName("FK_DocumentFeatures_Documents");
            });

            modelBuilder.Entity<DocumentLayer>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(1023);

                entity.HasOne(d => d.Document)
                    .WithMany(p => p.DocumentLayers)
                    .HasForeignKey(d => d.DocumentId)
                    .HasConstraintName("FK_DocumentLayers_Documents");
            });

            modelBuilder.Entity<DocumentTemplate>(entity =>
            {
                entity.Property(e => e.DocumentTemplateId).ValueGeneratedNever();

                entity.HasOne(d => d.Document)
                    .WithMany(p => p.DocumentTemplates)
                    .HasForeignKey(d => d.DocumentId)
                    .HasConstraintName("FK_DocumentTemplates_Documents");

                entity.HasOne(d => d.TemplateVersion)
                    .WithMany(p => p.DocumentTemplates)
                    .HasForeignKey(d => d.TemplateVersionId)
                    .HasConstraintName("FK_DocumentTemplates_TemplateVersions");
            });

            modelBuilder.Entity<DocumentType>(entity =>
            {
                entity.Property(e => e.DocumentTypeId).ValueGeneratedOnAdd();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(15);
            });

            modelBuilder.Entity<Employee>(entity =>
            {
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

            modelBuilder.Entity<EmployeeTerritory>(entity =>
            {
                entity.HasKey(e => new { e.EmployeeId, e.TerritoryId });

                entity.Property(e => e.TerritoryId).HasMaxLength(20);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<Gender>(entity =>
            {
                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasMaxLength(15);

                entity.Property(e => e.UserId).HasMaxLength(50);
            });

            modelBuilder.Entity<GoogleLanguage>(entity =>
            {
                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsFixedLength(true);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(15);

                entity.Property(e => e.Notes).HasMaxLength(127);
            });

            modelBuilder.Entity<ImageGrid>(entity =>
            {
                entity.Property(e => e.BlobUrl)
                    .IsRequired()
                    .HasMaxLength(2083)
                    .HasColumnName("BlobURL");

                entity.Property(e => e.Name).IsRequired();

                entity.HasOne(d => d.Document)
                    .WithMany(p => p.ImageGrids)
                    .HasForeignKey(d => d.DocumentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ImageGrids_Documents");
            });

            modelBuilder.Entity<Order>(entity =>
            {
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

            modelBuilder.Entity<OrderDetail>(entity =>
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

            modelBuilder.Entity<PaperOrientation>(entity =>
            {
                entity.Property(e => e.Orientation)
                    .IsRequired()
                    .HasMaxLength(9);
            });

            modelBuilder.Entity<PaperSize>(entity =>
            {
                entity.Property(e => e.PaperSizeId).ValueGeneratedOnAdd();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(31);

                entity.Property(e => e.PhysicalHeight).HasColumnType("decimal(6, 1)");

                entity.Property(e => e.PhysicalWidth).HasColumnType("decimal(6, 1)");
            });

            modelBuilder.Entity<Point>(entity =>
            {
                entity.Property(e => e.PointId).ValueGeneratedNever();
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(63);

                entity.Property(e => e.QuantityPerUnit).HasMaxLength(31);

                entity.Property(e => e.UnitPrice).HasColumnType("money");
            });

            modelBuilder.Entity<Project>(entity =>
            {
                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(1024);

                entity.Property(e => e.Region).HasMaxLength(31);

                entity.Property(e => e.StorageAccountKey)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.StorageAccountName)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.HasOne(d => d.Company)
                    .WithMany(p => p.Projects)
                    .HasForeignKey(d => d.CompanyId)
                    .HasConstraintName("FK_Projects_Companies");
            });

            modelBuilder.Entity<Property>(entity =>
            {
                entity.HasOne(d => d.Sentence)
                    .WithMany(p => p.Properties)
                    .HasForeignKey(d => d.SentenceId)
                    .HasConstraintName("FK_Properties_Sentences");

                entity.HasOne(d => d.TemplateAttribute)
                    .WithMany(p => p.Properties)
                    .HasForeignKey(d => d.TemplateAttributeId)
                    .HasConstraintName("FK_Properties_TemplateAttributes");
            });

            modelBuilder.Entity<Sentence>(entity =>
            {
                entity.Property(e => e.SentenceId).ValueGeneratedNever();

                entity.Property(e => e.Confidence).HasColumnType("decimal(3, 2)");

                entity.Property(e => e.SlendernessRatio)
                    .HasColumnType("decimal(18, 0)")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Vendor)
                    .IsRequired()
                    .HasMaxLength(63);

                entity.Property(e => e.Words).IsRequired();

                entity.HasOne(d => d.Document)
                    .WithMany(p => p.Sentences)
                    .HasForeignKey(d => d.DocumentId)
                    .HasConstraintName("FK_Sentences_Documents");
            });

            modelBuilder.Entity<StandardAttribute>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<Task>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.TimeStamp).HasColumnType("datetime");

                entity.HasOne(d => d.Document)
                    .WithMany(p => p.Tasks)
                    .HasForeignKey(d => d.DocumentId)
                    .HasConstraintName("FK_Tasks_Document");
            });

            modelBuilder.Entity<TemplateAttribute>(entity =>
            {
                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.HasOne(d => d.BlockPosition)
                    .WithMany(p => p.TemplateAttributes)
                    .HasForeignKey(d => d.BlockPositionId)
                    .HasConstraintName("FK_TemplateAttributes_BlockPositions");

                entity.HasOne(d => d.PaperSize)
                    .WithMany(p => p.TemplateAttributes)
                    .HasForeignKey(d => d.PaperSizeId)
                    .HasConstraintName("FK_TemplateAttributes_PaperSizes");

                entity.HasOne(d => d.StandardAttribute)
                    .WithMany(p => p.TemplateAttributes)
                    .HasForeignKey(d => d.StandardAttributeId)
                    .HasConstraintName("FK_TemplateAttributes_StandardAttributes");

                entity.HasOne(d => d.TemplateVersion)
                    .WithMany(p => p.TemplateAttributes)
                    .HasForeignKey(d => d.TemplateVersionId)
                    .HasConstraintName("FK_TemplateAttributes_TemplateVersions");
            });

            modelBuilder.Entity<TemplateVersion>(entity =>
            {
                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Image).HasColumnType("image");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(1023);

                entity.HasOne(d => d.Company)
                    .WithMany(p => p.TemplateVersions)
                    .HasForeignKey(d => d.CompanyId)
                    .HasConstraintName("FK_TemplateVersions_Companies");
            });

            modelBuilder.Entity<UserInfo>(entity =>
            {
                entity.HasKey(e => e.UserInfoId);

                entity.ToTable("UserInfo");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Id).HasMaxLength(50);

                entity.Property(e => e.Value)
                    .IsRequired()
                    .HasMaxLength(63);

                entity.HasOne(d => d.IdNavigation)
                    .WithMany(p => p.UserInfos)
                    .HasForeignKey(d => d.Id)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_UserInfo_AspNetUsers");

                entity.HasOne(d => d.UserInfoType)
                    .WithMany(p => p.UserInfos)
                    .HasForeignKey(d => d.UserInfoTypeId)
                    .HasConstraintName("FK_UserInfo_UserInfoTypes");
            });

            modelBuilder.Entity<UserInfoType>(entity =>
            {
                entity.Property(e => e.UserInfoTypeId).ValueGeneratedOnAdd();

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasMaxLength(63);
            });

            modelBuilder.Entity<UserType>(entity =>
            {
                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Value)
                    .IsRequired()
                    .HasMaxLength(15);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}