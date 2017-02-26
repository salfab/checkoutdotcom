using System.ComponentModel.DataAnnotations;

namespace checkoutdotcom.Controllers
{
    public class DrinkOrder : DrinkOrderBase
    {
        [Required]
        public string Name { get; set; }
    }
}