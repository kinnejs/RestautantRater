using ResturantRater.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace ResturantRater.Controllers
{
    public class RatingController : ApiController
    {

        private readonly RestaurantDbContext _context = new RestaurantDbContext();
        //Create a rating
        //api/rating
        [HttpPost]
        public async Task<IHttpActionResult> CreateRating([FromBody] Rating model)
        {
            //check if model is null
            if (model is null)
                return BadRequest("Your request body cannot be empty");
            //check is modelstate is invalid
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            //find restaurant by the model.RestaurantId and see if it exists
            var restaurantEntity = await _context.Restaurants.FindAsync(model.RestaurantId);
            if (restaurantEntity is null)
                return BadRequest($"The raget restaurant with the ID of {model.RestaurantId} does not exist");
            //create rating

            //Add to the rating table

            //add to the restaurant entity
            restaurantEntity.Ratings.Add(model);
            if (await _context.SaveChangesAsync() == 1)
                return Ok($"You rated restaurant {restaurantEntity.Name} successfully!");

            return InternalServerError();
        }

        //Get rating by Id

        //Get All ratings

        //Get all ratings for a specific restaurant by restaurant ID

        //Update a rating

        //Delete a rating
    }
}
