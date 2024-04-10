using Microsoft.AspNetCore.Mvc;

namespace Intex.Models.ViewModels
{
    public class AddProductViewModel
    {
        // The product_id is auto-generated in the database, so it's omitted here.

        public string Name { get; set; }

        public short Year { get; set; } // Corresponds to short in the Product model

        public short NumParts { get; set; } // Corresponds to short in the Product model

        public short Price { get; set; } // Corresponds to short in the Product model

        public string ImgLink { get; set; }

        public string PrimaryColor { get; set; }

        public string SecondaryColor { get; set; }

        public string Description { get; set; }

        public byte CategoryId { get; set; } // Corresponds to byte in the Product model
    }
}

