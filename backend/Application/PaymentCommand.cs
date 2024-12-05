using backend.Domain;
using backend.Infrastructure;

namespace backend.Application
{
    public class PaymentCommand
    {
        private readonly PaymentQuery _paymentQuery;
        private readonly CoffeeCommand _coffeeCommand;
        private readonly PaymentHandler _paymentHandler;
        const string outOfStock = "Error: No hay suficiente stock de café para completar la orden.";
        const string outOfChange = "Error: No se puede dar el vuelto con el cambio disponible.";

        public PaymentCommand(PaymentQuery paymentQuery, CoffeeCommand coffeeCommand, PaymentHandler paymentHandler)
        {
            _paymentQuery = paymentQuery;
            _coffeeCommand = coffeeCommand;
            _paymentHandler = paymentHandler;
        }

        public CashChangeModel PaymentTransaction(PaymentModel payment)
        {
            var existingChange = _paymentQuery.GetExistingChange();


            int cashChangeRequired = payment.CashChange;
            var change = CalculateChange(cashChangeRequired, existingChange, payment.PaymentWay);


            if (!change.CashChange.Any())
            {
                throw new InvalidOperationException(outOfChange);
            }
            bool isUpdated = _coffeeCommand.UpdateCoffeeAvailability(payment.CoffeeId, payment.CoffeeQuantity);
            if (!isUpdated)
            {
                throw new InvalidOperationException(outOfStock);
            }
            _paymentHandler.UpdateChangeExistence(change, payment.PaymentWay);

            return change;
        }

        public CashChangeModel CalculateChange(int changeRequired, Dictionary<int, int> existingChange, int[] paymentWay)
        {
            int[] denominations = { 1000, 500, 100, 50, 25 };
            var change = new CashChangeModel();
            change.CashChange = new List<(int MoneyValue, int Quantity)>();
            int totalAvailableChange = existingChange.Sum(c => c.Key * c.Value) +
                                        paymentWay[0] * denominations[0] +
                                        paymentWay[1] * denominations[1] +
                                        paymentWay[2] * denominations[2] +
                                        paymentWay[3] * denominations[3] +
                                        paymentWay[4] * denominations[4];

            if (totalAvailableChange < changeRequired)
            {
                return change; 
            }

            foreach (var money in existingChange.OrderByDescending(c => c.Key))
            {
                int moneyValue = money.Key;
                int availableQuantity = money.Value;

                if (changeRequired <= 0) break;

                int moneyCount = Math.Min(changeRequired / moneyValue, availableQuantity);
                changeRequired -= moneyCount * moneyValue;

                if (moneyCount > 0)
                {
                    change.CashChange.Add((moneyValue, moneyCount));
                }
            }

            for (int i = 0; i < paymentWay.Length; i++)
            {
                if (changeRequired <= 0) break;

                int moneyValue = denominations[i];
                int availableQuantity = paymentWay[i];

                int moneyCount = Math.Min(changeRequired / moneyValue, availableQuantity);
                changeRequired -= moneyCount * moneyValue;

                if (moneyCount > 0)
                {
                    change.CashChange.Add((moneyValue, moneyCount));
                }
            }
            if (changeRequired > 0)
            {
                return new CashChangeModel();
   
            } 
            return change;
        }
    }
}