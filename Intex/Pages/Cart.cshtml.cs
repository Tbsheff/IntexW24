using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Intex.Infrastructure;
using Intex.Models;
using static Intex.Models.Cart;
using System.Linq;
using Newtonsoft.Json;

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
        
        /*public void OnPost()
        {
            // Retrieve the cart from the session
            Cart cart = HttpContext.Session.GetJson<Cart>("cart") ?? new Cart();

            // Check if the cart is empty
            if (cart.Lines.Count == 0)
            {
                // Optionally, add a TempData message to inform the user
                TempData["Message"] = "Your cart is empty. Please add items to your cart before proceeding.";

                // Redirect back to the Cart page
                Response.Redirect(Url.Page("Cart"));
                return; // Exit the method to prevent further processing
            }

            // If the cart is not empty, proceed with serialization and redirection
            var cartLineItemsJson = System.Text.Json.JsonSerializer.Serialize(cart.Lines);
            HttpContext.Session.SetString("CartLineItems", cartLineItemsJson);
            Response.Redirect(Url.Page("Delivery"));
        }*/




        public void OnPost(int product_id, int quantity = 1)
        {
            if (product_id != null)
            {
                Product pro = _repo.Products
                                .FirstOrDefault(x => x.product_id == product_id);
                if (pro != null)
                {
                    Cart = HttpContext.Session.GetJson<Cart>("cart") ?? new Cart();
                    Cart.AddItem(pro, quantity);
                    HttpContext.Session.SetJson("cart", Cart);
                UpdateCartItemCount();
                }
            }
            else
            {
                throw new Exception("Product id is null");
                RedirectToPage("Cart");
            }
            

            

        }


        public IActionResult OnPostRemoveItem(int product_id)
        {
            //System.Diagnostics.Debug.WriteLine($"Product ID to remove: {product_id}");

            Cart = HttpContext.Session.GetJson<Cart>("cart") ?? new Cart();

            Product pro = _repo.Products
                .FirstOrDefault(x => x.product_id == product_id);


            if (pro != null)
            {
                Cart.RemoveLine(pro);
                HttpContext.Session.SetJson("cart", Cart);
            }

            return RedirectToPage(new { returnUrl = HttpContext.Request.Path + HttpContext.Request.QueryString });
        }


        public IActionResult OnPostUpdateCart(string UpdatedCart)
        {
            Cart = HttpContext.Session.GetJson<Cart>("cart") ?? new Cart();
            List<CartLine> updatedCart = JsonConvert.DeserializeObject<List<CartLine>>(UpdatedCart);
            foreach (CartLine item in updatedCart)
            {
                if (item.name != null && item.Quantity > 0)
                {
                    Cart.UpdateItem(item.name.product_id, item.Quantity);
                }
            }

            HttpContext.Session.SetJson("cart", Cart);

            return RedirectToPage("Cart");
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

        private void UpdateCartItemCount()
        {
            var itemCount = Cart.Lines.Sum(x => x.Quantity);
            HttpContext.Session.SetInt32("CartItemCount", itemCount);
        }


    }


}