using Microsoft.EntityFrameworkCore;
using ProductCatalog.Data.DataModels.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductCatalog.Data.Context
{
    public class ProductCatalogDBContext : DbContext, IProductCatalogDBContext
    {
        public ProductCatalogDBContext(DbContextOptions<ProductCatalogDBContext> options) : base(options)
        {
        }

        //Data modellerinin db de tabloları oluşturuldu.
        public DbSet<Category> Categories { get; set; }
        public DbSet<Color> Colors { get; set; }
        public DbSet<Offer> Offers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Trademark> Trademarks { get; set; }
        public DbSet<UseCase> UseCases { get; set; }
        public DbSet<User> Users { get; set; }

        public override int SaveChanges()
        {
            return base.SaveChanges();
        }
        public override void Dispose()
        {
             base.Dispose();
        }
    }
}
