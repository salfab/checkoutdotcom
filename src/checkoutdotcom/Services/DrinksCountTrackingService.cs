using System;
using System.Linq;
using System.Collections.Generic;

namespace checkoutdotcom.Controllers
{

    /// <summary>
    /// This is the service responsible for keeping track of the count of drinks ordered.
    /// </summary>
    /// <remarks>
    /// According to the specs, the only thing we will have to keep track of is the number of bottles ordered for a given drink.
    /// It may seem odd that we have signatures with just drink names and quantities at first glance.
    /// We could have used a repository pattern, and expose signatures that would accept <see cref="DrinkOrder"/> objects, 
    /// but at the moment, we don't need it so we can stick to the KISS principle, because YAGNI.
    /// Anyways, a refactor will be very quick to perform, shold we change our mind.
    /// </remarks>
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