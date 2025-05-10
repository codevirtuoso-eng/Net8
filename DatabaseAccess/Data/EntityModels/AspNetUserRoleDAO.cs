using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace DatabaseAccess.Data.EntityModels
{
    [Table("AspNetUserRoles")]
    public class AspNetUserRoleDAO
    {
        [Column("UserId")]
        public string UserId { get; set; }

        [Column("RoleId")]
        public string RoleId { get; set; }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
