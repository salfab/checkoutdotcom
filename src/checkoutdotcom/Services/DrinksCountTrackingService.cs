using System;
using System.Linq;

namespace checkoutdotcom.Controllers
{
    using System.Collections.Generic;

    public class DrinksCountTrackingService : IDrinksCountTrackingService
    {
        private readonly Dictionary<string, int> drinks;


        public DrinksCountTrackingService()
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
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }

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
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            return this.drinks.ContainsKey(name) ? this.drinks[name] : 0;
        }

        public bool TryDelete(string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            return this.drinks.Remove(name);
        }

        public bool TryUpdate(string name, int quantity)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            if (this.drinks.ContainsKey(name))
            {
                this.drinks[name] = quantity;
                return true;
            }
            return false;
        }
    }
}