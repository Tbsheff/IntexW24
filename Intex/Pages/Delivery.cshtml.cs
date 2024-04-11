using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Globalization;

namespace Intex.Pages
{
    public class DeliveryModel : PageModel
    {
        public decimal Subtotal { get; set; }
        public decimal Tax { get; set; }
        public decimal ShippingCost { get; set; }
        public decimal Total { get; set; }
        //public void OnGet()
        //{
        //    Subtotal = TempData.Peek("Subtotal") as decimal? ?? default;
        //    Tax = TempData.Peek("Tax") as decimal? ?? default;
        //    ShippingCost = TempData.Peek("ShippingCost") as decimal? ?? default;
        //    Total = TempData.Peek("Total") as decimal? ?? default;
        //}

        public void OnGet()
        {
            Subtotal = Convert.ToDecimal(TempData["Subtotal"] as string ?? "0", CultureInfo.InvariantCulture);
            ShippingCost = Convert.ToDecimal(TempData["ShippingCost"] as string ?? "0", CultureInfo.InvariantCulture);
            Tax = Convert.ToDecimal(TempData["Tax"] as string ?? "0", CultureInfo.InvariantCulture);
            Total = Convert.ToDecimal(TempData["Total"] as string ?? "0", CultureInfo.InvariantCulture);
        }

        // Property for list of countries.
            public List<string> Countries { get; set; } = new List<string>
        {
            "United States",
            "Canada",
            "United Kingdom",
            "Australia"
            // Add other countries as needed.
        };



    }
}
