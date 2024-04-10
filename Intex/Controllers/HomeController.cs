using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Intex.Models;
using Intex.Components;
using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;


namespace Intex.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ILegoRepository _repo;
    private readonly InferenceSession _session;

    public HomeController(ILogger<HomeController> logger, ILegoRepository repo)
    {
        _logger = logger;
        _repo = repo;
        try 
        {
            _session = new InferenceSession("C:/Users/burke/source/repos/IntexW24/Intex/decision_tree_model.onnx");
            _logger.LogInformation("ONNX model loaded successfully.");
        }
        catch (Exception ex) 
        {
            _logger.LogInformation($"Error loading ONNX: {ex.Message}");
        }
    }

    [HttpGet]
    public IActionResult TestModel()
    {
        return View();
    }

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


    public IActionResult Index()
    {
        
        var recommendationIds = _repo.Ratings.OrderBy(x => x.rating1).Take(10).Select(r => r.product_ID).ToList();
        var recommendedProducts = _repo.Products.Where(p => recommendationIds.Contains(p.product_id)).ToList();
        ViewBag.RecommendedProducts = recommendedProducts;

        return View("IndexTest");
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
    
    public IActionResult Login()
    {
        return View();
    }

    public IActionResult About()
    {
        return View();
    }
    
    public IActionResult Product(int pageNum = 1, string category = "All", int pageSize = 10)
    {
       ViewBag.PageNum = pageNum;
       ViewBag.Category = category;
       ViewBag.PageSize = 10;
        return View();
    }

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


}