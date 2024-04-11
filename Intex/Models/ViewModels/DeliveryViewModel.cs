using System.ComponentModel.DataAnnotations;

namespace Intex.Models.ViewModels
{
    public class DeliveryViewModel
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Country { get; set; }
    }
}
