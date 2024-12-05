using backend.Domain;

namespace backend.Infrastructure
{
    public interface IPaymentHandler
    {
        bool OutOfChange();
        Dictionary<int, int> GetExistingChange();
        void UpdateChangeExistence(CashChangeModel change, int[] PaymentWay);
    }
    public class PaymentHandler: IPaymentHandler
    {
        private Dictionary<int, int> moneyInventory;

        public PaymentHandler()
        {
            moneyInventory = new Dictionary<int, int>
            {   
                { 1000, 0 },
                { 500, 0 }, 
                { 100, 0 }, 
                { 50, 1 },  
                { 25, 0 }  
            };
        }

        public bool OutOfChange()
        {
            return moneyInventory.Values.All(value => value == 0);
        }

        public Dictionary<int, int> GetExistingChange()
        {
            return moneyInventory;
        }

        public void UpdateChangeExistence(CashChangeModel change, int[] PaymentWay)
        {
            AddPaymentToFounds(PaymentWay);
            SubstractCashChangeFromFunds(change);
        }
        private void AddPaymentToFounds( int[] PaymentWay)
        {
            var denominations = new[] { 1000, 500, 100, 50, 25 };
            for (int i = 0; i < denominations.Length; i++)
            {
                int denomination = denominations[i];
                int quantity = PaymentWay[i];

                if (moneyInventory.ContainsKey(denomination))
                {
                    moneyInventory[denomination] += quantity; 
                }
            }
        }

        private void SubstractCashChangeFromFunds(CashChangeModel change)
        {
            for (int i = 0; i < change.MoneyValue.Length; i++)
            {
                int denomination = change.MoneyValue[i];
                int quantity = change.Quantity[i];

                if (quantity == 0) continue;

                if (moneyInventory.ContainsKey(denomination))
                {
                    moneyInventory[denomination] -= quantity;
                }
            }
        }
    }
}
