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
        IEnumerable<bank> banks { get; }
        IEnumerable<card_type> card_types { get; }
        IEnumerable<category> categories { get; }
        IEnumerable<customer> customers { get; }
        IEnumerable<customer_recommendation> customer_recommendations { get;}
        IEnumerable<entry_mode> entry_modes { get; }
        IEnumerable<line_item> line_items { get; }
        IEnumerable<order> orders { get; }
        IEnumerable<product> products { get; }
        IEnumerable<product_recommendation> product_recommendations { get; }
        IEnumerable<rating> ratings { get; }
        IEnumerable<transaction_type> transaction_types { get; }
    }
}
