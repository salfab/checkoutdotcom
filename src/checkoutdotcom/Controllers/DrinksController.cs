using System;
using System.Collections.Generic;
using System.Linq;

using checkoutdotcom.Exceptions;

using Microsoft.AspNetCore.Authorization;
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
        [Authorize(Policy = "ValidApiKey")]  
        public IEnumerable<DrinkOrder> Get()
        {
            return this.drinksCountTrackingService.Get().Select(pair => new DrinkOrder { Name = pair.Key, Quantity = pair.Value });
        }

        [HttpGet]
        [Route("{drinkName}")]
        [Authorize(Policy = "ValidApiKey")]  
        public DrinkOrder Get(string drinkName)
        {
            if (drinkName == null)
            {
                throw new ArgumentNullException(nameof(drinkName));
            }

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
        [Authorize(Policy = "ValidApiKey")]  
        public void Delete(string drinkName)
        {
            if (drinkName == null)
            {
                throw new ArgumentNullException(nameof(drinkName));
            }

            var success = this.drinksCountTrackingService.TryDelete(drinkName);
            if (!success)
            {
                throw new ResourceNotFoundException("couldn't find drink " + drinkName);
            }            
        }


        [HttpPut]
        [Route("{drinkName}")]
        [Authorize(Policy = "ValidApiKey")]  
        public DrinkOrder UpdateDrink(string drinkName, [FromBody]DrinkOrderBase drinkOrder)
        {
            if (drinkName == null)
            {
                throw new ArgumentNullException(nameof(drinkName));
            }
            if (drinkOrder == null)
            {
                throw new ArgumentNullException(nameof(drinkOrder));
            }

            var successful  = this.drinksCountTrackingService.TryUpdate(drinkName, drinkOrder.Quantity.Value);
            if (!successful)
            {
                throw new ResourceNotFoundException("couldn't find drink " + drinkName);
            }

            return new DrinkOrder
                       {
                            Name = drinkName,
                            Quantity = this.drinksCountTrackingService.Get(drinkName)
                       };
        }
    }
}