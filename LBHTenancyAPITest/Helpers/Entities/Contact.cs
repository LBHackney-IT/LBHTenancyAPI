using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LBHTenancyAPITest.Helpers.Entities
{
    [Table("UHContacts")]
    public class Contact
    {

        [Column("con_key")]
        public int con_key { get; set; }

        [Key]
        [Column("con_ref")]
        [MaxLength(12)]
        public string con_ref { get; set; }

        /// <summary>
        /// TenancyAgreementReference
        /// </summary>
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
        /// Contact Name
        /// </summary>
        [Column("con_name")]
        [MaxLength(73)]
        public string con_name { get; set; }

    }
}
