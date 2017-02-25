namespace checkoutdotcom.Controllers
{
    using System.Collections.Generic;

    public interface IDrinksPersistenceService
    {
        Dictionary<string, int> Get();

        void AddDrink(string name, int quantity);
        int Get(string name);

        bool Delete(string name);
    }
}