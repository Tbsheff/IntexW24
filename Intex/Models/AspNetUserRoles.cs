using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Intex.Models;

public class AspNetUserRole
{
    [ForeignKey("User"), Column(Order = 0)]
    public string UserId { get; set; }
    
    [ForeignKey("Role"), Column(Order = 1)]
    public string RoleId { get; set; }

    public virtual AspNetUser User { get; set; }
    public virtual AspNetRole Role { get; set; }
}
