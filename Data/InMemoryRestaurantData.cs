using Core;
using System.Collections.Generic;
using System.Linq;

namespace Data
{
    public class InMemoryRestaurantData : IRestaurantData
    {
        private readonly List<Restaurant> restaurants;
        public InMemoryRestaurantData()
        {
            restaurants = new List<Restaurant>
            {
                new Restaurant { Id = 1, Name = "Sean's Authentic Italian Cuisine", Cuisine= CuisineType.Italian, Location = "Philadelphia"},
                new Restaurant { Id = 2, Name = "Bring the Pepto", Cuisine = CuisineType.Mexican, Location = "Philadelphia"}
            };
        }
        public IEnumerable<Restaurant> GetAll()
        {
            return from r in restaurants
                   orderby r.Name
                   select r;
        }
    }
}

