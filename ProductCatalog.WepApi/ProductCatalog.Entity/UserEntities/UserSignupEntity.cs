using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ProductCatalog.Entity.UserEntities
{
    public class UserSignupEntity
    {
        [Required]
        [StringLength(30, ErrorMessage = "{0} field max {1}, at least {2} chars", MinimumLength = 3)]
        public string Name { get; set; }

        [Required]
        [StringLength(30, ErrorMessage = "{0} field max {1}, at least {2} chars", MinimumLength = 2)]
        public string Surname { get; set; }

        [Required]
        [StringLength(30, ErrorMessage = "{0} field at least {1}, max {2} chars", MinimumLength = 8)]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(20, ErrorMessage = "{0} field max {1}, at least {2} chars", MinimumLength = 8)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password is not same!")]
        public string ConfirmPassword { get; set; }       
    }
}
