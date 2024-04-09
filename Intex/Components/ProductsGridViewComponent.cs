using Intex.Models;
using Microsoft.AspNetCore.Mvc;

namespace Intex.Components;

public class ProductsGridViewComponent: ViewComponent
{
        
        public IViewComponentResult Invoke(List<Product> products)
        {
            return View(products);
        }
}