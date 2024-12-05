namespace backend.Domain
{
    public class PaymentModel
    {
        public int[] PaymentWay { get; set; }

        public int [] CoffeeId { get; set; }
        public int[] CoffeeQuantity { get; set; }

        public int CashChange { get; set; }
    }
}
