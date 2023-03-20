namespace Model.EF
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Feedback")]
    public partial class Feedback
    {
        public int FeedbackID { get; set; }

        public Guid? UserID { get; set; }

        [StringLength(20)]
        public string ProductID { get; set; }

        public DateTime FeedbackDate { get; set; }

        public string Content { get; set; }

        public virtual Product Product { get; set; }

        public virtual UserAccount UserAccount { get; set; }
    }
}
