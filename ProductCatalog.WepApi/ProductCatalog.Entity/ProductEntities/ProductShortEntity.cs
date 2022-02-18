using System.ComponentModel.DataAnnotations;

namespace ProductCatalog.Entity.ProductEntities
{
    public class ProductShortEntity {

        [Required]
        [StringLength(500, ErrorMessage = "{0} field at least {1}, max {2} chars", MinimumLength = 3)]
        public string Name { get; set; }
        public string Image { get; set; }
        [Required]
        public long Price { get; set; }

        public bool IsSold { get; set; } = false;

    }
}
