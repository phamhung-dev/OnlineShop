using System.Data.Entity;

namespace Model.EF
{
    public partial class OnlineShopDataContext : DbContext
    {
        public OnlineShopDataContext()
            : base("name=OnlineShopDataContext")
        {
        }

        public virtual DbSet<Age> Ages { get; set; }
        public virtual DbSet<Color> Colors { get; set; }
        public virtual DbSet<Contact> Contacts { get; set; }
        public virtual DbSet<EmployeeAccount> EmployeeAccounts { get; set; }
        public virtual DbSet<Evaluation> Evaluations { get; set; }
        public virtual DbSet<Favourite> Favourites { get; set; }
        public virtual DbSet<Feedback> Feedbacks { get; set; }
        public virtual DbSet<Invoice> Invoices { get; set; }
        public virtual DbSet<Payment> Payments { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<ProductCategory> ProductCategories { get; set; }
        public virtual DbSet<ProductOrder> ProductOrders { get; set; }
        public virtual DbSet<ProductOrderDetail> ProductOrderDetails { get; set; }
        public virtual DbSet<ProductPhoto> ProductPhotoes { get; set; }
        public virtual DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public virtual DbSet<Size> Sizes { get; set; }
        public virtual DbSet<Slide> Slides { get; set; }
        public virtual DbSet<Stock> Stocks { get; set; }
        public virtual DbSet<Supplier> Suppliers { get; set; }
        public virtual DbSet<UserAccount> UserAccounts { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Age>()
                .Property(e => e.UpdateBy)
                .IsUnicode(false);

            modelBuilder.Entity<Color>()
                .Property(e => e.ColorID)
                .IsUnicode(false);

            modelBuilder.Entity<Color>()
                .Property(e => e.UpdateBy)
                .IsUnicode(false);

            modelBuilder.Entity<Color>()
                .HasMany(e => e.Stocks)
                .WithRequired(e => e.Color)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Contact>()
                .Property(e => e.Phone)
                .IsUnicode(false);

            modelBuilder.Entity<Contact>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<Contact>()
                .Property(e => e.FacebookLink)
                .IsUnicode(false);

            modelBuilder.Entity<Contact>()
                .Property(e => e.InstagramLink)
                .IsUnicode(false);

            modelBuilder.Entity<Contact>()
                .Property(e => e.TwitterLink)
                .IsUnicode(false);

            modelBuilder.Entity<Contact>()
                .Property(e => e.UpdateBy)
                .IsUnicode(false);

            modelBuilder.Entity<Contact>()
                .Property(e => e.LocationOnGoogleMap)
                .IsUnicode(false);

            modelBuilder.Entity<EmployeeAccount>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<EmployeeAccount>()
                .Property(e => e.Password)
                .IsUnicode(false);

            modelBuilder.Entity<EmployeeAccount>()
                .Property(e => e.Phone)
                .IsUnicode(false);

            modelBuilder.Entity<Evaluation>()
                .Property(e => e.ProductID)
                .IsUnicode(false);

            modelBuilder.Entity<Favourite>()
                .Property(e => e.ProductID)
                .IsUnicode(false);

            modelBuilder.Entity<Feedback>()
                .Property(e => e.ProductID)
                .IsUnicode(false);

            modelBuilder.Entity<Invoice>()
                .Property(e => e.TotalPayment)
                .HasPrecision(18, 0);

            modelBuilder.Entity<Payment>()
                .Property(e => e.PaymentID)
                .IsUnicode(false);

            modelBuilder.Entity<Product>()
                .Property(e => e.ProductID)
                .IsUnicode(false);

            modelBuilder.Entity<Product>()
                .Property(e => e.MetaTitle)
                .IsUnicode(false);

            modelBuilder.Entity<Product>()
                .Property(e => e.SupplierID)
                .IsUnicode(false);

            modelBuilder.Entity<Product>()
                .Property(e => e.UnitImportPrice)
                .HasPrecision(18, 0);

            modelBuilder.Entity<Product>()
                .Property(e => e.UnitSellPrice)
                .HasPrecision(18, 0);

            modelBuilder.Entity<Product>()
                .Property(e => e.UpdateBy)
                .IsUnicode(false);

            modelBuilder.Entity<Product>()
                .HasMany(e => e.Evaluations)
                .WithRequired(e => e.Product)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Product>()
                .HasMany(e => e.Stocks)
                .WithRequired(e => e.Product)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Product>()
                .HasMany(e => e.UserAccounts)
                .WithMany(e => e.Products);

            modelBuilder.Entity<ProductCategory>()
                .Property(e => e.UpdateBy)
                .IsUnicode(false);

            modelBuilder.Entity<ProductOrder>()
                .Property(e => e.PaymentID)
                .IsUnicode(false);

            modelBuilder.Entity<ProductOrder>()
                .Property(e => e.PaymentOnlineID)
                .IsUnicode(false);

            modelBuilder.Entity<ProductOrder>()
                .HasMany(e => e.ProductOrderDetails)
                .WithRequired(e => e.ProductOrder)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ProductOrderDetail>()
                .Property(e => e.ProductID)
                .IsUnicode(false);

            modelBuilder.Entity<ProductOrderDetail>()
                .Property(e => e.SizeID)
                .IsUnicode(false);

            modelBuilder.Entity<ProductOrderDetail>()
                .Property(e => e.ColorID)
                .IsUnicode(false);

            modelBuilder.Entity<ProductOrderDetail>()
                .Property(e => e.Price)
                .HasPrecision(18, 0);

            modelBuilder.Entity<ProductPhoto>()
                .Property(e => e.ProductID)
                .IsUnicode(false);

            modelBuilder.Entity<ProductPhoto>()
                .Property(e => e.UpdateBy)
                .IsUnicode(false);

            modelBuilder.Entity<ShoppingCart>()
                .Property(e => e.ProductID)
                .IsUnicode(false);

            modelBuilder.Entity<ShoppingCart>()
                .Property(e => e.SizeID)
                .IsUnicode(false);

            modelBuilder.Entity<ShoppingCart>()
                .Property(e => e.ColorID)
                .IsUnicode(false);

            modelBuilder.Entity<Size>()
                .Property(e => e.SizeID)
                .IsUnicode(false);

            modelBuilder.Entity<Size>()
                .Property(e => e.UpdateBy)
                .IsUnicode(false);

            modelBuilder.Entity<Size>()
                .HasMany(e => e.Stocks)
                .WithRequired(e => e.Size)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Slide>()
                .Property(e => e.UpdateBy)
                .IsUnicode(false);

            modelBuilder.Entity<Stock>()
                .Property(e => e.ProductID)
                .IsUnicode(false);

            modelBuilder.Entity<Stock>()
                .Property(e => e.SizeID)
                .IsUnicode(false);

            modelBuilder.Entity<Stock>()
                .Property(e => e.ColorID)
                .IsUnicode(false);

            modelBuilder.Entity<Stock>()
                .Property(e => e.UpdateBy)
                .IsUnicode(false);

            modelBuilder.Entity<Stock>()
                .HasMany(e => e.ProductOrderDetails)
                .WithRequired(e => e.Stock)
                .HasForeignKey(e => new { e.ProductID, e.SizeID, e.ColorID })
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Stock>()
                .HasMany(e => e.ShoppingCarts)
                .WithRequired(e => e.Stock)
                .HasForeignKey(e => new { e.ProductID, e.SizeID, e.ColorID })
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Supplier>()
                .Property(e => e.SupplierID)
                .IsUnicode(false);

            modelBuilder.Entity<Supplier>()
                .Property(e => e.Phone)
                .IsUnicode(false);

            modelBuilder.Entity<Supplier>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<Supplier>()
                .Property(e => e.UpdateBy)
                .IsUnicode(false);

            modelBuilder.Entity<UserAccount>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<UserAccount>()
                .Property(e => e.EmailConfirmCode)
                .IsUnicode(false);

            modelBuilder.Entity<UserAccount>()
                .Property(e => e.Password)
                .IsUnicode(false);

            modelBuilder.Entity<UserAccount>()
                .Property(e => e.Phone)
                .IsUnicode(false);

            modelBuilder.Entity<UserAccount>()
                .HasMany(e => e.Evaluations)
                .WithRequired(e => e.UserAccount)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<UserAccount>()
                .HasMany(e => e.ShoppingCarts)
                .WithRequired(e => e.UserAccount)
                .WillCascadeOnDelete(false);
        }
    }
}
