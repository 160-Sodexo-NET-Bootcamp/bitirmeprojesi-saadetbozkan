using ProductCatalog.Data.DataModels.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductCatalog.Data.DataModels.Concrete
{
    public class Offer : BaseProperty
    {      
        public long OfferAmount { get; set; }
        public bool? IsApproved { get; set; } // Red ve onay için.
        public int? UserId { get; set; }
        public int ProductId { get; set; }

        //User ile One to many işlişki
        [ForeignKey("UserId")]
        public User User { get; set; }

        //Product ile One to many işlişki
        [ForeignKey("ProductId")]
        public Product Product { get; set; }

    }
}
