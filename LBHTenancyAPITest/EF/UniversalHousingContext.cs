using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LBHTenancyAPITest.EF.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace LBHTenancyAPITest.EF
{
    public class UniversalHousingContext:DbContext
    {
        public UniversalHousingContext(DbContextOptions options):base(options)
        {

        }

        public DbSet<TenancyAgreement> tenagree { get; set; }
        public DbSet<Member> member { get; set; }
    }
}
