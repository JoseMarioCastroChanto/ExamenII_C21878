using backend.Domain;
using backend.Infrastructure;

namespace backend.Application
{
    public interface IPaymentCommand
    {
        CashChangeModel PaymentTransaction(PaymentModel payment);
        CashChangeModel CalculateChange(int changeRequired, Dictionary<int, int> existingChange, int[] paymentWay);
    }

    public class PaymentCommand: IPaymentCommand
    {
        private readonly IPaymentQuery _paymentQuery;
        private readonly ICoffeeCommand _coffeeCommand;
        private readonly IPaymentHandler _paymentHandler;
        const string outOfStock = "Error: No hay suficiente stock de café para completar la orden.";
        const string outOfChange = "Fallo al realizar la compra.";

        public PaymentCommand(IPaymentQuery paymentQuery, ICoffeeCommand coffeeCommand, IPaymentHandler paymentHandler)
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


            if (!change.IsSuccessful)
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

            var change = new CashChangeModel
            {
                MoneyValue = new int[denominations.Length], 
                Quantity = new int[denominations.Length],   
                IsSuccessful = false
            };

            if (changeRequired == 0)
            {
                change.IsSuccessful = true;
                return change;
            }

            int totalAvailableChange = CalculateTotalAvailableChange(existingChange, paymentWay, denominations);

            if (totalAvailableChange < changeRequired)
            {
                return new CashChangeModel(); 
            }

            int index = 0;
            changeRequired = AddChangeFromExistingChange(ref changeRequired, existingChange, change, ref index);

            AddChangeFromPaymentWay(ref changeRequired, paymentWay, denominations, change, ref index);
            if (changeRequired > 0)
            {
                return new CashChangeModel();
            }
            change.IsSuccessful = true;
            return change;
        }

        private int CalculateTotalAvailableChange(Dictionary<int, int> existingChange, int[] paymentWay, int[] denominations)
        {
            return existingChange.Sum(c => c.Key * c.Value) +
                   paymentWay.Select((quantity, i) => quantity * denominations[i]).Sum();
        }

        private int AddChangeFromExistingChange(ref int changeRequired, Dictionary<int, int> existingChange, CashChangeModel change, ref int index)
        {
            foreach (var money in existingChange.OrderByDescending(c => c.Key))
            {
                if (changeRequired <= 0) break;

                int moneyValue = money.Key;
                int availableQuantity = money.Value;
                int moneyCount = Math.Min(changeRequired / moneyValue, availableQuantity);

                if (moneyCount > 0)
                {
                    if (index >= change.MoneyValue.Length) break;  

                    change.MoneyValue[index] = moneyValue;
                    change.Quantity[index] = moneyCount;
                    index++;
                    changeRequired -= moneyCount * moneyValue;
                }
            }

            return changeRequired;
        }

        private void AddChangeFromPaymentWay(ref int changeRequired, int[] paymentWay, int[] denominations, CashChangeModel change, ref int index)
        {
            for (int i = 0; i < paymentWay.Length; i++)
            {
                if (changeRequired <= 0) break;

                int moneyValue = denominations[i];
                int availableQuantity = paymentWay[i];
                int moneyCount = Math.Min(changeRequired / moneyValue, availableQuantity);

                if (moneyCount > 0)
                {
                    if (index >= change.MoneyValue.Length) break;  

                    change.MoneyValue[index] = moneyValue;
                    change.Quantity[index] = moneyCount;
                    index++;
                    changeRequired -= moneyCount * moneyValue;
                }
            }
        }
    }
}