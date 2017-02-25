namespace checkoutdotcom.Controllers
{
    using System.Collections.Generic;

    /// <summary>
    /// The contract for the service responsible for tracking the drinks count.
    /// </summary>
    public interface IDrinksCountTrackingService
    {
        Dictionary<string, int> Get();

        void AddDrink(string name, int quantity);
        int Get(string name);

        bool Delete(string name);

        bool Update(string name, int quantity);
    }
}