using Microsoft.AspNetCore.Mvc;

namespace Intex.Components;

public class ProductsGridViewComponent: ViewComponent
{
        
        public IViewComponentResult Invoke()
        {
            return View();
        }
}