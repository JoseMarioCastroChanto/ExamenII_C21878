
using backend.Infrastructure;
using backend.Domain;

namespace backend.Application
{
    public class CoffeeQuery
    {
        private readonly CoffeeHandler _coffeeHandler;

        public CoffeeQuery(CoffeeHandler coffeeHandler)
        {
            _coffeeHandler = coffeeHandler;
        }
        public List<CoffeeModel> GetCoffees()
        { 
            return _coffeeHandler.GetCoffees();
        }
        public List<(int CoffeeId, int Available)> CheckCoffeeExistence(int [] coffeeIds)
        {
            return _coffeeHandler.CoffeeExistence(coffeeIds);
        }
    }
}