using Intex.Models;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Intex.Models.ViewModels;

namespace Intex.Components;

public class SliderViewComponent: ViewComponent
{
    private ILegoRepository _repo;

    public SliderViewComponent(ILegoRepository repo)
    {
        _repo = repo;
    }
    public IViewComponentResult Invoke()
    {
        
        var productList = _repo.Products.ToList();
        var ratingList = _repo.Ratings.ToList();

        var products = productList
            .Join(ratingList, product => product.product_id, rating => rating.product_ID, (product, rating) => new
            {
                ProductId = product.product_id,
                ProductName = product.name,
                Year = product.year,
                NumParts = product.num_parts,
                Price = product.price,
                ImgLink = product.img_link,
                PrimaryColor = product.primary_color,
                SecondaryColor = product.secondary_color,
                Description = product.description,
                CategoryId = product.category_id,
                Rating = rating.rating1
            })
            .OrderByDescending(result => result.Rating)
            .Take(10)
            .Select(result => new Product
            {
                product_id = result.ProductId,
                name = result.ProductName,
                year = result.Year,
                num_parts = result.NumParts,
                price = result.Price,
                img_link = result.ImgLink,
                primary_color = result.PrimaryColor,
                secondary_color = result.SecondaryColor,
                description = result.Description,
                category_id = result.CategoryId
            })
            .ToList();

        Random random = new Random();
        List<Product> newProducts = products.OrderBy(x => random.Next()).ToList();

        IndexViewModel viewModel = new IndexViewModel
        {
            Products = newProducts
        };

        return View(viewModel);
    }

}