using ProductCatalog.Data.DataModels.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductCatalog.Data.DataModels.Abstract
{
    //Product propertisi için üretilen classlar base sınıfı. 
    public class BaseProductProperty : BaseProperty
    {
        public string Name { get; set; }
        public IEnumerable<Product> Products { get; set; }
    }
}
