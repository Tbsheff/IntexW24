using Intex.Models;
using Intex.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Intex.Components;

public class ProductsGridViewComponent: ViewComponent
{
    private ILegoRepository _repo;
    
    public ProductsGridViewComponent(ILegoRepository repo)
    {
        _repo = repo;
    }
    public IViewComponentResult Invoke(int pageNum = 1, int pageSize = 10, string category = "All", string primaryColor = "All", string secondaryColor = "All")
    {
        ProductsListViewModel viewModel;
        ViewBag.Categories = _repo.Categories.ToList();
        ViewBag.PrimaryColor = _repo.Products.Select(x => x.primary_color).Distinct().ToList();
        ViewBag.SecondaryColor = _repo.Products.Select(x => x.secondary_color).Distinct().ToList();

        IEnumerable<Product> filteredProducts = _repo.Products;

        if (category != "All")
        {
            var catid = _repo.Categories
                .Where(c => c.description == category)
                .Select(c => c.category_id)
                .FirstOrDefault();

            filteredProducts = filteredProducts.Where(p => p.category_id == catid);
        }

        if (primaryColor != "All")
        {
            filteredProducts = filteredProducts.Where(p => p.primary_color == primaryColor);
        }

        if (secondaryColor != "All")
        {
            filteredProducts = filteredProducts.Where(p => p.secondary_color == secondaryColor);
        }

        viewModel = new ProductsListViewModel
        {
            Products = filteredProducts
                .OrderBy(p => p.product_id)
                .Skip((pageNum - 1) * pageSize)
                .Take(pageSize)
                .ToList(),
            PaginationInfo = new PaginationInfo
            {
                CurrentPage = pageNum,
                ItemsPerPage = pageSize,
                TotalItems = filteredProducts.Count()
            },
            
            CurrentCategory = category,
            SelectedPrimaryColor = primaryColor,
            SelectedSecondaryColor = secondaryColor,
            SelectedPageSize = pageSize
        };

        return View(viewModel);
    }
}