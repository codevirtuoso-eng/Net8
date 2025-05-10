using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace DatabaseAccess.Data.EntityModels
{
    [Table("AspNetRoles")]
    public partial class AspNetRoleDAO
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("Id")]
        public string Id { get; set; }

        [Column("Name")]
        public string Name { get; set; }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
