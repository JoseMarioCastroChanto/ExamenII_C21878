using Microsoft.AspNetCore.Mvc;
using backend.Application;
using backend.Domain;

namespace backend.Infrastructure
{
    public class CoffeeHandler
    {
        private List<CoffeeModel> _coffees = new List<CoffeeModel>
        {
            new CoffeeModel { Id = 1, Name = "Americano", Price = 950, Available = 10, Quantity = 0, Image = "https://cdn-icons-png.freepik.com/512/7618/7618001.png" },
            new CoffeeModel { Id = 2, Name = "Capuchino", Price = 1200, Available = 8, Quantity = 0, Image = "https://images.vexels.com/content/264248/preview/cappuccino-illustration-0442c5.png" },
            new CoffeeModel { Id = 3, Name = "Latte", Price = 1350, Available = 10, Quantity = 0, Image = "https://cdn-icons-png.flaticon.com/512/3127/3127450.png" },
            new CoffeeModel { Id = 4, Name = "Mocachino", Price = 1500, Available = 15, Quantity = 0, Image = "https://www.svgrepo.com/show/76483/coffee-cup.svg" }
        };

        public List<CoffeeModel> GetCoffees()
        {
            return _coffees;
        }

    }
}
