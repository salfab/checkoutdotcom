namespace checkoutdotcom.Controllers
{
    using System.Collections.Generic;

    public interface IDrinksPersistenceService
    {
        IEnumerable<string> Get();

        void AddDrink(string name, int quantity);
    }
}