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
        public IEnumerable<bank> banks => _context.banks;
        public IEnumerable<card_type> card_types => _context.card_types;
        public IEnumerable<category> categories => _context.categories;
        public IEnumerable<customer> customers => _context.customers;
        public IEnumerable<customer_recommendation> customer_recommendations => _context.customer_recommendations;
        public IEnumerable<entry_mode> entry_modes => _context.entry_modes;
        public IEnumerable<line_item> line_items => _context.line_items;
        public IEnumerable<order> orders => _context.orders;
        public IEnumerable<product> products => _context.products;
        public IEnumerable<product_recommendation> product_recommendations => _context.product_recommendations;
        public IEnumerable<rating> ratings => _context.ratings;
        public IEnumerable<transaction_type> transaction_types => _context.transaction_types;
    }
}
