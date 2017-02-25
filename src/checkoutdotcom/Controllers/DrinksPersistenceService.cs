namespace checkoutdotcom.Controllers
{
    using System.Collections.Generic;

    public class DrinksPersistenceService : IDrinksPersistenceService
    {
        private IEnumerable<string> drinks;

        public IEnumerable<string> Get()
        {
            return this.drinks;
        }
    }
}