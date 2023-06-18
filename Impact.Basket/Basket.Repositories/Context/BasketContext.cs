using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basket.Repositories.Context
{
    public class BasketContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase(databaseName: "BasketDataBase");
        }

        public DbSet<Models.Basket> Baskets { get; set; }
        public DbSet<Models.BasketItem> BasketItems { get; set; }
    }
}
