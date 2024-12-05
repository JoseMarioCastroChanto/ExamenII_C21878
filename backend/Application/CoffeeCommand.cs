using backend.Infrastructure;

namespace backend.Application
{
    public class CoffeeCommand
    {
        private CoffeeQuery _coffeeQuery;
        private CoffeeHandler _coffeeHandler;

        public CoffeeCommand(CoffeeQuery coffeeQuery, CoffeeHandler coffeeHandler)
        {
            _coffeeQuery = coffeeQuery;
            _coffeeHandler = coffeeHandler;
        }

        public bool UpdateCoffeeAvailability(int [] CoffeeId, int [] Quantity)
        {
 
            var existingCoffees = _coffeeQuery.CheckCoffeeExistence(CoffeeId);


            if (existingCoffees.Count != CoffeeId.Length)
            {
                return false;  
            }

                _coffeeHandler.UpdateCoffeeAvailability(CoffeeId, Quantity);
         

            return true;
        }
    }
}
