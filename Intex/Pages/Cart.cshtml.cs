using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Intex.Infrastructure;
using Intex.Models;

namespace Intex.Pages
{
    public class CartModel : PageModel
    {
        private ILegoRepository _repo;
        private readonly ILogger _logger;
        public CartModel(ILegoRepository temp, ILogger logger) 
        {
            _repo = temp;
            _logger = logger;
        }
        public Cart? Cart { get; set; }
        public void OnGet()
        {
            
        }

        public IActionResult OnPost(int product_id)
        {
            _logger.LogInformation($"{product_id}");
            Product pro = _repo.Products
                .FirstOrDefault(x => x.product_id == product_id);

            if (pro != null)
            {
                Cart.AddItem(pro, 1); // Adjust quantity based on form input
            }

            return RedirectToPage();
        }
    }
}
