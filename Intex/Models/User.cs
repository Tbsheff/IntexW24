using System.ComponentModel.DataAnnotations;

namespace Intex.Models
{
    public class User
    {
        public required string username { get; set; }
        public int user_id { get; set; }
    }
}
