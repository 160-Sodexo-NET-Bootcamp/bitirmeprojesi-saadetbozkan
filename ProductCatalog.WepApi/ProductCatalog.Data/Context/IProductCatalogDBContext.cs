using Microsoft.EntityFrameworkCore;
using ProductCatalog.Data.DataModels.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductCatalog.Data.Context
{
    public interface IProductCatalogDBContext
    {
        DbSet<Category> Categories { get; set; }
        DbSet<Color> Colors { get; set; }
        DbSet<Offer> Offers { get; set; }
        DbSet<Product> Products { get; set; }
        DbSet<Trademark> Trademarks { get; set; }
        DbSet<UseCase> UseCases { get; set; }
        DbSet<User> Users { get; set; }
        int SaveChanges();
        void Dispose();
    }
}
