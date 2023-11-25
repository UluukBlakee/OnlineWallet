using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Diagnostics.Metrics;

namespace OnlineWallet.Models
{
    public class WalletContext : IdentityDbContext<User, IdentityRole<int>, int>
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<ServiceProvider> ServiceProviders { get; set; }
        public DbSet<ServiceUser> ServiceUsers { get; set; }
        public WalletContext(DbContextOptions<WalletContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ServiceProvider>().HasData(
                new ServiceProvider()
                {
                    Id = 1,
                    Name = "Tazalyk"
                },
                new ServiceProvider()
                {
                    Id = 2,
                    Name = "Tulpar"
                },
                new ServiceProvider()
                {
                    Id = 3,
                    Name = "Teploset"
                },
                new ServiceProvider()
                {
                    Id = 4,
                    Name = "NeoTelecom"
                },
                new ServiceProvider()
                {
                    Id = 5,
                    Name = "Saima"
                },
                new ServiceProvider()
                {
                    Id = 6,
                    Name = "Megaline"
                },
                new ServiceProvider()
                {
                    Id = 7,
                    Name = "Megacom"
                },
                new ServiceProvider()
                {
                    Id = 8,
                    Name = "Beeline"
                },
                new ServiceProvider()
                {
                    Id = 9,
                    Name = "O!"
                },
                new ServiceProvider()
                {
                    Id = 10,
                    Name = "Aknet"
                }
            );
            base.OnModelCreating(modelBuilder);
        }
    }
}
