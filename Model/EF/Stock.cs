namespace Model.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Stock")]
    public partial class Stock
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Stock()
        {
            ProductOrderDetails = new HashSet<ProductOrderDetail>();
            ShoppingCarts = new HashSet<ShoppingCart>();
        }

        [Key]
        [Column(Order = 0)]
        [StringLength(20)]
        public string ProductID { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(7)]
        public string SizeID { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(7)]
        public string ColorID { get; set; }

        public int Quantity { get; set; }

        public DateTime CreateAt { get; set; }

        public DateTime UpdateAt { get; set; }

        [Required]
        [StringLength(254)]
        public string UpdateBy { get; set; }

        public bool Status { get; set; }

        public virtual Color Color { get; set; }

        public virtual Product Product { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProductOrderDetail> ProductOrderDetails { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ShoppingCart> ShoppingCarts { get; set; }

        public virtual Size Size { get; set; }
    }
}
