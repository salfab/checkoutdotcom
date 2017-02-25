using System;
using System.Linq;

namespace checkoutdotcom.Controllers
{
    using System.Collections.Generic;

    using Microsoft.AspNetCore.Mvc;

    [Route("api/shopping-list")]
    public class ShoppingListController : Controller
    {
        private readonly IDrinksCountTrackingService drinksCountTrackingService;

        public ShoppingListController(IDrinksCountTrackingService drinksCountTrackingService)
        {
            this.drinksCountTrackingService = drinksCountTrackingService;
        }

        /// <summary>
        /// Adds a number of drinks to the shopping-list.
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        ///   The specs ask us to add drinks. Drinks of this type may already exist in the shopping list.
        ///   In that case, its quantity will be incremented according, therefore, the endpoint can't follow the RESTful pattern, since it doesn't always lead to the creation of a new resource. 
        ///   "adding a drink" is therefore an action (RPC) on the shopping list and can't be represented as a located resource on which different actions (verbs) can be performed.
        ///   If we wanted to make it RESTful, we would need to expose the endpoint via the PUT or PATCH verb. The consumer would then have the burden of incrementing the counters, 
        ///   and of calling the correct (POST / PATCH) endpoint. In order to reduce the complexity, and avoid concurrency issues, we will prefer an RPC approach over a RESTful service.
        /// </remarks>
        [Route("add-drink")]
        [HttpPost]
        public void AddDrink([FromBody]DrinkOrder drinkOrder)
        {
            if (drinkOrder == null)
            {
                throw new ArgumentNullException(nameof(drinkOrder));
            }

            this.drinksCountTrackingService.AddDrink(drinkOrder.Name, drinkOrder.Quantity.Value);
        }
    }
}