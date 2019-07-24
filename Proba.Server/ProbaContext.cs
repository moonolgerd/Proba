using Greet;
using Microsoft.EntityFrameworkCore;
using System;

namespace Proba.Server
{
    public class ProbaContext : DbContext
    {
        public DbSet<ProbaMessage> Probas { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=proba.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProbaMessage>().HasKey("Greeting");
            modelBuilder.Entity<ProbaMessage>().Ignore("Date");
        }
    }
}
