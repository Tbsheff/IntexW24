using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Intex.Infrastructure;
using Intex.Models;

namespace Intex.Pages
{
    public class CartModel : PageModel
    {
        private ILegoRepository _repo;
        public CartModel(ILegoRepository temp) 
        {
            _repo = temp;
        }
        public Cart? Cart { get; set; }
        public void OnGet()
        {
            Cart = HttpContext.Session.GetJson<Cart>("cart") ?? new Cart();
   
        }

        public void OnPost(int product_id)
        {
            Product pro = _repo.Products
                .FirstOrDefault(x => x.product_id == product_id);

            if(pro != null)
            {
                Cart = HttpContext.Session.GetJson<Cart>("cart") ?? new Cart();
                Cart.AddItem(pro, 1);
                HttpContext.Session.SetJson("cart", Cart);
                UpdateCartItemCount();
            }

        }

        private void UpdateCartItemCount()
        {
            var itemCount = Cart.Lines.Sum(x => x.Quantity);
            HttpContext.Session.SetInt32("CartItemCount", itemCount);
        }
    }
}