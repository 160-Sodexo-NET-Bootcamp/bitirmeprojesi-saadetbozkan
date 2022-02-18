using System.ComponentModel.DataAnnotations;

namespace ProductCatalog.Entity.OfferEntities
{
    public class OfferGetEntity
    {      
        public long OfferAmount { get; set; }
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public bool? IsApproved { get; set; }
    }
}
