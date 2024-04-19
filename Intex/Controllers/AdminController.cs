using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Intex.Models;
using Intex.Components;
using Intex.Models.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.ML;
using Microsoft.AspNetCore.Authorization;


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
    // Ensures only users with Admin role can access this action
    [Authorize(Roles = "Admin")]
    public IActionResult Index()
    {
        // Initializes gender options for forms where they might be needed
        Genders = new List<SelectListItem>
        {
            new SelectListItem("Male", "M"),
            new SelectListItem("Female", "F"),
        };

        // Fetches users, joins with customers, and returns a view with the user list
        var usersWithCustomersAndRoles = _repo.Users
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
            .ToList();

        ViewBag.Genders = Genders;
        return View(usersWithCustomersAndRoles);
    }

    // GET request for editing a user with the specified id
    [HttpGet("Admin/EditUser/{id?}")]
    public async Task<IActionResult> EditUser(short id )
    {
        // Fetches user details from the repository based on user id
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
    // POST request to update user details
    [Authorize(Roles = "Admin")]

    [HttpPost("Admin/EditUser/{id?}")]
    public async Task<IActionResult> EditUser(short id, UsersViewModel viewModel)
    { 
        id = viewModel.User.user_id;

        if (id > 0)
        {
            try
            {
                var user = await _repo.GetUserByIdAsync(id);
                var aspUser = _repo.AspNetUsers.FirstOrDefault(x => x.UserName == viewModel.User.username);
                
                if (user == null)
                {
                    return NotFound();
                }

                if (user.username != viewModel.User.username)
                {
                    user.username = viewModel.User.username;
                    aspUser.UserName = viewModel.User.username;
                    aspUser.NormalizedUserName = viewModel.User.username.ToUpper();
                    aspUser.Email = viewModel.User.username;
                    aspUser.NormalizedUserName = viewModel.User.username.ToUpper();
                    
                    _repo.UpdateUser(user);
                }
                // Update other user properties as needed
                

                var customer = await _repo.GetByIdAsync(id);
                if (customer != null)
                {
                    // update properties of customer object
                    customer.first_name = viewModel.Customer.first_name;
                    customer.last_name = viewModel.Customer.last_name;
                    customer.gender = viewModel.Customer.gender;
                    customer.birth_date = viewModel.Customer.birth_date;
                    customer.country_of_residence = viewModel.Customer.country_of_residence;
                    // push changes to database
                    await _repo.UpdateCustomerAsync(customer);
                }
                // save changes to database
                await _repo.SaveAsync();
            }
            // catch errors
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

    // Checks if a user exists by ID
    private async Task<bool> UserExists(short id)
    {
        return await _repo.GetUserByIdAsync(id) != null;
    }

    // Displays all products managed by an admin
    [Authorize(Roles = "Admin")]
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

    // Returns a form for adding a new product
    [Authorize(Roles = "Admin")]
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

    [Authorize(Roles = "Admin")]
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
            _repo.AddProduct(product); // Ensure your repository's AddProduct method is expecting a Product entity
            _repo.SaveAsync(); // Save the changes

            // Redirect to the ManageItems view to see the list of products
            return RedirectToAction("ManageItems");
        }

        // If the model state is not valid, return to the AddProduct view with the current model to show validation errors
        // Repopulate necessary data like categories
        ViewBag.Categories = new SelectList(_repo.Categories, "CategoryId", "CategoryName");
        return View(model);
    }
    [Authorize(Roles = "Admin")]
    [HttpGet]
    public IActionResult EditProduct(int id)
    {
        var product = _repo.Products.FirstOrDefault(p => p.product_id == id);
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

    // This method handles the HTTP POST request to edit a product. It requires administrative privileges.
    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> EditProduct(EditProductViewModel model)
    {
        // Check if the form data is valid according to the data annotations in the ViewModel
        if (ModelState.IsValid)
        {
            // Retrieve the product from the database using the product ID
            var product = _repo.Products.FirstOrDefault(p => p.product_id == model.ProductId);
            // If no product is found, return a NotFound response
            if (product == null)
            {
                return NotFound();
            }

            // Update the product's properties with the values from the form
            product.name = model.Name;
            product.year = model.Year;
            product.num_parts = model.NumParts;
            product.price = model.Price;
            product.img_link = model.ImgLink;
            product.primary_color = model.PrimaryColor;
            product.secondary_color = model.SecondaryColor;
            product.description = model.Description;
            product.category_id = model.CategoryId;

            try
            {
                // Attempt to update the product in the database
                _repo.UpdateProduct(product);
                await _repo.SaveAsync();
            }
            catch (Exception ex)
            {
                // Log the exception or handle it accordingly
                return StatusCode(500, "An error occurred while saving the product. Please try again.");
            }
            // If successful, redirect to the Manage Items page
            return RedirectToAction("ManageItems");
        }
        // If the model state is not valid, reload the Edit view with the current model to show validation errors
        return View("Edit", model);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> DeleteUser(short id)
    {
        try
        {
            // Get the user entity from the repository
            var user = await _repo.GetUserByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }
            

            // Get the associated customer entity
            var customer = await _repo.GetByIdAsync(id);

            // Remove the user from the AspNetUsers table
            var aspUser = _repo.AspNetUsers.FirstOrDefault(x => x.UserName == user.username);
            if (aspUser != null)
            {
                _repo.RemoveAspUser(aspUser);
                await _repo.SaveAsync();
            }

            // Remove the user from the Users table and customers table
            _repo.RemoveUser(id);
            

            // Save the changes to the database
            await _repo.SaveAsync();

            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            // Log the exception or handle it accordingly
            return StatusCode(500, "An error occurred while deleting the user. Please try again.");
        }
    }

    // Review orders suspected of fraud.
    [Authorize(Roles = "Admin")]
    public IActionResult ReviewOrders()
    {
        // Retrieve orders flagged as fraudulent and paginate results based on the provided pageIndex and pageSize.
        var orders = _repo.Orders.Where(o => o.fraud == true).ToList(); // Gets all orders
        var customers = _repo.Customers; // Gets all customers
        var entryModes = _repo.Entry_Modes; // Gets all entry modes
        var transactionTypes = _repo.Transaction_Types; // Gets all transaction types
        var banks = _repo.Banks; // Gets all banks
        var cardTypes = _repo.Card_Types; // Gets all card types

        // Join orders with other entities to create a detailed view model for each order.
        var orderDetails = (from o in orders
                join c in customers on o.customer_ID equals c.customer_ID
                join em in entryModes on o.entry_mode_id equals em.entry_mode_id
                join tt in transactionTypes on o.transaction_type_id equals tt.transaction_type_id
                join b in banks on o.bank_id equals b.bank_id
                join ct in cardTypes on o.card_type_id equals ct.card_type_id
                select new OrderDetailViewModel
                {
                    TransactionId = o.transaction_ID,
                    CustomerName = c.first_name + " " + c.last_name,
                    Date = o.date,
                    DayOfWeek = o.day_of_week,
                    Hour = o.hour,
                    EntryModeDescription = em.description,
                    Amount = o.amount,
                    TransactionTypeDescription = tt.description,
                    CountryOfTransaction = o.country_of_transaction,
                    ShippingAddress = o.shipping_address,
                    BankName = b.name,
                    CardTypeDescription = ct.description,
                    Fraud = o.fraud
                })
            .OrderByDescending(x => x.TransactionId)
            .Take(10)
            .ToList();

        // Pass the detailed orders to the view model.
        var model = new OrdersViewModel { Orders = orderDetails.ToList() };
        return View(model);
    }

    // This method handles the approval of an order by setting the fraud flag to false.
    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> ApproveOrder(int id)
    {
        _logger.LogInformation("Received request to approve order with transaction ID: {TransactionId}", id);

        // Retrieve the order by ID.
        var order = _repo.Orders.FirstOrDefault(o => o.transaction_ID == id);
        if (order != null)
        {
            _logger.LogInformation("Think this works");
            order.fraud = false; // Update the fraud status to false.
            _repo.UpdateOrderAsync(order); // Update the order in the database.
            _repo.SaveAsync(); // Ensure to await if it's asynchronous
            return RedirectToAction("ReviewOrders"); // Redirect to the ReviewOrders action or any other appropriate action
        }
        else
        {
            _logger.LogInformation("dang");
            // If order is not found, return a view or an error message
            return NotFound(); // Or return a View with a specific error message
        }
    }

    // This method handles the deletion of a product.
    [Authorize(Roles = "Admin")]
    [HttpPost]
     public async Task<IActionResult> DeleteProduct(short id)
     {
         try
         {
             
             // Remove the user from the Users table and customers table
             _repo.RemoveProduct(id);
                 
     
             // Save the changes to the database
             await _repo.SaveAsync();
     
             return RedirectToAction("ManageItems");
         }
         catch (Exception ex)
         {
             // Log the exception or handle it accordingly
             return StatusCode(500, "An error occurred while deleting the product. Please try again.");
         }
     }
     
     }







