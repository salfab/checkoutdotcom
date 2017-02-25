namespace checkoutdotcom.Controllers
{
    using System.Collections.Generic;

    public interface IDrinksPersistenceService
    {
        IEnumerable<string> Get();
    }
}