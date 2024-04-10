using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Intex.Infrastructure;
using Intex.Models;
using static Intex.Models.Cart;
using System.Linq;

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
            }


        public IActionResult OnPostCheckout()
        {
            Cart = HttpContext.Session.GetJson<Cart>("cart") ?? new Cart();

            // Calculate subtotal, tax, shippingCost, and total here
            decimal subtotal = Cart.Lines.Sum(cl => cl.name.price * cl.Quantity);
            decimal taxRate = 0.08m; // Example tax rate of 8%
            decimal shippingCost = 15.00m; // Example flat shipping cost
            decimal tax = subtotal * taxRate;
            decimal total = subtotal + tax + shippingCost;

            // Now assign these values to TempData
            TempData["Subtotal"] = subtotal.ToString("N2");
            TempData["Tax"] = tax.ToString("N2");
            TempData["ShippingCost"] = shippingCost.ToString("N2");
            TempData["Total"] = total.ToString("N2");

            return RedirectToPage("/Delivery");
        }
    }


}