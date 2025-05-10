using SharedLibrary.Common.Models;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SharedLibrary.DTO.AspNetUser
{
    public partial class AspNetUserClaimResponseDTO
    {
        [JsonPropertyName("claimlist")]
        public List<AspNetUserClaim> ClaimList { get; set; } = new List<AspNetUserClaim>();

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }

    }
}
