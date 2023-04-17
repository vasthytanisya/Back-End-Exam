using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GripFoodBackEnd.Entities;
using GripFoodBackEnd.Models;

namespace GripFoodBackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RestaurantsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public RestaurantsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Restaurants
        [HttpGet]
        public async Task<ActionResult<List<RestaurantDataGridItem>>> GetRestaurants()
        {
          if (_context.Restaurants == null)
          {
              return NotFound();
          }

          return await _context.Restaurants.AsNoTracking().Select(Q => new RestaurantDataGridItem
            {
                Id = Q.Id,
                Name = Q.Name,
            }).ToListAsync();
        }

        // GET: api/Restaurants/5
        [HttpGet("{id}", Name ="GetRestaurantDetail")]
        public async Task<ActionResult<FoodItemDataGridItem>> GetRestaurant(string id)
        {
            if (_context.Restaurants == null)
            {
                return NotFound();
            }

            var foodItem = await _context.FoodItems
                .AsNoTracking()
                .Where(Q => Q.Restaurant.Id == id)
                .Select(Q => new FoodItemDataGridItem
                {
                    Id = Q.Id,
                    Name = Q.Name,
                    Price = Q.Price
                })
                .FirstOrDefaultAsync();

            if (foodItem == null)
            {
                return NotFound();
            }

            return foodItem;
        }

        // PUT: api/Restaurants/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("{id}", Name ="UpdateRestaurant")]
        public async Task<IActionResult> Post(string id, Restaurant restaurant)
        {
            if (id != restaurant.Id)
            {
                return BadRequest();
            }

            _context.Entry(restaurant).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RestaurantExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Restaurants
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost(Name ="CreateRestaurant")]
        public async Task<ActionResult<Restaurant>> Post(RestaurantCreateModel restaurant)
        {
          if (_context.Restaurants == null)
          {
              return Problem("Entity set 'ApplicationDbContext.Restaurants'  is null.");
          }

            var insert = new Restaurant
            {
                Id = Ulid.NewUlid().ToString(),
                Name = restaurant.Name,
            };

            _context.Restaurants.Add(insert);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (RestaurantExists(insert.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return insert;
        }

        // DELETE: api/Restaurants/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRestaurant(string id)
        {
            if (_context.Restaurants == null)
            {
                return NotFound();
            }
            var restaurant = await _context.Restaurants.FindAsync(id);
            if (restaurant == null)
            {
                return NotFound();
            }

            _context.Restaurants.Remove(restaurant);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RestaurantExists(string id)
        {
            return (_context.Restaurants?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
