
using System.ComponentModel.DataAnnotations;

namespace MvcWebApplication.Models
{
    public class MenuListing
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Price")]
        public decimal Price { get; set; }

        [Required]
        [Display(Name = "Category")]
        public string Category { get; set; }
    }
}
