// Copyright (c) Fedor Bashilov. All rights reserved.

namespace Web.Facade.Data
{
    using Microsoft.EntityFrameworkCore;
    using Web.Facade.Models;

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