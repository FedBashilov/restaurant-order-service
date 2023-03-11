// Copyright (c) Fedor Bashilov. All rights reserved.

namespace Infrastructure.Database
{
    using Infrastructure.Core.Models;
    using Microsoft.EntityFrameworkCore;

    public class OrderDatabaseContext : DbContext
    {
        public OrderDatabaseContext(DbContextOptions<OrderDatabaseContext> options)
            : base(options) => this.Database.EnsureCreated();

        public DbSet<Order> Orders => this.Set<Order>();

        public DbSet<OrderMenuItem> OrderMenuItems => this.Set<OrderMenuItem>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OrderMenuItem>()
                .HasKey(omi => new { omi.OrderId, omi.MenuItemId });
        }
    }
}