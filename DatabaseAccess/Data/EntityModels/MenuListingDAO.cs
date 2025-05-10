using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DatabaseAccess.Data.EntityModels
{
	[Table("MenuListings")]
	public class MenuListingDAO
	{
		[Key]
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
