namespace Model.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("ProductOrder")]
    public partial class ProductOrder
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ProductOrder()
        {
            Invoices = new HashSet<Invoice>();
            ProductOrderDetails = new HashSet<ProductOrderDetail>();
        }

        public Guid ProductOrderID { get; set; }

        public DateTime OrderDate { get; set; }

        public DateTime? ShipDate { get; set; }

        public Guid? UserID { get; set; }

        public bool Status { get; set; }

        [StringLength(6)]
        public string PaymentID { get; set; }

        [StringLength(30)]
        public string PaymentOnlineID { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Invoice> Invoices { get; set; }

        public virtual Payment Payment { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProductOrderDetail> ProductOrderDetails { get; set; }

        public virtual UserAccount UserAccount { get; set; }
    }
}
