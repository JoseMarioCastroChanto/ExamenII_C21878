using backend.Application;
using backend.Infrastructure;

namespace backend.Application
{
    public interface ICoffeeCommand
    {
        bool UpdateCoffeeAvailability(int[] CoffeeId, int[] Quantity);
    }
    public class CoffeeCommand : ICoffeeCommand
    {
        private ICoffeeQuery _coffeeQuery;
        private ICoffeeHandler _coffeeHandler;
        const string validParametersMessage = "Los identificadores de café y las cantidades deben ser válidos.";
        const string enoughStockMessage = "No hay suficiente stock de café para la cantidad solicitada.";

        public CoffeeCommand(ICoffeeQuery coffeeQuery, ICoffeeHandler coffeeHandler)
        {
            _coffeeQuery = coffeeQuery;
            _coffeeHandler = coffeeHandler;
        }

        public bool UpdateCoffeeAvailability(int[] CoffeeId, int[] Quantity)
        {
            if (!CheckValidCoffeeIds(CoffeeId) || !CheckValidCoffeeQuantities(Quantity))
            {
                throw new ArgumentException(validParametersMessage);
            }

            var existingCoffees = _coffeeQuery.CheckCoffeeExistence(CoffeeId);

            if (!CheckStock(existingCoffees, Quantity))
            {
                throw new InvalidOperationException(enoughStockMessage);
            }


            _coffeeHandler.UpdateCoffeeAvailability(CoffeeId, Quantity);

            return true;
        }


        private bool CheckValidCoffeeIds(int[] CoffeeId)
        {
            return CoffeeId != null && CoffeeId.All(id => id > 0);
        }


        private bool CheckValidCoffeeQuantities(int[] Quantity)
        {
            return Quantity != null && Quantity.All(q => q > 0);
        }


        private bool CheckStock(List<(int CoffeeId, int Available)> Available, int[] Quantity)
        {
            for (int i = 0; i < Available.Count; i++)
            {
                if (Available[i].Available < Quantity[i])
                {
                    return false;
                }
            }
            return true;
        }
    }
}