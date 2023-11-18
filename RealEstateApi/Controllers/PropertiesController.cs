using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using RealEstateApi.Data;
using RealEstateApi.Models;
using System.Security.Claims;

namespace RealEstateApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PropertiesController : ControllerBase
    {
        ApiDbContext _dbContext = new ApiDbContext();

        [HttpGet("PropertyList")]
        [Authorize]
        public IActionResult GetProperties(int categoryId)
        {
            var propertiesResullt = _dbContext.Properties.Where(c=>c.CategoryId== categoryId);

            if(propertiesResullt == null) return NotFound();    

            return Ok(propertiesResullt);
        }

        [HttpGet("PropertyDetail")]
        [Authorize]
        public IActionResult GetPropertyDetail(int id)
        {
            var propertiesResullt = _dbContext.Properties.FirstOrDefault(c => c.Id == id);

            if (propertiesResullt == null) return NotFound();

            return Ok(propertiesResullt);
        }

        [HttpGet("TrendingProperties")]
        [Authorize]
        public IActionResult GetTrendingProperties(int id)
        {
            var propertiesResullt = _dbContext.Properties.Where(c => c.IsTrending == true);

            if (propertiesResullt == null) return NotFound();

            return Ok(propertiesResullt);
        }

        [HttpGet("SearchProperties")]
        [Authorize]
        public IActionResult GetSearchProperties(string address)
        {
            var propertiesResullt = _dbContext.Properties.Where(c => c.Address.Contains(address));

            if (propertiesResullt == null) return NotFound();

            return Ok(propertiesResullt);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Post([FromBody] Models.Property property)
        {
            if(property == null)
            {
                return NoContent();
            }
            else
            {

                var userEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
                var user = _dbContext.Users.FirstOrDefault(u=>u.Email==userEmail);

                if (user == null) return NotFound();
                property.IsTrending = false;
                property.UserId = user.Id; 
                _dbContext.Properties.Add(property);
                _dbContext.SaveChanges();
                return StatusCode(StatusCodes.Status201Created);

            }
        }

        [HttpPut("{id}")]
        [Authorize]
        public IActionResult Put(int id, [FromBody] Models.Property property)
        {
            var propertyResult = _dbContext.Properties.FirstOrDefault(p => p.Id == id);
            if (propertyResult == null)
            {
                return NotFound();
            }
            else
            {

                var userEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
                var user = _dbContext.Users.FirstOrDefault(u => u.Email == userEmail);

                if (user == null) return NotFound();
                if(propertyResult.UserId == user.Id)
                {

                    propertyResult.Name = property.Name;
                    propertyResult.Detail = property.Detail;
                    propertyResult.Price = property.Price;
                    propertyResult.Address = property.Address;
                    propertyResult.IsTrending = property.IsTrending;
                    property.UserId = user.Id;

                    _dbContext.SaveChanges();
                    return Ok("Record updated successfully");

                }

                return BadRequest(); 

            }
        }


        [HttpDelete("{id}")]
        [Authorize]
        public IActionResult Put(int id)
        {
            var propertyResult = _dbContext.Properties.FirstOrDefault(p => p.Id == id);
            if (propertyResult == null)
            {
                return NotFound();
            }
            else
            {

                var userEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
                var user = _dbContext.Users.FirstOrDefault(u => u.Email == userEmail);

                if (user == null) return NotFound();
                if (propertyResult.UserId == user.Id)
                {
                    _dbContext.Remove(propertyResult);
                    _dbContext.SaveChanges();
                    return Ok("Record removed successfully");

                }

                return BadRequest();

            }
        }
    }
}
