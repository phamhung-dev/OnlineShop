namespace Model.EF
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Evaluation")]
    public partial class Evaluation
    {
        [Key]
        [Column(Order = 0)]
        public Guid UserID { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(20)]
        public string ProductID { get; set; }

        public byte ScoreRating { get; set; }

        public virtual Product Product { get; set; }

        public virtual UserAccount UserAccount { get; set; }
    }
}
