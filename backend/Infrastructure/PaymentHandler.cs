using backend.Domain;

namespace backend.Infrastructure
{
    public class PaymentHandler
    {
        private Dictionary<int, int> moneyInventory;

        public PaymentHandler()
        {
            moneyInventory = new Dictionary<int, int>
            {   
                { 1000, 0 },
                { 500, 20 }, 
                { 100, 30 }, 
                { 50, 50 },  
                { 25, 25 }  
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
            SubstractCashChanegFromFounds(change);
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

        private void SubstractCashChanegFromFounds(CashChangeModel change)
            {
                foreach (var key in change.CashChange)
            {
                int denomination = key.MoneyValue;
                int quantity = key.Quantity; 

                if (moneyInventory.ContainsKey(denomination))
                {
                    moneyInventory[denomination] -= quantity; 
                }
            }
        }
    }
}
