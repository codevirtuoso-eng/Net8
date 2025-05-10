using System;
using System.Text.Json;

namespace SharedLibrary.Common.Models
{
	public class OrderSearch
	{
		public string UserId { get; set; }

		public DateTime? BeginOrderDate { get; set; }

		public DateTime? EndOrderDate { get; set; }

		public override string ToString()
		{
			return JsonSerializer.Serialize(this);
		}
	}
}
