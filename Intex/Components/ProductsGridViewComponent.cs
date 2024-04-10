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
        public IViewComponentResult Invoke(int pageNum = 1, int pageSize = 10, string category = "All")
        {
            ProductsListViewModel viewModel;
            
            ViewBag.Categories = _repo.Categories.ToList();
            
            if (category == "All")
            {
                viewModel = new ProductsListViewModel
                {
                    Products = _repo.Products
                        .OrderBy(p => p.product_id)
                        .Skip((pageNum - 1) * pageSize)
                        .Take(pageSize)
                        .ToList(),
                
                    PaginationInfo = new PaginationInfo
                    {
                        CurrentPage = pageNum,
                        ItemsPerPage = pageSize,
                        TotalItems = _repo.Products.Count()
                    },
                    CurrentCategory = category
                };
            }
            else
            {
                var cat_id = _repo.Categories
                                .Where(c => c.description == category)
                                .Select(c => c.category_id)
                                .FirstOrDefault();
                
                           viewModel = new ProductsListViewModel
                            {
                                Products =_repo.Products.Where(p => p.category_id == cat_id)
                                    .OrderBy(p => p.product_id)
                                    .Skip((pageNum - 1) * pageSize)
                                    .Take(pageSize)
                                    .ToList(),
                
                                PaginationInfo = new PaginationInfo
                                {
                                    CurrentPage = pageNum,
                                    ItemsPerPage = pageSize,
                                    TotalItems = category == "All" ?
                                        _repo.Products.Count() :
                                        _repo.Products.Count(p => _repo.Categories.Any(c => c.category_id == p.category_id && c.description == category))
                                },
                                CurrentCategory = category
                            };
            }
            
            return View(viewModel);
        }
        
}