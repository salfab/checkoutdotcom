using System.Collections.Generic;
using System.Linq;

using Microsoft.AspNetCore.Mvc;

namespace checkoutdotcom.Controllers
{
    public class DrinksController : Controller
    {
        private readonly IDrinksPersistenceService drinksPersistenceService;

        public DrinksController(IDrinksPersistenceService drinksPersistenceService)
        {
            this.drinksPersistenceService = drinksPersistenceService;
        }

        [HttpGet]
        [Route("api/shopping-list/drinks/{drinkName}")]
        public IActionResult Get(string drinkName)
        {
            var count = this.drinksPersistenceService.Get(drinkName);
            if (count == 0)
            {
                return this.NotFound();
            }
            return Ok(new DrinkOrder { Name = drinkName, Quantity = count});
        }
    }
}