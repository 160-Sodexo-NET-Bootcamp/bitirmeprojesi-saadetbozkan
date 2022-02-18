using System.ComponentModel.DataAnnotations;

namespace ProductCatalog.Entity.OfferEntities
{
    public class OfferPostEntity
    {      
        [Required]
        public long OfferAmount { get; set; }
    }
}
