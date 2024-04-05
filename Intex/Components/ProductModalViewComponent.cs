using Microsoft.AspNetCore.Mvc;

namespace Intex.Components;

public class ProductModalViewComponent : ViewComponent
{
    
    public IViewComponentResult Invoke()
    {
        return View();
    }
}