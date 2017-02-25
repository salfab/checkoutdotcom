using System.Collections.Generic;
using System.Linq;

using Microsoft.AspNetCore.Mvc;

namespace checkoutdotcom.Controllers
{
    [Route("api/shopping-list")]
    public class DrinksController : Controller
    {
        private readonly IDrinksPersistenceService drinksPersistenceService;

        public DrinksController(IDrinksPersistenceService drinksPersistenceService)
        {
            this.drinksPersistenceService = drinksPersistenceService;
        }

        [HttpGet]
        [Route("drinks")]
        public IEnumerable<DrinkOrder> Get()
        {
            return this.drinksPersistenceService.Get().Select(pair => new DrinkOrder { Name = pair.Key, Quantity = pair.Value });
        }


        [HttpGet]
        [Route("drinks/{drinkName}")]
        public IActionResult Get(string drinkName)
        {
            var count = this.drinksPersistenceService.Get(drinkName);

            if (count == 0)
            {
                return this.NotFound();
            }

            var drinkOrder = new DrinkOrder { Name = drinkName, Quantity = count};

            return this.Ok(drinkOrder);
        }
    }
}