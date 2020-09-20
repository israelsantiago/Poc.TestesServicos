using System;
using Microsoft.EntityFrameworkCore;
using PoC.TestesServicos.Data.Configurations;
using PoC.TestesServicos.Data.Models;

namespace PoC.TestesServicos.Data
{
    public class UsersDataContext : DbContext
    {
        public UsersDataContext(DbContextOptions<UsersDataContext> options) 
            : base(options) {
        }        
        
        public DbSet<User> Users { get; set; }
        public DbSet<Team> Teams { get; set; }

        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new TeamConfiguration());
            modelBuilder.ApplyConfiguration(new UserTeamConfiguration());
            base.OnModelCreating(modelBuilder);
        }
    }
}