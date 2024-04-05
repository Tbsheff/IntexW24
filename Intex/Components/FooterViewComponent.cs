using Microsoft.AspNetCore.Mvc;

namespace Intex.Components;

public class FooterViewComponent : ViewComponent
{
        
        public IViewComponentResult Invoke()
        {
            return View();
        }
}