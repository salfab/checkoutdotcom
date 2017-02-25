using System.Linq;

namespace checkoutdotcom.Controllers
{
    using System.Collections.Generic;

    using Microsoft.AspNetCore.Mvc;

    [Route("api/shopping-list")]
    public class ShoppingListController : Controller
    {
        private readonly IDrinksPersistenceService drinksPersistenceService;

        public ShoppingListController(IDrinksPersistenceService drinksPersistenceService)
        {
            this.drinksPersistenceService = drinksPersistenceService;
        }

        /// <summary>
        /// Adds a number of drinks to the shopping-list.
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        ///   The specs ask us to add drinks here.
        ///   This is not the creation of a new resource. Drinks of this type may already exist. 
        ///   We are not creating a new resource and therefore, we can't have a truly REST endpoint, since "add" is an action and not the location of a resource.
        ///   If we wanted to make it RESTful, we would need to expose this endpoint as a PUT or PATCH endpoint to allow the consumer to edit the "shopping-list" resource.
        ///   Instead, we'll use a non-REST action-oriented endpoint.
        /// </remarks>
        [Route("add-drink")]
        [HttpPost]
        public void AddDrink([FromBody]DrinkOrder drinkOrder)
        {
            this.drinksPersistenceService.AddDrink(drinkOrder.Name, drinkOrder.Quantity);
        }
    }
}