using System.Linq;

namespace checkoutdotcom.Controllers
{
    using System.Collections.Generic;

    public class DrinksPersistenceService : IDrinksPersistenceService
    {
        private readonly Dictionary<string, int> drinks;


        public DrinksPersistenceService()
        {
            this.drinks = new Dictionary<string, int>();
        }
        public Dictionary<string, int> Get()
        {
            // let's not return a reference to the actual this.drinks dictionary.
            return this.drinks.ToDictionary(pair => pair.Key, pair => pair.Value);
        }

        public void AddDrink(string name, int quantity)
        {
            if (this.drinks.ContainsKey(name))
            {
                this.drinks[name] += quantity;
            }
            else
            {                
               this.drinks.Add(name, quantity);
            }
        }

        public int Get(string name)
        {
            return this.drinks.ContainsKey(name) ? this.drinks[name] : 0;
        }

        public bool Delete(string name)
        {
            return this.drinks.Remove(name);
        }
    }
}