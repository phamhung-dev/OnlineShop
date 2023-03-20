namespace Model.EF
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Contact")]
    public partial class Contact
    {
        public int ContactID { get; set; }

        [StringLength(300)]
        public string Address { get; set; }

        [StringLength(10)]
        public string Phone { get; set; }

        [StringLength(254)]
        public string Email { get; set; }

        [StringLength(500)]
        public string FacebookLink { get; set; }

        [StringLength(500)]
        public string InstagramLink { get; set; }

        [StringLength(500)]
        public string TwitterLink { get; set; }

        public DateTime CreateAt { get; set; }

        public DateTime UpdateAt { get; set; }

        [Required]
        [StringLength(254)]
        public string UpdateBy { get; set; }

        public bool Status { get; set; }

        [StringLength(1000)]
        public string LocationOnGoogleMap { get; set; }
    }
}
