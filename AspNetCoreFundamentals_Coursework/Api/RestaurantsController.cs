using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core;
using Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreFundamentals_Coursework.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class RestaurantsController : ControllerBase
    {
        private readonly IRestaurantData data;

        public RestaurantsController(IRestaurantData data)
        {
            this.data = data;
        }
        // GET: api/Restaurants
        [HttpGet]
        public IEnumerable<Restaurant> Get()
        {
            var restaurants = data.GetByName();
            return restaurants ?? new List<Restaurant>();
        }

        // GET: api/Restaurants/5
        [HttpGet("{id}", Name = "Get")]
        public IActionResult Get(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var restaurant = data.GetById(id);
            if(restaurant == null)
            {
                return NotFound();
            }
            return Ok(restaurant);
        }

        // POST: api/Restaurants
        [HttpPost]
        public IActionResult Post([FromBody] Restaurant restaurant)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            data.Create(restaurant);
            data.Commit();
            return CreatedAtAction("GetRestaurant", new { id = restaurant.Id }, restaurant);
        }

        // PUT: api/Restaurants/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Restaurant restaurant)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if(id != restaurant.Id)
            {
                return BadRequest();
            }
            try
            {
                data.Update(restaurant);
                data.Commit();
            }
            catch (Exception)
            {
                if (!RestaurantExists(id))
                {
                    return NotFound();
                }
                throw;
            }
            return NoContent();
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var restaurant = data.GetById(id);
            if(restaurant == null)
            {
                return NotFound();
            }
            data.Delete(id);
            return NoContent();
        }

        private bool RestaurantExists(int id)
        {
            return data.GetById(id) != null;
        }
    }
}
