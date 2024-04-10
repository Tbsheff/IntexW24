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
        var recommendationIds = _repo.Ratings.OrderBy(x => x.rating1).Take(10).ToList();
        var recommendedProducts = new List<Product>();
        
        foreach (var recommendationId in recommendationIds)
        {
            var recommendedProduct = _repo.Products.FirstOrDefault(p => p.product_id == recommendationId.product_ID);
            if (recommendedProduct != null)
            {
                recommendedProducts.Add(recommendedProduct);
            }
        }
        ViewBag.RecommendedProducts = recommendedProducts;
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
       ViewBag.PageNum = pageNum;
       ViewBag.Category = category;
       ViewBag.PageSize = 10;
        return View();
    }

    public IActionResult ProductDetails(int id = 1)
    {
        ViewBag.Product = _repo.Products.FirstOrDefault(p => p.product_id == id);
        
        var recommendedProducts = new List<Product>();

        var recommendations = _repo.Product_Recommendations.FirstOrDefault(p => p.product_ID == id);

        if (recommendations != null)
        {
            var recommendationIds = new List<byte>
            {
                recommendations.recommendation_1,
                recommendations.recommendation_2,
                recommendations.recommendation_3,
                recommendations.recommendation_4,
                recommendations.recommendation_5,
                recommendations.recommendation_6,
                recommendations.recommendation_7,
                recommendations.recommendation_8,
                recommendations.recommendation_9,
                recommendations.recommendation_10
            };

            foreach (var recommendationId in recommendationIds)
            {
                var recommendedProduct = _repo.Products.FirstOrDefault(p => p.product_id == recommendationId);
                if (recommendedProduct != null)
                {
                    recommendedProducts.Add(recommendedProduct);
                }
            }
        }

        ViewBag.RecommendedProducts = recommendedProducts;
        
        return View();
    }


}