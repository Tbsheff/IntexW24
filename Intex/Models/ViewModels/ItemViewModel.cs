using Microsoft.AspNetCore.Mvc;

namespace Intex.Models.ViewModels
{
    public class ItemViewModel
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int Year { get; set; }
        public int NumberOfParts { get; set; }
        public int Price { get; set; }
        // Include any other properties you need for the product
    }

}
