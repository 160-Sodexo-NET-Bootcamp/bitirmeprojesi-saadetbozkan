using System.ComponentModel.DataAnnotations;

namespace ProductCatalog.Entity.OfferEntities
{
    public class AnswerForOfferEntity
    {
        [RegularExpression(@"Onayla|Reddet", ErrorMessage = "Please enter a valid value)")]
        public string IsApproved { get; set; }
    }
}
