using Lab4.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lab4.Data
{
    public class MarketDbContext : DbContext
    {
        public MarketDbContext(DbContextOptions<MarketDbContext> options) : base(options)
        {

        }

        public DbSet<Client> Clients { get; set; }
        public DbSet<Brokerage> Brokerages { get; set; }
        public DbSet<Advertisement> Advertisements { get; set; }

        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<AdvertisementBrokerage> AdvertisementBrokerages { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Client>().ToTable("Client");
            modelBuilder.Entity<Brokerage>().ToTable("Brokerage");
            modelBuilder.Entity<Subscription>().ToTable("Subscriptiom");
            modelBuilder.Entity<Advertisement>().ToTable("Advertisement");
            modelBuilder.Entity<AdvertisementBrokerage>().ToTable("AdvertisementBrokerage");

            modelBuilder.Entity<Subscription>()
                .HasKey(c => new { c.ClientId, c.BrokerageId });

        }
    }
}