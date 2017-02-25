using System.Collections.Generic;
using System.Linq;

using Microsoft.AspNetCore.Mvc;

namespace checkoutdotcom.Controllers
{
    [Route("api/shopping-list/drinks")]
    public class DrinksController : Controller
    {
        private readonly IDrinksPersistenceService drinksPersistenceService;

        public DrinksController(IDrinksPersistenceService drinksPersistenceService)
        {
            this.drinksPersistenceService = drinksPersistenceService;
        }

        [HttpGet]        
        public IEnumerable<DrinkOrder> Get()
        {
            return this.drinksPersistenceService.Get().Select(pair => new DrinkOrder { Name = pair.Key, Quantity = pair.Value });
        }


        [HttpGet]
        [Route("{drinkName}")]
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


        [HttpDelete]
        [Route("{drinkName}")]
        public IActionResult Delete(string drinkName)
        {
            var success = this.drinksPersistenceService.Delete(drinkName);
            if (!success)
            {
                return this.NotFound();
            }

            return this.Ok();
        }

        [HttpPut]
        public IActionResult UpdateDrink([FromBody]DrinkOrder drinkOrder)
        {
            var successful  = this.drinksPersistenceService.Update(drinkOrder.Name, drinkOrder.Quantity);
            if (!successful)
            {
                return this.NotFound();
            }
            return this.Ok();
        }
    }
}