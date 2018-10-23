using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LBHTenancyAPITest.Helpers.Entities
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

        
        [Column("age")]
        public int age { get; set; }

        [Column("responsible")]
        public bool responsible { get; set; }

    }

    public static class MemberExtensions
    {
        public static string GetFullName(this Member member)
        {
            return member == null ? null : $"{member.forename} {member.surname}";
        }
    }
}
