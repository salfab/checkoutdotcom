using System.Collections.Generic;
using System.Linq;

using checkoutdotcom.Exceptions;

using Microsoft.AspNetCore.Mvc;

namespace checkoutdotcom.Controllers
{
    [Route("api/shopping-list/drinks")]
    public class DrinksController : Controller
    {
        private readonly IDrinksCountTrackingService drinksCountTrackingService;

        public DrinksController(IDrinksCountTrackingService drinksCountTrackingService)
        {
            this.drinksCountTrackingService = drinksCountTrackingService;
        }

        [HttpGet]        
        public IEnumerable<DrinkOrder> Get()
        {
            return this.drinksCountTrackingService.Get().Select(pair => new DrinkOrder { Name = pair.Key, Quantity = pair.Value });
        }

        [HttpGet]
        [Route("{drinkName}")]
        public DrinkOrder Get(string drinkName)
        {
            var count = this.drinksCountTrackingService.Get(drinkName);

            if (count == 0)
            {
                throw new ResourceNotFoundException("couldn't find drink " + drinkName);
            }

            var drinkOrder = new DrinkOrder { Name = drinkName, Quantity = count};

            return drinkOrder;
        }


        [HttpDelete]
        [Route("{drinkName}")]
        public void Delete(string drinkName)
        {
            var success = this.drinksCountTrackingService.TryDelete(drinkName);
            if (!success)
            {
                throw new ResourceNotFoundException("couldn't find drink " + drinkName);
            }            
        }


        [HttpPut]
        [Route("{drinkName}")]
        public void UpdateDrink(string drinkName, [FromBody]DrinkOrderBase drinkOrder)
        {
            var successful  = this.drinksCountTrackingService.TryUpdate(drinkName, drinkOrder.Quantity.Value);
            if (!successful)
            {
                throw new ResourceNotFoundException("couldn't find drink " + drinkName);
            }            
        }
    }
}