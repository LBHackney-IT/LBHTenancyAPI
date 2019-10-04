using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LBHTenancyAPITest.Helpers.Entities
{
    [Table("tenagree")]
    public class TenancyAgreement
    {
        public DateTime? start_date;

        /// <summary>
        /// NumberOfBedrooms
        /// </summary>
        [Column("num_bedrooms")]
        [MaxLength(255)]
        public Nullable<int> num_bedrooms { get; set; }
        /// <summary>
        /// TenancyPaymentReference
        /// </summary>
        [Column("u_saff_rentacc")]
        [MaxLength(20)]
        public string payment_ref { get; set; }

        /// <summary>
        /// TenancyAgreementReference
        /// </summary>
        [Key]
        [Column("tag_ref")]
        [MaxLength(11)]
        [Required]
        public string tag_ref { get; set; }

        /// <summary>
        /// Property Reference
        /// </summary>
        [Column("prop_ref")]
        [MaxLength(12)]
        public string prop_ref { get; set; }

        /// <summary>
        /// House Reference
        /// </summary>
        [Column("house_ref")]
        [MaxLength(10)]
        public string house_ref { get; set; }

        /// <summary>
        /// Current Balance
        /// </summary>
        [Column("cur_bal")]
        public decimal cur_bal { get; set; }

        /// <summary>
        /// Tenure
        /// </summary>
        [Column("tenure")]
        [MaxLength(3)]
        public string tenure { get; set; }

        /// <summary>
        /// Rent Charges
        /// </summary>
        [Column("rent")]
        public decimal rent { get; set; }

        /// <summary>
        /// Service Charges
        /// </summary>
        [Column("service")]
        public decimal service { get; set; }

        /// <summary>
        /// Other Charges
        /// </summary>
        [Column("other_charge")]
        public decimal other_charge { get; set; }
    }
}
