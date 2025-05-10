
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace MvcWebApplication.Models
{
	public class MenuListing
	{
		public int Id { get; set; }
		public int ItemId { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public decimal Price { get; set; }
		public decimal Cost { get; set; }
		public string Category { get; set; }

		public override string ToString()
		{
			return JsonSerializer.Serialize(this);
		}
	}
}
