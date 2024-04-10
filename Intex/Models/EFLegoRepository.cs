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
        public void UpdateUser(User user)
        {
            // Update the entity state (Entity Framework approach)
            _context.Update(user);

           
        }
    }
}
