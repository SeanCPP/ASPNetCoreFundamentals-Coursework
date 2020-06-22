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
        public IEnumerable<Restaurant> GetByName(string name = null)
        {
            return from r in restaurants
                   where string.IsNullOrEmpty(name) || r.Name.StartsWith(name,System.StringComparison.CurrentCultureIgnoreCase)
                   orderby r.Name
                   select r;
        }
        public Restaurant GetById(int id)
        {
            return restaurants.SingleOrDefault(r => r.Id == id);
        }

        public Restaurant Update(Restaurant itemToUpdate)
        {
            var restaurant = restaurants.SingleOrDefault(r => r.Id == itemToUpdate.Id);
            if(restaurant != null)
            {
                restaurant.Name = itemToUpdate.Name;
                restaurant.Location = itemToUpdate.Location;
                restaurant.Cuisine = itemToUpdate.Cuisine;
            }
            return restaurant;
        }
        public int Commit() 
        {
            return 0;
        }
    }
}

