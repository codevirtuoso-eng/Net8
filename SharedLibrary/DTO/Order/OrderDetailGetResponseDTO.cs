using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;

namespace SharedLibrary.DTO.Order
{
    public class OrderDetailGetResponseDTO
    {
        [JsonPropertyName("orderid")] // helps with de-serialization
        public string OrderId { get; set; }

        [JsonPropertyName("orderdetailid")]
        public string OrderDetailId { get; set; }

        [JsonPropertyName("itemid")]
        public int ItemId { get; set; }

        [JsonPropertyName("category")]
        public string Category { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("cost")]
        public decimal Cost { get; set; }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
