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
    }
}