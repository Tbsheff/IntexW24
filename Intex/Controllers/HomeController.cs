using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Intex.Models;
using Intex.Components;
using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;
using Intex.Models.ViewModels;
using System.Linq;
using System.Text.Json;

namespace Intex.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ILegoRepository _repo;
    private readonly InferenceSession _session;
    private IWebHostEnvironment _env;

    // Constructor that initializes necessary services and the ONNX model session
    public HomeController(ILogger<HomeController> logger, ILegoRepository repo, IWebHostEnvironment env)
    {
        _logger = logger;
        _repo = repo;
        _env = env;
        string modelPath = Path.Combine(_env.ContentRootPath, "decision_tree_model.onnx");
        try 
        {
            
            _session = new InferenceSession(modelPath);
            _logger.LogInformation("ONNX model loaded successfully.");
        }
        catch (Exception ex) 
        {
            _logger.LogInformation($"Error loading ONNX: {ex.Message}");
        }
    }

    // Display the initial test model form
    [HttpGet]
    public IActionResult TestModel()
    {
        return View();
    }

    // Process predictions based on model inputs received via POST
    [HttpPost]
    public IActionResult Predict(int customer_ID, int time, int amount, int age, int day_of_week_Mon,
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
        try
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
                if (prediction != null && prediction.Length > 0)
                {
                    var fraudulent = (int)prediction[0];
                    ViewBag.Prediction = fraudulent;
                }
                else
                {
                    ViewBag.Prediction = "Error in making prediction";
                }
            }

            _logger.LogInformation("Prediction executed successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error during prediction: {ex.Message}");
            ViewBag.Prediction = "Error during prediction.";
        }

        return View("TestModel");
    }

    // Default landing page showing a product recommendation list based on user authentication and preferences
    public IActionResult Index()
    {
        // Business logic to retrieve recommended product IDs
        var recommendationIds = _repo.Ratings.OrderBy(x => x.rating1).Take(10).Select(r => r.product_ID).ToList();


        // Authenticated users receive personalized recommendations
        if (User.Identity.IsAuthenticated)
        {
            string username = User.Identity.Name;
            int userId = _repo.Users.Where(x => x.username == username).Select(x => x.user_id).FirstOrDefault();

            if (userId != 0) 
            {
                int customerId = _repo.Customers.Where(x => x.customer_ID == userId).Select(x => x.customer_ID).FirstOrDefault();

                if (customerId != 0)
                {
                    var recommendations = _repo.Customer_Recommendations
                        .Where(x => x.customer_id == customerId)
                        .Select(r => new List<byte>
                        {
                            r.recommendation_1,
                            r.recommendation_2,
                            r.recommendation_3,
                            r.recommendation_4,
                            r.recommendation_5,
                            r.recommendation_6,
                            r.recommendation_7,
                            r.recommendation_8,
                            r.recommendation_9,
                            r.recommendation_10
                        })
                        .FirstOrDefault();

                    if (recommendations != null)
                    {
                        recommendationIds = recommendations;
                    }
                }
            }
        }

        var recommendedProducts = _repo.Products.Where(p => recommendationIds.Contains(p.product_id)).ToList();
        ViewBag.RecommendedProducts = recommendedProducts;

        return View("IndexTest");
    }

    // Returns the Privacy policy view
    public IActionResult Privacy()
    {
        return View();
    }

    // Returns an error page, often used to display error messages and diagnostics
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    // Returns the login page view
    public IActionResult Login()
    {
        return View();
    }

    // Returns the about page view
    public IActionResult About()
    {
        return View();
    }

    // Returns the products list based on filters applied via query parameters
    public IActionResult Product(int pageNum = 1,  string category = "All", int pageSize = 10, string primaryColor = "All", string secondaryColor = "All")
    { 
        List<int> pageSizes = new List<int>{  5,10, 20 };
       ViewBag.PageNum = pageNum;
       ViewBag.Category = category;
       ViewBag.PageSize = pageSize;
       ViewBag.PrimaryColor = primaryColor;
       ViewBag.SecondaryColor = secondaryColor;
       ViewBag.pageSizes = pageSizes;
       
        return View();
    }

    // Displays product details based on the product ID and shows recommended products
    public IActionResult ProductDetails(int id = 1)
    {
        ViewBag.Product = _repo.Products.FirstOrDefault(p => p.product_id == id);
        
        var recommendedProducts = new List<Product>();

        var recommendations = _repo.Product_Recommendations.FirstOrDefault(p => p.product_ID == id);

        if (recommendations != null)
        {
            var recommendationIds = new List<byte>
            {
                recommendations.recommendation_1,
                recommendations.recommendation_2,
                recommendations.recommendation_3,
                recommendations.recommendation_4,
                recommendations.recommendation_5,
                recommendations.recommendation_6,
                recommendations.recommendation_7,
                recommendations.recommendation_8,
                recommendations.recommendation_9,
                recommendations.recommendation_10
            };

            foreach (var recommendationId in recommendationIds)
            {
                var recommendedProduct = _repo.Products.FirstOrDefault(p => p.product_id == recommendationId);
                if (recommendedProduct != null)
                {
                    recommendedProducts.Add(recommendedProduct);
                }
            }
        }

        ViewBag.RecommendedProducts = recommendedProducts;
        
        return View();
    }

    // Returns the delivery information view with a form
    [HttpGet]
    public IActionResult Delivery()
    {
        return View(new DeliveryViewModel());
    }

    // Processes the submitted delivery form, saves information if valid or returns errors
    [HttpPost]
    public IActionResult Delivery(DeliveryViewModel model)
    {
        if (ModelState.IsValid)
        {

            // Map the model to your entities and save them
            return RedirectToAction("Index"); // or to a confirmation page
        }

        // If model state is not valid, return to the form with the model to show validation errors
        return View(model);
    }

    // Returns the payment information view
    public IActionResult Payment()
    {
        return View();
    }


    // Validates the reCAPTCHA response to prevent bots from submitting forms
    public bool IsReCaptchaValid(string captchaResponse)
    {
        string secret = "6LevL7gpAAAAAEacEYR48YIhvEcgiroys4W1-uzz";
        var client = new HttpClient();
        string result = client.GetStringAsync($"https://www.google.com/recaptcha/api/siteverify?secret={secret}&response={captchaResponse}").Result;
        var obj = JsonSerializer.Deserialize<JsonElement>(result);
        return obj.GetProperty("success").GetBoolean();
    }


}