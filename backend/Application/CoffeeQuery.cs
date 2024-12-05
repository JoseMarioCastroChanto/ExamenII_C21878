
using backend.Infrastructure;
using backend.Domain;

namespace backend.Application
{
    public interface ICoffeeQuery
    {
        List<CoffeeModel> GetCoffees();
        List<(int CoffeeId, int Available)> CheckCoffeeExistence(int[] coffeeIds);
    }
    public class CoffeeQuery: ICoffeeQuery
    {
        private readonly ICoffeeHandler _coffeeHandler;

        public CoffeeQuery(ICoffeeHandler coffeeHandler)
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