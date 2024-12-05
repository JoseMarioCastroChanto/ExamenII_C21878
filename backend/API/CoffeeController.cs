using Microsoft.AspNetCore.Mvc;
using backend.Application;
using backend.Domain;



namespace backend.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoffeeController : ControllerBase
    {
        private  ICoffeeQuery _query;

        public CoffeeController(ICoffeeQuery query)
        {
            _query = query;
        }

        [HttpGet]
        public List<CoffeeModel> GetCoffees()
        {
            var coffees = _query.GetCoffees();
            return coffees;
        }

    }
}
