using System.Text.Json;

namespace SharedLibrary.Common.Models
{
    public partial class AspNetRole
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
