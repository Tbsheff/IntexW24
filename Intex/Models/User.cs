using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Intex.Models
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public short user_id { get; set; }
        public required string username { get; set; }
        
        public Customer Customer { get; set; }

    }
}