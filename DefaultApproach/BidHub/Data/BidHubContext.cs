using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DotNetEnv;

namespace BidHub.Models
{
    public class BidHubContext : DbContext
    {

        public BidHubContext(DbContextOptions<BidHubContext> options) : base(options)
        {
            Env.Load();
        }

        public DbSet<Auction> Auctions { get; set; }
        public DbSet<Item> Items { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseNpgsql(Environment.GetEnvironmentVariable("CONNECTION_STRING"));
    }
}