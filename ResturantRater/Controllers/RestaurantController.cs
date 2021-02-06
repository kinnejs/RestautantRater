using ResturantRater.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace ResturantRater.Controllers
{
    public class RestaurantController : ApiController
    {
        private readonly RestaurantDbContext _context = new RestaurantDbContext();

        //Post (Create)
        //api/Restaurant
        [HttpPost]
        public async Task<IHttpActionResult> PostRestaurant([FromBody]Restaurant model)
        {

            if (model is null)
            {
                return BadRequest("Your request body cannot be empty");
            }
            if(ModelState.IsValid)
            {
                //Store the model in datebase
                _context.Restaurants.Add(model);
                int changeCount = await _context.SaveChangesAsync();

                return Ok("Your restaurant was created!");
            }

            //the model is not valid
            return BadRequest(ModelState);
        }

        //Get All
        //api/Restaurant
        [HttpGet]
        public async Task<IHttpActionResult> GetAll()
        {
            List<Restaurant> restaurants = await _context.Restaurants.ToListAsync();
            return Ok(restaurants);
        }

        //Get by ID
        //api/Restaurant/(id)
        [HttpGet]
        public async Task<IHttpActionResult> GetById([FromUri] int id)
        {
            Restaurant restaurant = await _context.Restaurants.FindAsync(id);

            if(restaurant != null)
            {
                return Ok(restaurant);
            }
            return NotFound();
        }

        //Put (Update)
        //api/Restaurant/(id)
        [HttpPut]
        public async Task<IHttpActionResult> UpdateRestaurant([FromUri] int id, [FromBody] Restaurant updatedRestaurant)
        {
            //Check the ids if the match
            if(id != updatedRestaurant?.Id)
            {
                return BadRequest("Ids do not match.");
            }
            //Check model state
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            //Find restaurant in database
            Restaurant restaurant = await _context.Restaurants.FindAsync(id);

            //if it doesnt match the do something
            if (restaurant is null)
                return NotFound();
            //Update the properties
            restaurant.Name = updatedRestaurant.Name;
            restaurant.Address = updatedRestaurant.Address;
            

            //Save changes
            await _context.SaveChangesAsync();

            return Ok("The restaurant was updated!");
        }

        //Delete
        //api.Restuarant/{id}
        [HttpDelete]
        public async Task<IHttpActionResult> DeleteRestaurant([FromUri] int id)
        {
            Restaurant restaurant = await _context.Restaurants.FindAsync(id);

            if (restaurant is null)
                return NotFound();

            _context.Restaurants.Remove(restaurant);

            if (await _context.SaveChangesAsync() == 1)
            {
                return Ok("The resturant was deleted.");
            }
            return InternalServerError();

        }
    }
}
