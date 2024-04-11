namespace Intex.Models
{
    public interface ILegoRepository
    {
        IEnumerable<AspNetRole> AspNetRoles { get; }
        IEnumerable<AspNetRoleClaim> AspNetRoleClaims { get; }
        IEnumerable<AspNetUser> AspNetUsers { get; }
        IEnumerable<AspNetUserClaim> AspNetUserClaims { get; }
        IEnumerable<AspNetUserLogin> AspNetUserLogins { get; }
        IEnumerable<AspNetUserToken> AspNetUserTokens { get; }
        IEnumerable<Bank> Banks { get; }
        IEnumerable<Card_Type> Card_Types { get; }
        IEnumerable<Category> Categories { get; }
        IEnumerable<Customer> Customers { get; }
        IEnumerable<Customer_Recommendation> Customer_Recommendations { get;}
        IEnumerable<Entry_Mode> Entry_Modes { get; }
        IEnumerable<Line_Item> Line_Items { get; }
        IEnumerable<Order> Orders { get; }
        IEnumerable<Product> Products { get; }
        IEnumerable<Product_Recommendation> Product_Recommendations { get; }
        IEnumerable<Rating> Ratings { get; }
        IEnumerable<Transaction_Type> Transaction_Types { get; }
        IEnumerable<User> Users { get; }
        void UpdateUser(User user);
        void UpdateProduct(Product product);
        void AddProduct(Product product);
        Task UpdateOrderAsync(Order order);
        Task<User> GetUserByIdAsync(short id);

        Task<Customer> GetByIdAsync(short id);
        Task UpdateCustomerAsync(Customer customer);
        Task SaveAsync();

        Task RemoveUser(short id);
        Task RemoveAspUser(AspNetUser user);
        Task RemoveProduct(short id);
        Task AddOrder(Order order);
        void AddUser(User newUser);
        void Save();
        void AddCustomer(Customer customer);
    }
}
