using Intex.Infrastructure;
using Intex.Models;
using Intex.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Intex.Pages;

public class Payment : PageModel
{
    private ILegoRepository _repo;
    public Payment(ILegoRepository temp)
    {
        _repo = temp;
    }
    public Cart? Cart { get; set; }
    
    public decimal Subtotal { get; set; }
    public decimal Tax { get; set; }
    public decimal ShippingCost { get; set; }
    public decimal Total { get; set; }
    
    public List<Bank> Banks { get; set; }
    public List<Card_Type> Cards { get; set; }
    
    //Get all the info for payment stuff
    public void OnGet()
    {
        Cart = HttpContext.Session.GetJson<Cart>("cart") ?? new Cart();
        Banks = _repo.Banks.ToList();
        Cards = _repo.Card_Types.ToList();
        
        
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
       
        
    }

    //Post stuff to cart
    public void OnPost()
    {
        HttpContext.Session.SetJson("address", new Address
        {
            FirstName = Request.Form["firstName"],
            LastName = Request.Form["lastName"],
            Street = Request.Form["address"],
            City = Request.Form["city"],
            State = Request.Form["state"],
            Zip = Request.Form["postalCode"]
        });
        
        
        HttpContext.Session.SetJson("cardDetails", new CardDetails
           {
               
               BankId = Convert.ToByte(Request.Form["bank"]),
               CardTypeId = Convert.ToByte(Request.Form["cardType"])
           });
        
        Response.Redirect(Url.Page("ReviewOrder"));
    }
    
    
    
    
}

internal class CardDetails
{
    public byte BankId { get; set; }
    public byte CardTypeId { get; set; }
}

public class Address
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Street { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string Zip { get; set; }
    public string Country { get; set; }
}