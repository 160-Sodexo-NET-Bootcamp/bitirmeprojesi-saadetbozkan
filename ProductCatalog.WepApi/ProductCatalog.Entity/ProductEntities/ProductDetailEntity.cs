using System.ComponentModel.DataAnnotations;

namespace ProductCatalog.Entity.ProductEntities
{
    public class ProductDetailEntity {

        [Required]
        [StringLength(100, ErrorMessage = "{0} field at least {1}, max {2} chars", MinimumLength = 3)]
        public string Name { get; set; }

        [Required]
        [StringLength(500, ErrorMessage = "{0} field at least {1}, max {2} chars", MinimumLength = 3)]
        public string Detail { get; set; }

        [Required]
        [Range(1, long.MaxValue, ErrorMessage = "Please enter valid integer Number")]
        public long Price { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Please enter valid integer Number")]
        public int CategoryId { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Please enter valid integer Number")]
        public int UseCaseId { get; set; }
        public bool IsOfferable { get; set; } = false;
        public bool IsSold { get; set; } = false;
        public string Image { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Please enter valid integer Number")]
        public int? TrademarkId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Please enter valid integer Number")]
        public int? ColorId { get; set; }

    }
}
