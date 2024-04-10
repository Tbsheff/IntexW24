using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Intex.Models;
using Intex.Components;
using Intex.Models.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;


public class AdminController : Controller
{
    
    private readonly ILogger<AdminController> _logger;
    private readonly ILegoRepository _repo;
    public List<SelectListItem> Genders { get; set; }

    public AdminController(ILogger<AdminController> logger, ILegoRepository repo)
    {
        _logger = logger;
        _repo = repo;
    }
    
    public IActionResult Index()
    {
        Genders = new List<SelectListItem>
        {
            new SelectListItem("Male", "M"),
            new SelectListItem("Female", "F"),
        };
        
        var usersWithCustomersAndRoles = _repo.Users
            .Join(
                _repo.Customers,
                user => user.user_id, // Assuming 'user_id' is your join key in 'User'
                customer => customer.customer_ID, // Assuming 'UserId' is your join key in 'Customer'
                (user, customer) => new UsersViewModel
                {
                    User = user,
                    Customer = customer
                }
            )
            .ToList();

        ViewBag.Genders = Genders;
        return View(usersWithCustomersAndRoles);
    }
    
    public async Task<IActionResult> EditUser(short id = 30001)
    {
        var userViewModel =  _repo.Users
            .Join(
                _repo.Customers,
                user => user.user_id,
                customer => customer.customer_ID,
                (user, customer) => new UsersViewModel
                {
                    User = user,
                    Customer = customer
                }
            )
            .Where(u => u.User.user_id == id)
            .ToList()
            .FirstOrDefault();

        if (userViewModel == null)
        {
            return NotFound();
        }

        return View(userViewModel);
    }

    [HttpPost]
    
    public async Task<IActionResult> EditUser(short id, UsersViewModel viewModel)
    {
        if (id != viewModel.User.user_id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                var user = await _repo.GetUserByIdAsync(id);
                if (user == null)
                {
                    return NotFound();
                }

                user.username = viewModel.User.username;
                // Update other user properties as needed
                _repo.UpdateUser(user);

                var customer = await _repo.GetByIdAsync(id);
                if (customer != null)
                {
                    customer.first_name = viewModel.Customer.first_name;
                    customer.last_name = viewModel.Customer.last_name;
                    // Update other customer properties as needed
                    await _repo.UpdateCustomerAsync(customer);
                }

                await _repo.SaveAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index)); // Redirect to the index action or wherever you want
        }
        return View(viewModel);
    }

    private async Task<bool> UserExists(short id)
    {
        return await _repo.GetUserByIdAsync(id) != null;
    }

    

    public IActionResult ManageItems()
    {
        var products = _repo.Products
            .Select(p => new ItemViewModel
            {
                ProductId = p.product_id,
                ProductName = p.name,
                Year = p.year,
                NumberOfParts = p.num_parts,
                Price = p.price
            })
            .ToList();

        return View(products);
    }


    [HttpGet]
    public IActionResult AddProduct()
    {
        // Create a new instance of the AddProductViewModel
        var model = new AddProductViewModel();

        // Attempt to fetch categories from the repository
        var categories = _repo.Categories?.ToList();

        // Check if the categories list is null or empty
        if (categories == null || !categories.Any())
        {
            // Handle the scenario where there are no categories
            // You could log this situation because it's unexpected based on your application's requirements
            _logger.LogWarning("Categories list is null or empty when accessing AddProduct view.");

            // Initialize an empty SelectList to prevent null reference in the view
            ViewBag.Categories = new SelectList(Enumerable.Empty<SelectListItem>());
        }
        else
        {
            // Successfully retrieved categories, pass them to the view via ViewBag
            ViewBag.Categories = new SelectList(categories, "CategoryId", "CategoryName");
        }

        // Pass the model to the view
        return View(model);
    }


    [HttpPost]
    public IActionResult AddProduct(AddProductViewModel model)
    {
        if (ModelState.IsValid)
        {
            // Map the ViewModel to your database model
            var product = new Product
            {
                // Assuming 'product_id' is auto-generated by the database, so it's not set here

                name = model.Name, // Make sure the property names match your Product model and ViewModel
                year = model.Year,
                num_parts = model.NumParts,
                price = model.Price,
                img_link = model.ImgLink,
                primary_color = model.PrimaryColor,
                secondary_color = model.SecondaryColor,
                description = model.Description,
                category_id = model.CategoryId
                // 'DescriptionShort' is not included as it's not part of your Product model
            };

            // Add and save in the database
            //_repo.AddProduct(product); // Ensure your repository's AddProduct method is expecting a Product entity
            //_repo.SaveChanges(); // Save the changes

            // Redirect to the ManageItems view to see the list of products
            return RedirectToAction("ManageItems");
        }

        // If the model state is not valid, return to the AddProduct view with the current model to show validation errors
        // Repopulate necessary data like categories
        ViewBag.Categories = new SelectList(_repo.Categories, "CategoryId", "CategoryName");
        return View(model);
    }

    [HttpGet]
    public IActionResult EditProduct(int productId)
    {
        var product = _repo.Products.FirstOrDefault(p => p.product_id == productId);
        if (product == null)
        {
            return NotFound();
        }

        var model = new EditProductViewModel
        {
            ProductId = product.product_id,
            Name = product.name,
            Year = product.year,
            NumParts = product.num_parts,
            Price = product.price,
            ImgLink = product.img_link,
            PrimaryColor = product.primary_color,
            SecondaryColor = product.secondary_color,
            Description = product.description,
            CategoryId = product.category_id
        };

        return View("Edit", model);
    }

    [HttpPost]
    public IActionResult EditProduct(EditProductViewModel model)
    {
        if (ModelState.IsValid)
        {
            var product = _repo.Products.FirstOrDefault(p => p.product_id == model.ProductId);
            if (product == null)
            {
                return NotFound();
            }

            product.name = model.Name;
            product.year = model.Year;
            product.num_parts = model.NumParts;
            product.price = model.Price;
            product.img_link = model.ImgLink;
            product.primary_color = model.PrimaryColor;
            product.secondary_color = model.SecondaryColor;
            product.description = model.Description;
            product.category_id = model.CategoryId;

            //_repo.SaveChanges();

            return RedirectToAction("ManageItems");
        }
        return View("Edit", model);
    }


}