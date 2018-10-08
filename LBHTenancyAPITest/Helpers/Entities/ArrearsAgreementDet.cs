using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LBHTenancyAPITest.Helpers.Entities
{
    public class ArrearsAgreementDet
    {

        [Column("tag_ref")]
        [MaxLength(11)]
        public string tag_ref { get; set; }
        [Column("aragdet_amount")]
        public decimal amount { get; set; }

        [Column("arag_sid")]
        public int arag_sid { get; set; }

        [Column("aragdet_frequency")]
        [MaxLength(11)]
        public string aragdet_frequency { get; set; }

    }
}