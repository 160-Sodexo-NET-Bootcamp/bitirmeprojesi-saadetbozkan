using ProductCatalog.Data.DataModels.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductCatalog.Data.DataModels.Concrete
{
    public class User : BaseProperty
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int FaultyInputCount { get; set; }
        //Product ile One to many işlişki
        public IEnumerable<Product> Products { get; set; }
    }
}
