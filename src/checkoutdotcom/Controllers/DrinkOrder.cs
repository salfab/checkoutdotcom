using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace checkoutdotcom.Controllers
{
    public class DrinkOrder
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public int? Quantity { get; set; }
    }
}