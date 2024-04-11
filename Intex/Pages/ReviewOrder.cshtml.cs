using Intex.Infrastructure;
using Intex.Models;
using Intex.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Intex.Pages;

public class ReviewOrder : PageModel
{
    private ILegoRepository _repo;
    public ReviewOrder(ILegoRepository temp)
    {
        _repo = temp;
    }
    public Cart? Cart { get; set; }
    
    public decimal Subtotal { get; set; }
    public decimal Tax { get; set; }
    public decimal ShippingCost { get; set; }
    public decimal Total { get; set; }
    
    public List<Cart.CartLine> CartLines { get; set; } = new List<Cart.CartLine>();
    
    public void OnGet()
    {
        Cart = HttpContext.Session.GetJson<Cart>("cart") ?? new Cart();
        Address address = HttpContext.Session.GetJson<Address>("address") ?? new Address();

        
        ShippingCost = 15.00m; // Example shipping cost
        Tax = Cart.CalculateTotal() * 0.08m; // Example tax rate

        Subtotal = Cart.CalculateTotal();
        Total = Subtotal + ShippingCost + Tax;

        ViewData["Subtotal"] = Cart.CalculateTotal();
        ViewData["EstimatedShipping"] = ShippingCost;
        ViewData["EstimatedTax"] = Tax;
        ViewData["Total"] = Cart.CalculateTotal() + ShippingCost + Tax;
        
        ViewData["FirstName"] = address.FirstName;
        ViewData["LastName"] = address.LastName;
        ViewData["Address"] = address.Street;
        ViewData["City"] = address.City;
        ViewData["State"] = address.State;
        ViewData["PostalCode"] = address.Zip;
        
        CartLines = Cart.Lines;
    }
    
    public async Task<IActionResult> OnPost()
    {
        Cart cart = HttpContext.Session.GetJson<Cart>("cart") ?? new Cart();
        Address address = HttpContext.Session.GetJson<Address>("address") ?? new Address();
        var cardDetails = HttpContext.Session.GetJson<CardDetails>("cardDetails") ?? new CardDetails();
        
        decimal shippingCost = 15.00m; // Example shipping cost
        decimal taxRate = 0.08m; // Example tax rate

        var userId = _repo.Users.Where(x => x.username == User.Identity.Name).FirstOrDefault();

        Order order = new Order
        {
            customer_ID = userId.user_id, // Set the customer ID accordingly
            date = DateOnly.FromDateTime(DateTime.UtcNow),
            day_of_week = DateTime.UtcNow.DayOfWeek.ToString(),
            hour = (byte)DateTime.UtcNow.Hour,
            entry_mode_id = 3, // Set the entry mode ID accordingly
            amount = (short)cart.CalculateTotal(), // Assuming the amount is in cents
            transaction_type_id = 3, // Set the transaction type ID accordingly
            country_of_transaction = "US", // Set the country code accordingly
            shipping_address = $"{address.Street}, {address.City}, {address.State} {address.Zip}", // Example shipping address format
            bank_id = cardDetails.BankId, // Set the bank ID accordingly
            card_type_id = cardDetails.CardTypeId, // Set the card type ID accordingly
            fraud = false // Assuming no fraud detection for now
        };

        await _repo.AddOrder(order);
        await _repo.SaveAsync();

        // Clear the session and redirect to a confirmation page
        HttpContext.Session.SetJson("cart", new Cart());
        HttpContext.Session.SetJson("address", new Address());
        return RedirectToPage("/OrderConfirmation");
    }
}