using Microsoft.AspNetCore.Mvc;

namespace Intex.Components;

public class BannerViewComponent : ViewComponent
{
    
    public IViewComponentResult Invoke()
    {
        return View();
    }
}