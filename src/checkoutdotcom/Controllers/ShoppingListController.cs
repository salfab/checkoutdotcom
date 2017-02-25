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

        [HttpGet]
        public IEnumerable<string> Get()
        {
            return this.drinksPersistenceService.Get();
        }
    }
}