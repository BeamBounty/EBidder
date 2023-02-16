using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

using EBidderWeb.Server.Models.EBidder;

namespace EBidderWeb.Server.Data
{
    public partial class EBidderContext : DbContext
    {
        public EBidderContext()
        {
        }

        public EBidderContext(DbContextOptions<EBidderContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<EBidderWeb.Server.Models.EBidder.Buyer>().HasKey(table => new {
                table.SSN, table.BuyerID
            });

            builder.Entity<EBidderWeb.Server.Models.EBidder.Seller>().HasKey(table => new {
                table.SSN, table.SellerID
            });
        }

        public DbSet<EBidderWeb.Server.Models.EBidder.Admin> Admins { get; set; }

        public DbSet<EBidderWeb.Server.Models.EBidder.Artwork> Artworks { get; set; }

        public DbSet<EBidderWeb.Server.Models.EBidder.Buyer> Buyers { get; set; }

        public DbSet<EBidderWeb.Server.Models.EBidder.GameItem> GameItems { get; set; }

        public DbSet<EBidderWeb.Server.Models.EBidder.Nft> Nfts { get; set; }

        public DbSet<EBidderWeb.Server.Models.EBidder.Seller> Sellers { get; set; }
    }
}