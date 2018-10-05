using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Dapper;

namespace LBHTenancyAPITest.EF.Entities
{
    [Dapper.Table("arag")]
    public class ArrearsAgreement
    {
        [Dapper.Key]
        [Dapper.Column("arag_ref")]
        [MaxLength(15)]
        [Dapper.Required]
        public string arag_ref { get; set; }

        [Dapper.Column("tag_ref")]
        [MaxLength(11)]
        public string tag_ref { get; set; }

        [Dapper.Column("arag_status")]
        [MaxLength(11)]
        public string arag_status { get; set; }

        [Dapper.Column("arag_startdate")]
        public DateTime arag_startdate { get; set; }
    }
}
