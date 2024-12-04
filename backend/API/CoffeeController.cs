using Microsoft.AspNetCore.Mvc;
using backend.Application;
using backend.Domain;



namespace backend.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoffeeController : ControllerBase
    {
        private  CoffeeQuery _query;

        public CoffeeController(CoffeeQuery query)
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
