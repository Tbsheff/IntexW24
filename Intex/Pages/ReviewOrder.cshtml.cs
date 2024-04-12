using Intex.Infrastructure;
using Intex.Models;
using Intex.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.ML.OnnxRuntime.Tensors;
using Microsoft.ML.OnnxRuntime;

namespace Intex.Pages;

public class ReviewOrder : PageModel
{
    private ILegoRepository _repo;
    private readonly InferenceSession _session;
    private readonly IWebHostEnvironment _env;
    public ReviewOrder(ILegoRepository temp, IWebHostEnvironment env)
    {
        _repo = temp;
        
        _env = env;
        //Load onnx model
        string modelPath = Path.Combine(_env.WebRootPath, "decision_tree_model.onnx");
        _session = new InferenceSession(modelPath);
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

        //update order stuff
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

        //load in the customer data, for ease of referencing
        Customer customer = _repo.Customers.Where(x => x.customer_ID == order.customer_ID).FirstOrDefault();

        //we put the data into a model so that it would be easier to organize
        PredictionInput input = new PredictionInput
        {
            CustomerID = order.customer_ID,
            Time = order.hour,
            Amount = order.amount,
            Age = customer.age,
            DayOfWeekMon = (order.day_of_week == "Monday" ? 1 : 0),
            DayOfWeekSat = (order.day_of_week == "Saturday" ? 1 : 0),
            DayOfWeekSun = (order.day_of_week == "Sunday" ? 1 : 0),
            DayOfWeekThu = (order.day_of_week == "Thursday" ? 1 : 0),
            DayOfWeekTue = (order.day_of_week == "Tuesday" ? 1 : 0),
            DayOfWeekWed = (order.day_of_week == "Wednesday" ? 1 : 0),
            EntryModePIN = 0,
            EntryModeTap = 0,
            TypeOfTransactionOnline = 1,
            TypeOfTransactionPOS = 0,
            CountryOfTransactionIndia = 0,
            CountryOfTransactionRussia = 0,
            CountryOfTransactionUSA = 1,
            CountryOfTransactionUnitedKingdom = 0,
            ShippingAddressIndia = 0,
            ShippingAddressRussia = 0,
            ShippingAddressUnitedKingdom = 0,
            ShippingAddressUSA = 1,
            BankHSBC = (order.bank_id == 6 ? 1 : 0),
            BankHalifax = (order.bank_id == 4 ? 1 : 0),
            BankLloyds = (order.bank_id == 2 ? 1 : 0),
            BankMetro = (order.bank_id == 7 ? 1 : 0),
            BankMonzo = (order.bank_id == 5 ? 1 : 0),
            BankRBS = (order.bank_id == 1 ? 1 : 0),
            TypeOfCardVisa = (order.card_type_id == 1 ? 1 : 0),
            GenderM = (customer.gender == "M" ? 1 : 0),
            CountryOfResidenceIndia = (customer.country_of_residence == "India" ? 1 : 0),
            CountryOfResidenceRussia = (customer.country_of_residence == "Russia" ? 1 : 0),
            CountryOfResidenceUnitedKingdom = (customer.country_of_residence == "United Kingdom" ? 1 : 0),
            CountryOfResidenceUSA = (customer.country_of_residence == "USA" ? 1 : 0)
        };
        //call function to run prediction

        int fraudulent = Predict(input.CustomerID, input.Time, input.Amount, input.Age, input.DayOfWeekMon,
            input.DayOfWeekSat, input.DayOfWeekSun, input.DayOfWeekThu, input.DayOfWeekTue, input.DayOfWeekWed,
            input.EntryModePIN, input.EntryModeTap, input.TypeOfTransactionOnline, input.TypeOfTransactionPOS,
            input.CountryOfTransactionIndia, input.CountryOfTransactionRussia, input.CountryOfTransactionUSA,
            input.CountryOfTransactionUnitedKingdom, input.ShippingAddressIndia, input.ShippingAddressRussia,
            input.ShippingAddressUSA, input.ShippingAddressUnitedKingdom, input.BankHSBC, input.BankHalifax,
            input.BankLloyds, input.BankMetro, input.BankMonzo, input.BankRBS, input.TypeOfCardVisa, input.GenderM,
            input.CountryOfResidenceIndia, input.CountryOfResidenceRussia, input.CountryOfResidenceUSA,
            input.CountryOfResidenceUnitedKingdom);
        //determine which page to show, depending on if the order is potentially fraudulent
        if (fraudulent == 0)
        { 
            return RedirectToPage("/OrderConfirmation"); 
        }
        else
        {
            return RedirectToPage("/OrderUnderReview");
        }
    }

    //function to run model prediciton (taken from videos on LS)
    //I changed it to return an int so that we can process to figure out which page to display
    public int Predict(int customer_ID, int time, int amount, int age, int day_of_week_Mon,
       int day_of_week_Sat, int day_of_week_Sun, int day_of_week_Thu,
       int day_of_week_Tue, int day_of_week_Wed, int entry_mode_PIN,
       int entry_mode_Tap, int type_of_transaction_Online,
       int type_of_transaction_POS, int country_of_transaction_India,
       int country_of_transaction_Russia, int country_of_transaction_USA,
       int country_of_transaction_United_Kingdom, int shipping_address_India,
       int shipping_address_Russia, int shipping_address_USA,
       int shipping_address_United_Kingdom, int bank_HSBC, int bank_Halifax,
       int bank_Lloyds, int bank_Metro, int bank_Monzo, int bank_RBS,
       int type_of_card_Visa, int gender_M, int country_of_residence_India,
       int country_of_residence_Russia, int country_of_residence_USA,
       int country_of_residence_United_Kingdom)
    {
        
            var input = new List<float> {customer_ID, time, amount, age, day_of_week_Mon,
            day_of_week_Sat, day_of_week_Sun, day_of_week_Thu,
            day_of_week_Tue, day_of_week_Wed, entry_mode_PIN,
            entry_mode_Tap, type_of_transaction_Online,
            type_of_transaction_POS, country_of_transaction_India,
            country_of_transaction_Russia, country_of_transaction_USA,
            country_of_transaction_United_Kingdom, shipping_address_India,
            shipping_address_Russia, shipping_address_USA,
            shipping_address_United_Kingdom, bank_HSBC, bank_Halifax,
            bank_Lloyds, bank_Metro, bank_Monzo, bank_RBS,
            type_of_card_Visa, gender_M, country_of_residence_India,
            country_of_residence_Russia, country_of_residence_USA,
            country_of_residence_United_Kingdom };

            var inputTensor = new DenseTensor<float>(input.ToArray(), new[] { 1, input.Count });

            var inputs = new List<NamedOnnxValue>
            {
                NamedOnnxValue.CreateFromTensor("float_input", inputTensor)
            };

            using (var results = _session.Run(inputs))
            {
                var prediction = results.FirstOrDefault(item => item.Name == "output_label")?.AsTensor<long>().ToArray();
                var fraudulent = (int)prediction[0];
                return fraudulent;
            }            
    }
}