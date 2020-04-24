using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CityInfo.API.Entities;
using CityInfo.API.Models;
using CityInfo.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Cities1Controller : ControllerBase
    {
        private readonly ICityInfoRepository _cityInfoRepository;
        private readonly IMapper _mapper;
        private readonly IMailService _mailService;

        public Cities1Controller(ICityInfoRepository _cityInfoRepository,
                                 IMapper _mapper,
                                 IMailService _mailService)
        {
            this._cityInfoRepository = _cityInfoRepository;
            this._mapper = _mapper;
            this._mailService = _mailService;
        }

        // GET: api/Cities1
        [HttpGet]
        public IActionResult GetCities(bool includePointsOfInterest = false,
                                       bool includeLanguages = false)
        {
            var cityEntities = _cityInfoRepository.GetCitiesAdvanced(includePointsOfInterest,
                                                                     includeLanguages);

            //IEnumerable<CityDto> CITYDtos = _mapper.Map<IEnumerable<CityDto>>(cityEntities);
            //return Ok(_mapper.Map<IEnumerable<CityDto>>(cityEntities));


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
        public IActionResult CreateCity([FromBody] City city)
        {
            if (city.Description == city.Name)
            {
                ModelState.AddModelError(
                    "Description",
                    "The provided description should be different from the name.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _cityInfoRepository.AddCity(city);

            _cityInfoRepository.Save();

            return Ok(city.Id);
        }

        [HttpPatch("{id}")]
        public IActionResult PartiallyUpdateCity(int id,
            [FromBody] JsonPatchDocument<CityForUpdateDto> patchDoc)
        {
            // JsonPatchDocument virker på DTO og ikke på dendelige entity model !!!
           
            var CityEntity = _cityInfoRepository
                .GetCityAdvanced(id);

            if (CityEntity == null)
            {
                return NotFound();
            }

            var CityToPatch = _mapper
                .Map<CityForUpdateDto>(CityEntity);

            patchDoc.ApplyTo(CityToPatch, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (CityToPatch.Description == CityToPatch.Name)
            {
                ModelState.AddModelError(
                    "Description",
                    "The provided description should be different from the name.");
            }

            if (!TryValidateModel(CityToPatch))
            {
                return BadRequest(ModelState);
            }

            _mapper.Map(CityToPatch, CityEntity);

            _cityInfoRepository.UpdateCity(CityEntity);
            // LTPE
            // Linjen herover er egentlig ikke nødvendig med den nuværende implementation af
            // vores program, Da Enity Framework Core holder automatisk styr på vores 
            // ændringer. Men kode linjen er god at have med for en sikkerheds skyld.
            // Metoden UpdateCity er for nærværende bare en tom metode !!!

            _cityInfoRepository.Save();

            return NoContent();
        }

        // PUT: api/Cities1/5
        [HttpPut("{id}")]
        public IActionResult UpdateCity(int id,
            [FromBody] CityForUpdateDto city)
        {
            if (city.Description == city.Name)
            {
                ModelState.AddModelError(
                    "Description",
                    "The provided description should be different from the name.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!_cityInfoRepository.CityExists(id))
            {
                return NotFound();
            }

            var cityEntity = _cityInfoRepository
                .GetCityAdvanced(id);

            if (cityEntity == null)
            {
                return NotFound();
            }

            _mapper.Map(city, cityEntity);

            _cityInfoRepository.UpdateCity(cityEntity);
            // LTPE
            // Linjen herover er egentlig ikke nødvendig med den nuværende implementation af
            // vores program, Da Enity Framework Core holder automatisk styr på vores 
            // ændringer. Men kode linjen er god at have med for en sikkerheds skyld.
            // Metoden UpdateCity er for nærværende bare en tom metode !!!
            _cityInfoRepository.Save();

            return NoContent();
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public IActionResult DeleteCity(int id)
        {
            var cityEntity = _cityInfoRepository
                .GetCityAdvanced(id);

            if (cityEntity == null)
            {
                return NotFound();
            }

            _cityInfoRepository.DeleteCity(cityEntity);

            _cityInfoRepository.Save();

            _mailService.Send("City deleted.",
                    $"City {cityEntity.Name} with id {cityEntity.Id} was deleted.");

            return NoContent();
        }
    }
}
