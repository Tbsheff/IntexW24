using Intex.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Globalization;
using Intex.Infrastructure;
using Intex.Models;
using Microsoft.AspNetCore.Authorization;


namespace Intex.Pages
{
    [Authorize] 
    public class DeliveryModel : PageModel
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public DeliveryModel(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        //Create model to have everything we need for delivery

        [BindProperty]
        public DeliveryViewModel DeliveryInfo { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Tax { get; set; }
        public decimal ShippingCost { get; set; }
        public decimal Total { get; set; }
        
        public List<Cart.CartLine> CartItems { get; set; }
        
        public Cart Cart { get; set; } = new Cart();

        //Pass info 
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
        
        //Update session address
        public void OnPost()
        {
            HttpContext.Session.SetJson("address", new Address
            {
                FirstName = Request.Form["DeliveryInfo.FirstName"],
                LastName = Request.Form["DeliveryInfo.LastName"],
                Street = Request.Form["address"],
                City = Request.Form["city"],
                State = Request.Form["state"],
                Zip = Request.Form["postalCode"],
                Country = Request.Form["DeliveryInfo.Country"]
            });

            Response.Redirect(Url.Page("Payment"));
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
