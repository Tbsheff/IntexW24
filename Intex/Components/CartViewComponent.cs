using Microsoft.AspNetCore.Mvc;

namespace Intex.Components;

public class CartViewComponent : ViewComponent
{
        
        public IViewComponentResult Invoke()
        {
            return View();
        }
    
}