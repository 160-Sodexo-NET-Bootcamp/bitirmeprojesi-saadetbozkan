using ProductCatalog.Data.DataModels.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductCatalog.Data.DataModels.Concrete
{
    public class Product : BaseProperty
    {
        public string Name { get; set; }
        public string Detail { get; set; }
        public bool IsOfferable { get; set; }
        public bool IsSold { get; set; }
        public string Image { get; set; }
        public long Price { get; set; }

        //Category ile one to many  ilişkisi bulunur.
        [ForeignKey("Category")]
        public int CategoryId { get; set; }
        public Category Category { get; set; }

        //Color ile one to many  ilişkisi bulunur.
        [ForeignKey("Color")]
        public int ColorId { get; set; }
        public Color Color { get; set; }

        //Category ile one to many  ilişkisi bulunur.
        [ForeignKey("UseCase")]
        public int UseCaseId { get; set; }
        public UseCase UseCase { get; set; }

        //Category ile one to many  ilişkisi bulunur.
        [ForeignKey("Trademark")]
        public int TrademarkId { get; set; }
        public Trademark Trademark { get; set; }

        //User One to many işlişki
        [ForeignKey("OwnerId")]
        public int OwnerId { get; set; }
        public User Owner { get; set; }

        //Offer ile One to many işlişki
        public IEnumerable<Offer> Offers { get; set; }

    }
}
