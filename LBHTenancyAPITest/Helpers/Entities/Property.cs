﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LBHTenancyAPITest.Helpers.Entities
{
    [Table("property")]
    public class Property
    {
        /// <summary>
        /// TenancyAgreementReference
        /// </summary>
        [Key]
        [Column("prop_ref")]
        [MaxLength(12)]
        [Required]
        public string prop_ref { get; set; }

        [Column("short_address")]
        [MaxLength(200)]
        [Required]
        public string short_address { get; set; }

        [Column("post_code")]
        [MaxLength(10)]
        [Required]
        public string post_code { get; set; }

        [Column("address1")]
        [MaxLength(255)]
        [Required]
        public string address1 { get; set; }

        [Column("num_bedrooms")]
        [MaxLength(255)]
        public int? num_bedrooms { get; set; }
    }
}
