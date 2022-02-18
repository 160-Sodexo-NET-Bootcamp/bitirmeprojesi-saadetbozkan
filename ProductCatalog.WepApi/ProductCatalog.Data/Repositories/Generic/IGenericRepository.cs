using ProductCatalog.Data.DataModels.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductCatalog.Data.Repositories.Generic
{
    public interface IGenericRepository<T> where T : class 
    { 
        IEnumerable<T> GetAll();
        T GetById(int id);
        bool Add(T model);
        bool Update(T model);
        bool Delete(int id);
        IList<T> Where(System.Linq.Expressions.Expression<Func<T, bool>> where);

    }
}
