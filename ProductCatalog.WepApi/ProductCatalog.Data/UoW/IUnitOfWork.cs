using ProductCatalog.Data.DataModels.Concrete;
using ProductCatalog.Data.Repositories.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductCatalog.Data.UoW
{
    public interface IUnitOfWork
    {
        IGenericRepository<Category> CategoryRepository { get; }
        IGenericRepository<Color> ColorRepository { get; }
        IGenericRepository<Offer> OfferRepository { get; }
        IGenericRepository<Product> ProductRepository { get; }
        IGenericRepository<Trademark> TrademarkRepository { get; }
        IGenericRepository<UseCase> UseCaseRepository { get; }
        IGenericRepository<User> UserRepository { get; }

        int Complate();

    }
}
