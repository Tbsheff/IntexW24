using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Intex.Models;
using Intex.Components;

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

    public IActionResult About()
    {
        return View();
    }
    
    public IActionResult Product(int pageNum = 1, string category = "All")
    {
       

        return View();
    }

}