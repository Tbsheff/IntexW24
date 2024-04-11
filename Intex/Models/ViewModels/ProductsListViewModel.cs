namespace Intex.Models.ViewModels;

public class ProductsListViewModel
{
    public IEnumerable<Product> Products { get; set; }
    public PaginationInfo PaginationInfo { get; set; }
    
    public string SelectedPrimaryColor { get; set; }
    public string SelectedSecondaryColor { get; set; }
    public string CurrentCategory { get; set; }
    
    public int SelectedPageSize { get; set; }
}
