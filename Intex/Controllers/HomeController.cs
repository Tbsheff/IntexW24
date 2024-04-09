using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Intex.Models;
using Intex.Components;
using Intex.Models.ViewModels;

namespace Intex.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ILegoRepository _repo;

    public HomeController(ILogger<HomeController> logger, ILegoRepository repo)
    {
        _logger = logger;
        _repo = repo;
    }



    public IActionResult Index()
    {
        return View("IndexTest");
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
    
    public IActionResult ShoppingCart()
    {
        return View();
    }
    
    public IActionResult Login()
    {
        return View();
    }

    public IActionResult Product(int pageNum = 1, string category = "All")
    {
        /*var pageSize = 10; // Set your desired page size
        
        var cat_id = _repo.Categories
            .Where(c => c.description == category)
            .Select(c => c.category_id)
            .FirstOrDefault();

        var viewModel = new ProductsListViewModel
        {
            Products =_repo.Products.Where(p => p.category_id == cat_id)
                .OrderBy(p => p.product_id)
                .Skip((pageNum - 1) * /*#1#pageSize)
                .Take(pageSize)
                .ToList(),

            PaginationInfo = new PaginationInfo
            {
                CurrentPage = pageNum,
                ItemsPerPage = pageSize,
                TotalItems = category == "All" ?
                    _repo.Products.Count() :
                    _repo.Products.Count(p => _repo.Categories.Any(c => c.category_id == p.category_id && c.description == category))
            },
            CurrentCategory = category
        };
        */


        return View();
    }


}