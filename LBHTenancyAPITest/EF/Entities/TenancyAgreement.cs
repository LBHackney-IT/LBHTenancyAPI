using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LBHTenancyAPITest.EF.Entities
{
    [Table("tenagree")]
    public class TenancyAgreement
    {
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
        /// Current Balance
        /// </summary>
        [Column("cur_bal")]
        [MaxLength(12)]
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
        [MaxLength(3)]
        public decimal service { get; set; }

        /// <summary>
        /// Other Charges
        /// </summary>
        [Column("other_charge")]
        [MaxLength(3)]
        public decimal other_charge { get; set; }
    }
}
