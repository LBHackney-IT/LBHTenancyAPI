using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace LBHTenancyAPITest.EF
{
    public class UniversalHousingContext:DbContext
    {
        public UniversalHousingContext(DbContextOptions options):base(options)
        {

        }

        public DbSet<Member> Members { get; set; }
    }
}
