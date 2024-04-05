using Microsoft.AspNetCore.Mvc;

namespace Intex.Components;

public class SliderViewComponent: ViewComponent
{
    
    public IViewComponentResult Invoke()
    {
        return View();
    }
}