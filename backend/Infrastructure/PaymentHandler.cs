﻿using backend.Domain;

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
                { 500, 20 }, 
                { 100, 30 }, 
                { 50, 50 },  
                { 25, 25 }  
            };
        }

        public bool OutOfChange()
        {
            bool allZeroExcept1000 = moneyInventory.Where(kvp => kvp.Key != 1000)
                                       .All(kvp => kvp.Value == 0);
            return allZeroExcept1000;
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
