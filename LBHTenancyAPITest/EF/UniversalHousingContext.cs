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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TenancyAgreement>();
            modelBuilder.Entity<Member>();
            modelBuilder.Entity<Property>();
            modelBuilder.Entity<ArrearsAgreement>();
            modelBuilder.Entity<Contact>();
        }

        public DbSet<TenancyAgreement> tenagree { get; set; }
        public DbSet<Member> member { get; set; }
        public DbSet<Property> property { get; set; }
        public DbSet<ArrearsAgreement> arag { get; set; }
        public DbSet<Contact> contacts { get; set; }
    }
}
