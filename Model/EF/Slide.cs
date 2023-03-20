namespace Model.EF
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Slide")]
    public partial class Slide
    {
        public int SlideID { get; set; }

        [Column(TypeName = "image")]
        [Required]
        public byte[] Photo { get; set; }

        public DateTime CreateAt { get; set; }

        public DateTime UpdateAt { get; set; }

        [Required]
        [StringLength(254)]
        public string UpdateBy { get; set; }

        public bool Status { get; set; }

        public int PositionAppear { get; set; }
    }
}
