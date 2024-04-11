using Microsoft.EntityFrameworkCore;

namespace Intex.Models
{
    public class EFLegoRepository : ILegoRepository
    {
        private MyDbContext _context;
        public EFLegoRepository(MyDbContext temp) {
        _context = temp;
        }
        public IEnumerable<AspNetRole> AspNetRoles => _context.AspNetRoles;
        public IEnumerable<AspNetRoleClaim> AspNetRoleClaims => _context.AspNetRoleClaims;
        public IEnumerable<AspNetUser> AspNetUsers => _context.AspNetUsers;
        public IEnumerable<AspNetUserClaim> AspNetUserClaims => _context.AspNetUserClaims;
        public IEnumerable<AspNetUserLogin> AspNetUserLogins => _context.AspNetUserLogins;
        public IEnumerable<AspNetUserToken> AspNetUserTokens => _context.AspNetUserTokens;
        public IEnumerable<Bank> Banks => _context.banks;
        public IEnumerable<Card_Type> Card_Types => _context.card_types;
        public IEnumerable<Category> Categories => _context.categories;
        public IEnumerable<Customer> Customers => _context.customers;
        public IEnumerable<Customer_Recommendation> Customer_Recommendations => _context.customer_recommendations;
        public IEnumerable<Entry_Mode> Entry_Modes => _context.entry_modes;
        public IEnumerable<Line_Item> Line_Items => _context.line_items;
        public IEnumerable<Order> Orders => _context.orders;
        public IEnumerable<Product> Products => _context.products;
        public IEnumerable<Product_Recommendation> Product_Recommendations => _context.product_recommendations;
        public IEnumerable<Rating> Ratings => _context.ratings;
        public IEnumerable<Transaction_Type> Transaction_Types => _context.transaction_types;
        public IEnumerable<User> Users => _context.users;
        public async Task<User> GetUserByIdAsync(short id)
        {
            return await _context.users.FindAsync(id);
        }

        public void UpdateUser(User user)
        {
            _context.users.Update(user);
        }
        
        public void UpdateProduct(Product product)
        {
            _context.products.Update(product);
        }
        
        public void AddProduct(Product product)
        {
            _context.products.Add(product);
        }
        
        public async Task AddOrder(Order order)
        {
            _context.orders.Add(order);
        }

        public async Task UpdateOrderAsync(Order order)
        {
            _context.orders.Update(order);
            await _context.SaveChangesAsync();
        }


        public async Task<Customer> GetByIdAsync(short id)
        {
            return await _context.customers.FindAsync(id);
        }

        public async Task UpdateCustomerAsync(Customer customer)
        {
            _context.customers.Update(customer);
        }
        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
        
        public async Task RemoveUser(short id)
        {
            // Find the user entity
            var user = await _context.users.FindAsync(id);

            if (user != null)
            {
                // Find the associated customer entity
                var customer = await _context.customers.FirstOrDefaultAsync(c => c.customer_ID == id);

                // Remove the user from the Users DbSet
                _context.users.Remove(user);

                // If a customer entity is found, remove it from the Customers DbSet
                if (customer != null)
                {
                    _context.customers.Remove(customer);
                }

                // Save the changes to the database
                await _context.SaveChangesAsync();
            }
        }

        public async Task RemoveAspUser(AspNetUser user)
        {
            _context.AspNetUsers.Remove(user);
        }
        
        public async Task RemoveProduct(short id)
        {
            var product = _context.products.FirstOrDefault(x => x.product_id == id);

             _context.Remove(product);
             
        }
        
        public void AddUser(User newUser)
        {
            _context.users.Add(newUser);
        }
        
        public void Save()
        {
            _context.SaveChanges();
        }

        public void AddCustomer(Customer customer)
        {
            _context.customers.Add(customer);
        }
    }
}
