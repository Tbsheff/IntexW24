using Intex.Models;
using Microsoft.AspNetCore.Mvc;

namespace Intex.Components;

public class RecommendedProductsViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(List<Product> recommendedProducts)
    {
        ViewBag.RecommendedProducts = recommendedProducts;
        return View();
    }
}