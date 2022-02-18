using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ProductCatalog.Data.Context;
using ProductCatalog.Data.DataModels.Concrete;
using ProductCatalog.Data.Repositories.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductCatalog.Data.UoW
{
    
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ILogger logger;
        private readonly IConfiguration configuration;
        private readonly IProductCatalogDBContext context;

        public IGenericRepository<Category> CategoryRepository { get; }
        public IGenericRepository<Color> ColorRepository { get; }
        public IGenericRepository<Offer> OfferRepository { get; }
        public IGenericRepository<Product> ProductRepository { get; }
        public IGenericRepository<Trademark> TrademarkRepository { get; }
        public IGenericRepository<UseCase> UseCaseRepository { get; }
        public IGenericRepository<User> UserRepository { get; }

       

        public UnitOfWork(ProductCatalogDBContext context, ILoggerFactory logger, IConfiguration configuration)
        {
            this.context = context;
            this.logger = logger.CreateLogger("UnitOfWork");
            this.configuration = configuration;

            CategoryRepository = new GenericRepository<Category>(context, this.logger);
            ColorRepository = new GenericRepository<Color>(context, this.logger);
            OfferRepository = new GenericRepository<Offer>(context, this.logger);
            ProductRepository = new GenericRepository<Product>(context, this.logger);
            TrademarkRepository = new GenericRepository<Trademark>(context, this.logger);
            UseCaseRepository = new GenericRepository<UseCase>(context, this.logger);
            UserRepository = new GenericRepository<User>(context, this.logger);

        }

        public int Complate()
        {
            return context.SaveChanges();
        }

        public void Dispose()
        {
            context.Dispose();
        }
    }
}
