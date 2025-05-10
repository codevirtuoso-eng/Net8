using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace MvcWebApplication.Models
{
	public class MenuListing
	{
		[Required]
		[Display(Name = "Item Id")]
		public int ItemId { get; set; }

		[Required]
		[Display(Name = "Category")]
		public string Category { get; set; }

		[Required]
		[MinLength(1)]
		[MaxLength(25)]
		[Display(Name = "Name")]
		public string Name { get; set; }

		[Required]
		[Range(0, 9999999999999999.99)]
		[Display(Name = "Cost")]
		public decimal Cost { get; set; }
	}
}
