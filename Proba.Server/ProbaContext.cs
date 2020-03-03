using Greet;
using Microsoft.EntityFrameworkCore;
using System;

namespace Proba.Server
{
    public class ProbaContext : DbContext
    {
        public DbSet<ProbaMessage> Probas { get; set; }

        public ProbaContext(DbContextOptions options): base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProbaMessage>().HasKey("Greeting");
            modelBuilder.Entity<ProbaMessage>().Ignore("Date");
        }
    }
}
