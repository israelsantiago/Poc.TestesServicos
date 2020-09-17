using System;
using Microsoft.EntityFrameworkCore;
using PoC.TestesServicos.Data.Configurations;
using PoC.TestesServicos.Data.Models;

namespace PoC.TestesServicos.Data
{
    public class UsersDataContext : DbContext
    {
        private readonly IContextConfiguration _config;

        public UsersDataContext(IContextConfiguration config)
        {
            _config = config;
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Team> Teams { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_config.ConnectionString, sqlServerOptionsAction: sqlOptions =>
            {
                sqlOptions.EnableRetryOnFailure(maxRetryCount: 20, maxRetryDelay: TimeSpan.FromSeconds(10), errorNumbersToAdd: null);
            });            
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new TeamConfiguration());
            modelBuilder.ApplyConfiguration(new UserTeamConfiguration());
            base.OnModelCreating(modelBuilder);
        }
    }
}