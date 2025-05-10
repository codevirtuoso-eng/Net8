using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace DatabaseAccess.Data.EntityModels
{
    [Table("AspNetUsers")]
    public class AspNetUserDAO
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("Id")]
        public string Id { get; set; }

        [Column("UserName")]
        public string UserName { get; set; }

        [Column("UserPassword")]
        public string UserPassword { get; set; }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
