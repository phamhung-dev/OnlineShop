namespace Model.EF
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("ProductPhoto")]
    public partial class ProductPhoto
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid ProductPhotoID { get; set; }

        [StringLength(20)]
        public string ProductID { get; set; }

        [Column(TypeName = "image")]
        public byte[] Photo { get; set; }

        public DateTime CreateAt { get; set; }

        public DateTime UpdateAt { get; set; }

        [Required]
        [StringLength(254)]
        public string UpdateBy { get; set; }

        public bool Status { get; set; }

        public virtual Product Product { get; set; }
    }
}
