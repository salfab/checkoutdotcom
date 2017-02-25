namespace checkoutdotcom.Controllers
{
    using System.Collections.Generic;

    internal interface IDrinksPersistenceService
    {
        IEnumerable<string> Get();
    }
}