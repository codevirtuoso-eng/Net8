using SharedLibrary.Common.Models;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SharedLibrary.DTO.AspNetUser
{
    public partial class AspNetUserRoleResponseDTO
    {
        [JsonPropertyName("rolelist")]
        public List<AspNetRole> RoleList { get; set; } = new List<AspNetRole>();

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
