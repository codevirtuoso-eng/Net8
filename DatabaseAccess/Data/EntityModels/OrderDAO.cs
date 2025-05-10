using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace DatabaseAccess.Data.EntityModels
{

    [Table("Orders")]
    public class OrderDAO
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("OrderId")]
        public string OrderId { get; set; }

        [Column("UserId")]
        public string UserId { get; set; }

        [Column("OrderTotal")]
        public decimal OrderTotal { get; set; }

        [Column("OrderDate")]
        public DateTime OrderDate { get; set; }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
