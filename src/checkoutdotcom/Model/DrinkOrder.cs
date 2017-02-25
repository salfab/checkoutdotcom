using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace checkoutdotcom.Controllers
{
    public class DrinkOrder : DrinkOrderBase
    {
        [Required]
        public string Name { get; set; }
    }
}