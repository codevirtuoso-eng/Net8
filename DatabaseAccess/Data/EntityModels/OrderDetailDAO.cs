using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace DatabaseAccess.Data.EntityModels
{

	[Table("OrderDetails")]
	public class OrderDetailDAO
	{
		[Column("OrderId")]
		public string OrderId { get; set; }

		[Column("OrderDetailId")]
		public string OrderDetailId { get; set; }

		[Column("ItemId")]
		public int ItemId { get; set; }

		[Column("Category")]
		public string Category { get; set; }

		[Column("Name")]
		public string Name { get; set; }

		[Column("Cost")]
		public decimal Cost { get; set; }

		public override string ToString()
		{
			return JsonSerializer.Serialize(this);
		}
	}
}
