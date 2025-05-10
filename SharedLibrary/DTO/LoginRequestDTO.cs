using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SharedLibrary.DTO
{
    public class LoginRequestDTO
    {
        [JsonPropertyName("username")]
        [Required]
        [MinLength(1)]
        [MaxLength(50)]
        public string UserName { get; set; }

        [JsonPropertyName("userpassword")]
        [Required]
        [MinLength(1)]
        [MaxLength(50)]
        public string UserPassword { get; set; }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
