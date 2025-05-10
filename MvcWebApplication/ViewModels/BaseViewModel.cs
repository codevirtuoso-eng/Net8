using System.Text.Json;

namespace MvcWebApplication.ViewModels
{
	public class BaseViewModel
	{
		public BaseViewModel()
		{
			Message = string.Empty;
		}

		public string Message { get; set; }

		public override string ToString()
		{
			return JsonSerializer.Serialize(this);
		}
	}
}
