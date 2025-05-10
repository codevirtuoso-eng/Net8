using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace DatabaseAccess.Data.EntityModels
{
    [Table("AspNetUserClaims")]
    public class AspNetUserClaimDAO
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Id")]
        public int Id { get; set; }

        [Column("UserId")]
        public string UserId { get; set; }

        [Column("ClaimType")]
        public string ClaimType { get; set; }

        [Column("ClaimValue")]
        public string ClaimValue { get; set; }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }

    }
}
