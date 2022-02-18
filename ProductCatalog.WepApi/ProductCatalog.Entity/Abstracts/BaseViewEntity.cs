using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductCatalog.Entity.Abstracts
{
    public class BaseViewEntity :IViewEntity
    {
        [Required]
        public string Name { get; set; }
    }
}
