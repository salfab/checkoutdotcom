namespace checkoutdotcom.Controllers
{
    public class DrinkOrder
    {
        public DrinkOrder(string name, int quantity)
        {
            this.Name = name;
            this.Quantity = quantity;
        }

        public string Name { get; private set; }

        public int Quantity { get; private set; }
    }
}