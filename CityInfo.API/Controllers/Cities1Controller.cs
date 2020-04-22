using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CityInfo.API.Entities;
using CityInfo.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Cities1Controller : ControllerBase
    {
        private readonly ICityInfoRepository _cityInfoRepository;

        public Cities1Controller(ICityInfoRepository _cityInfoRepository)
        {
            this._cityInfoRepository = _cityInfoRepository;
        }

        // GET: api/Cities1
        [HttpGet]
        public IActionResult GetCities(bool includePointsOfInterest = false,
                                       bool includeLanguages = false)
        {
            var cityEntities = _cityInfoRepository.GetCitiesAdvanced(includePointsOfInterest,
                                                                     includeLanguages);

            return Ok(cityEntities);  
        }

        // GET: api/Cities1/5
        [HttpGet("{id}")]
        public IActionResult GetCity(int id, 
                                     bool includePointsOfInterest = false,
                                     bool includeLanguages = false)
        {
            var city = _cityInfoRepository.GetCityAdvanced(id, 
                                                           includePointsOfInterest,
                                                           includeLanguages);

            if (city == null)
            {
                return NotFound();
            }

            return Ok(city);
        }

        // POST: api/Cities1
        [HttpPost]
        public IActionResult CreatePointOfInterest([FromBody] City city)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _cityInfoRepository.AddCity(city);

            _cityInfoRepository.Save();

            return Ok(city.Id);
        }

        // PUT: api/Cities1/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
