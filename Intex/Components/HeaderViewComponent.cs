using Microsoft.AspNetCore.Mvc;

namespace Intex.Components;

public class HeaderViewComponent: ViewComponent
{
    
    public IViewComponentResult Invoke()
    {
        return View();
    }
}