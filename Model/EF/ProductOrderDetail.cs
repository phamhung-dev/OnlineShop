namespace Model.EF
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("ProductOrderDetail")]
    public partial class ProductOrderDetail
    {
        [Key]
        [Column(Order = 0)]
        public Guid ProductOrderID { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(20)]
        public string ProductID { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(7)]
        public string SizeID { get; set; }

        [Key]
        [Column(Order = 3)]
        [StringLength(7)]
        public string ColorID { get; set; }

        public decimal Price { get; set; }

        public int Quantity { get; set; }

        public virtual ProductOrder ProductOrder { get; set; }

        public virtual Stock Stock { get; set; }
    }
}
