using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LBHTenancyAPITest.EF.Entities
{
    [Table("member")]
    public class Member
    {
        [Key]
        [Column("house_ref")]
        [MaxLength(10)]
        [Required]
        public string house_ref { get; set; }
        [Required]
        [Column("person_no")]
        public int person_no { get; set; }
        [Required]
        [Column("oap")]
        public bool oap { get; set; }
        [Required]
        [Column("at_risk")]
        public bool at_risk{ get; set; }
        [Required]
        [Column("full_ed")]
        public bool full_ed { get; set; }
        [Required]
        [MaxLength(3)]
        [Column("bank_acc_type")]
        public string bank_acc_type { get; set; }

        
        [MaxLength(10)]
        [Column("title")]
        public string title { get; set; }

        [MaxLength(24)]
        [Column("forename")]
        public string forename { get; set; }

        [MaxLength(20)]
        [Column("surname")]
        public string surname { get; set; }

        //[ethnic_origin] [char](3) NULL,
        //[gender] [char](1) NULL,
        //[title] [char](10) NULL,
        //[initials] [char](3) NULL,
        //[forename] [char](24) NULL,
        //[surname] [char](20) NULL,
        //[age] [numeric] (3, 0) NULL,

        //[relationship] [char](1) NULL,
        //[econ_status] [char](1) NULL,
        //[responsible]
        //[bit]
        //NOT NULL,

        //[wheelch_user] [char](3) NULL,
        //[disabled] [char](3) NULL,
        //[cl_group_a] [char](3) NULL,
        //[cl_group_b] [char](3) NULL,
        //[ethnic_colour] [char](3) NULL,
        //[at_risk]
        //[bit]
        //NOT NULL,

        //[ni_no] [char](12) NULL,
        //[full_ed]
        //[bit]
        //NOT NULL,

        //[member_sid] [int] NOT NULL,

        //[contacts_sid] [int] NULL,
        //[tstamp] [timestamp] NULL,
        //[comp_avail] [char](200) NULL,
        //[comp_display] [char](200) NULL,
        //[occupation] [char](3) NULL,
        //[asboissued] [bit] NULL,
        //[liablemember] [bit] NULL,
        //[dob] [dbo].[uhdate]
        //NOT NULL,

        //[nationality] [char](3) NULL,
        //[ci_surname] [varchar] (255) NULL,
        //[u_pin_number] [char](20) NULL,
        //[ci_title] AS([title] collate Latin1_General_CI_AI),
        //[ci_forename] AS([forename] collate Latin1_General_CI_AI),
        //[u_ethnic_other] [char](20) NULL,
        //[tenportactcode] [uniqueidentifier] NULL,
        //[transgender] [varchar] (3) NULL,
        //[sex_orient] [varchar] (3) NULL,
        //[religion_belief] [varchar] (3) NULL,
        //[marriage_civil] [varchar] (3) NULL,
        //[first_lang] [varchar] (3) NULL,
        //[soc_ec_stat] [varchar] (3) NULL,
        //[soc_class] [varchar] (3) NULL,
        //[appearance] [varchar] (3) NULL,
        //[vulnerable] [varchar] (3) NULL,
        //[hiv_positive] [varchar] (3) NULL,
        //[crim_rec] [varchar] (3) NULL,
        //[contact_type] [varchar] (3) NULL,
        //[corr_type] [varchar] (3) NULL,
        //[resp_dep] [varchar] (3) NULL,
        //[pregnant] [bit] NULL,
        //[bank_acc_type] [char](3) NOT NULL,

        //[homeless] [varchar] (3) NULL

    }
}
