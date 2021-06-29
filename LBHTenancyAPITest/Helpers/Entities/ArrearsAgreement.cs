using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LBHTenancyAPITest.Helpers.Entities
{
    [Table("UHArag")]
    public class ArrearsAgreement
    {
        [Key]
        [Column("arag_ref")]
        [MaxLength(15)]
        [Required]
        public string arag_ref { get; set; }

        [Column("tag_ref")]
        [MaxLength(11)]
        public string tag_ref { get; set; }

        [Column("arag_status")]
        [MaxLength(11)]
        public string arag_status { get; set; }

        [Column("arag_startdate")]
        public DateTime arag_startdate { get; set; }

        [Column("arag_sid")]
        public int arag_sid { get; set; }

    }
}
