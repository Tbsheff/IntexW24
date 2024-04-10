using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Intex.Models;
using Intex.Components;
using Intex.Models.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

public class AdminController : Controller
{
    
    private readonly ILogger<AdminController> _logger;
    private readonly ILegoRepository _repo;
    public List<SelectListItem> Genders { get; set; }

    public AdminController(ILogger<AdminController> logger, ILegoRepository repo)
    {
        _logger = logger;
        _repo = repo;
    }
    
    public IActionResult Index()
    {
        Genders = new List<SelectListItem>
        {
            new SelectListItem("Male", "M"),
            new SelectListItem("Female", "F"),
        };
        
        var usersWithCustomersAndRoles = _repo.Users
            .Join(
                _repo.Customers,
                user => user.user_id, // Assuming 'user_id' is your join key in 'User'
                customer => customer.customer_ID, // Assuming 'UserId' is your join key in 'Customer'
                (user, customer) => new UsersViewModel
                {
                    User = user,
                    Customer = customer
                }
            )
            .ToList();


        // Materialize the first query
        /*var usersWithCustomers = _repo.Users
            .Join(
                _repo.Customers,
                user => user.user_id, // Assuming 'user_id' is your join key in 'Users'
                customer => customer.customer_ID, // Assuming 'customer_ID' is your join key in 'Customers'
                (user, customer) => new { User = user, Customer = customer }
            )
            .Join(_repo.AspNetUsers,
                u => u.User.username,
                un => un.UserName,
                (uc, aspNetUser) => new { uc.User, uc.Customer, AspNetUser = aspNetUser }
                )
            .Join(
                _repo.AspNetUserRoles, // Assuming this is the name of your UserRoles repo
                uc => uc.User.user_id, // User Id from the previous join
                userRole => userRole.UserId, // Assuming 'UserId' is the name in UserRoles
                (uc, userRole) => new { uc.User, uc.Customer, userRole.RoleId }
            )
            .Join(
                _repo.AspNetRoles, // Assuming this is the name of your Roles repo
                ucr => ucr.RoleId, // Role Id from previous join
                role => role.Id, // Assuming 'Id' is the name of the role identifier in AspNetRoles
                (ucr, role) => new UsersViewModel
                {
                    User = ucr.User,
                    Customer = ucr.Customer,
                    Role = role.Name // Or whatever property holds the role name
                }
            )
            .Take(10)
            .ToList();*/

        // Assign the materialized data to ViewBag
        ViewBag.Genders = Genders;
        return View(usersWithCustomersAndRoles);
    }
}