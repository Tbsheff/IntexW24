using Intex.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Globalization;
using Intex.Infrastructure;
using Intex.Models;


namespace Intex.Pages
{
    public class DeliveryModel : PageModel
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public DeliveryModel(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        [BindProperty]
        public DeliveryViewModel DeliveryInfo { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Tax { get; set; }
        public decimal ShippingCost { get; set; }
        public decimal Total { get; set; }
        
        public List<Cart.CartLine> CartItems { get; set; }
        
        public Cart Cart { get; set; } = new Cart();

        public void OnGet()
        {
            Cart = HttpContext.Session.GetJson<Cart>("cart") ?? new Cart();

            ShippingCost = 15.00m; // Example shipping cost
            Tax = Cart.CalculateTotal() * 0.08m; // Example tax rate

            Subtotal = Cart.CalculateTotal();
            Total = Subtotal + ShippingCost + Tax;

            ViewData["Subtotal"] = Cart.CalculateTotal();
            ViewData["EstimatedShipping"] = ShippingCost;
            ViewData["EstimatedTax"] = Tax;
            ViewData["Total"] = Cart.CalculateTotal() + ShippingCost + Tax;
        }
        
        public void OnPost([FromBody] List<Cart.CartLine> cartItems)
        {
            CartItems = cartItems;
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
        }

        

        /*
        //public void OnGet()
        //{
        //    Subtotal = TempData.Peek("Subtotal") as decimal? ?? default;
        //    Tax = TempData.Peek("Tax") as decimal? ?? default;
        //    ShippingCost = TempData.Peek("ShippingCost") as decimal? ?? default;
        //    Total = TempData.Peek("Total") as decimal? ?? default;
        //}

        /*public void OnGet()
        {
            Subtotal = Convert.ToDecimal(TempData["Subtotal"] as string ?? "0", CultureInfo.InvariantCulture);
            ShippingCost = Convert.ToDecimal(TempData["ShippingCost"] as string ?? "0", CultureInfo.InvariantCulture);
            Tax = Convert.ToDecimal(TempData["Tax"] as string ?? "0", CultureInfo.InvariantCulture);
            Total = Convert.ToDecimal(TempData["Total"] as string ?? "0", CultureInfo.InvariantCulture);
            
            if (Subtotal == 0 && ShippingCost == 0 && Tax == 0 && Total == 0)
            {
                Subtotal = 0;
                ShippingCost = 0;
                Tax = 0;
                Total = 0;
            }

            ViewData["Subtotal"] = Subtotal;
            ViewData["EstimatedShipping"] = ShippingCost;
            ViewData["EstimatedTax"] = Tax;
            ViewData["Total"] = Total;
        }#1#

        // Property for list of countries.
            /*public List<string> Countries { get; set; } = new List<string>
        {
            "United States",
            "Canada",
            "United Kingdom",
            "Australia"
            // Add other countries as needed.
        };#1#

        /*public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Serialize DeliveryInfo and store in session
            var deliveryInfoJson = System.Text.Json.JsonSerializer.Serialize(DeliveryInfo);
            _httpContextAccessor.HttpContext.Session.SetString("DeliveryInfo", deliveryInfoJson);

            return RedirectToPage("NextPage"); // Redirect to the next page or confirmation page
        }#1#
        */

    }
}
