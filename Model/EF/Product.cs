namespace Model.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Product")]
    public partial class Product
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Product()
        {
            Evaluations = new HashSet<Evaluation>();
            Favourites = new HashSet<Favourite>();
            Feedbacks = new HashSet<Feedback>();
            ProductPhotoes = new HashSet<ProductPhoto>();
            Stocks = new HashSet<Stock>();
            UserAccounts = new HashSet<UserAccount>();
        }

        [StringLength(20)]
        public string ProductID { get; set; }

        [Required]
        [StringLength(300)]
        public string ProductName { get; set; }

        [Required]
        [StringLength(350)]
        public string MetaTitle { get; set; }

        [StringLength(7)]
        public string SupplierID { get; set; }

        public int? ProductCategoryID { get; set; }

        public int? AgeID { get; set; }

        public bool Gender { get; set; }

        [Column(TypeName = "ntext")]
        public string Description { get; set; }

        public int Warranty { get; set; }

        public double ScoreRating { get; set; }

        public decimal UnitImportPrice { get; set; }

        public decimal UnitSellPrice { get; set; }

        public double DiscountPercent { get; set; }

        public bool ShowOnHome { get; set; }

        public DateTime CreateAt { get; set; }

        public DateTime UpdateAt { get; set; }

        [Required]
        [StringLength(254)]
        public string UpdateBy { get; set; }

        public bool Status { get; set; }

        public virtual Age Age { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Evaluation> Evaluations { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Favourite> Favourites { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Feedback> Feedbacks { get; set; }

        public virtual ProductCategory ProductCategory { get; set; }

        public virtual Supplier Supplier { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProductPhoto> ProductPhotoes { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Stock> Stocks { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserAccount> UserAccounts { get; set; }
    }
}
