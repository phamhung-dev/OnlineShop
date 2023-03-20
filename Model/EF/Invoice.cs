namespace Model.EF
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Invoice")]
    public partial class Invoice
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid InvoiceID { get; set; }

        public DateTime ExportDate { get; set; }

        public decimal TotalPayment { get; set; }

        public bool CustomerConfirm { get; set; }

        public Guid? ProductOrderID { get; set; }

        public Guid? EmployeeID { get; set; }

        public bool Status { get; set; }

        public virtual EmployeeAccount EmployeeAccount { get; set; }

        public virtual ProductOrder ProductOrder { get; set; }
    }
}
