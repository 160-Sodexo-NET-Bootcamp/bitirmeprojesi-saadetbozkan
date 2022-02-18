using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ProductCatalog.Data.Context;
using ProductCatalog.Data.DataModels.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductCatalog.Data.Repositories.Generic
{
    // Crud işlemleri için yapılan genrek repository.
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly ILogger logger;
        protected IProductCatalogDBContext context;
        internal DbSet<T> dbSet;
        public GenericRepository(ProductCatalogDBContext context, ILogger logger)
        {
            this.context = context;
            this.logger = logger;
            this.dbSet = context.Set<T>();
        }

        //Tabloya veri ekleme işlemi için : 
        public bool Add(T model)
        {
            dbSet.Add(model);
            return true;
        }

        //Tablodaki id'si verilen veriyi silmek için : 
        public bool Delete(int id)
        {
            var model = dbSet.Find(id);
            dbSet.Remove(model);
            return true;
        }

        //Tablodaki tüm verileri çekmek için : 
        public IEnumerable<T> GetAll()
        {
            var models = dbSet.ToList();
            return models;
        }

        //TAblodan istenilen veriyi çekmek için : 
        public T GetById(int id)
        {
            var model = dbSet.Find(id);
            return model;
        }

        //Tablodaki veriyi güncellemek için : 
        public bool Update(T model)
        {
            dbSet.Update(model);
            return true;
        }

        //Tablodaki herhangi bir kolona göre veri getirmek için :
        public virtual IList<T> Where(System.Linq.Expressions.Expression<Func<T, bool>> where)
        {
            return dbSet.Where(where).ToList();
        }
    }
}
