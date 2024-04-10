using Microsoft.AspNetCore.Mvc;

namespace Intex.Models.ViewModels
{
    public class AddProductViewModel : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
