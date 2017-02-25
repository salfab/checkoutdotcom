using System.ComponentModel.DataAnnotations;

namespace checkoutdotcom.Controllers
{
    public class DrinkOrderBase
    {
        [Required]
        public int? Quantity { get; set; }
    }
}