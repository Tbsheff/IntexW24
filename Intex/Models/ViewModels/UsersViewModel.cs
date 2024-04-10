namespace Intex.Models.ViewModels;

public class UsersViewModel
{
    public User User { get; set; }
    public Customer Customer { get; set; }
    
    public AspNetUser AspNetUser { get; set; }
    public string Role { get; set; }
    
}