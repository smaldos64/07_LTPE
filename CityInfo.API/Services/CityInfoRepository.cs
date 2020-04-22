using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CityInfo.API.Contexts;
using CityInfo.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace CityInfo.API.Services
{
    public class CityInfoRepository : ICityInfoRepository
    {
        private readonly CityInfoContext _context;

        public CityInfoRepository(CityInfoContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public IEnumerable<City> GetCities()
        {
            return _context.Cities.OrderBy(c => c.Name).ToList();
        }

        public IEnumerable<City> GetCitiesAdvanced(bool includePointsOfInterest = false,
            bool includeLanguages = false)
        {
            if ( (true == includePointsOfInterest) && (true == includeLanguages))
            {
                return _context.Cities.OrderBy(c => c.Name).
                    Include(c => c.PointsOfInterest).
                    Include(c => c.CityLanguages).
                    ThenInclude(l => l.Language).
                    ToList();
            }
            
            if ((true == includePointsOfInterest) && (false == includeLanguages))
            {
                return _context.Cities.OrderBy(c => c.Name).
                    Include(c => c.PointsOfInterest).
                    ToList();
            }

            if ((false == includePointsOfInterest) && (true == includeLanguages))
            {
                return _context.Cities.OrderBy(c => c.Name).
                    Include(c => c.CityLanguages).
                    ToList();
            }

            return _context.Cities.OrderBy(c => c.Name).
                ToList();
        }

        public City GetCity(int cityId, bool includePointsOfInterest)
        {
            if (includePointsOfInterest)
            {
                return _context.Cities.Include(c => c.PointsOfInterest)
                    .Where(c => c.Id == cityId).FirstOrDefault();
            }

            return _context.Cities
                    .Where(c => c.Id == cityId).FirstOrDefault();
        }

        public City GetCityAdvanced(int cityId,
                             bool includePointsOfInterest = false,
                             bool includeLanguages = false)
        {
            if ((true == includePointsOfInterest) && (true == includeLanguages))
            {
                return _context.Cities.Where(c => c.Id == cityId).
                    Include(c => c.PointsOfInterest).
                    Include(c => c.CityLanguages).
                    ThenInclude(l => l.Language).
                    FirstOrDefault();
            }

            if ((true == includePointsOfInterest) && (false == includeLanguages))
            {
                return _context.Cities.Where(c => c.Id == cityId).
                    Include(c => c.PointsOfInterest).
                    FirstOrDefault();
            }

            if ((false == includePointsOfInterest) && (true == includeLanguages))
            {
                return _context.Cities.Where(c => c.Id == cityId).
                     Include(c => c.CityLanguages).
                     ThenInclude(l => l.Language).
                     FirstOrDefault();
            }

            return _context.Cities.Where(c => c.Id == cityId).
                    FirstOrDefault();
        }

        public PointOfInterest GetPointOfInterestForCity(int cityId, int pointOfInterestId)
        {
            return _context.PointsOfInterest
               .Where(p => p.CityId == cityId && p.Id == pointOfInterestId).FirstOrDefault();
        }

        public IEnumerable<PointOfInterest> GetPointsOfInterestForCity(int cityId)
        {
            return _context.PointsOfInterest
                          .Where(p => p.CityId == cityId).ToList();
        }

        public bool CityExists(int cityId)
        {
            return _context.Cities.Any(c => c.Id == cityId);
        }

        public void AddPointOfInterestForCity(int cityId, PointOfInterest pointOfInterest)
        {
            var city = GetCity(cityId, false);
            city.PointsOfInterest.Add(pointOfInterest);
        }

        public void UpdatePointOfInterestForCity(int cityId, PointOfInterest pointOfInterest)
        {
            // I andre implementatiober af ICityRepository skal der muligvis adderes kode
            // her for at få gemt opdaterede Cities.
        }

        public void DeletePointOfInterest(PointOfInterest pointOfInterest)
        {
            _context.PointsOfInterest.Remove(pointOfInterest);
        }

        public bool Save()
        {
            return (_context.SaveChanges() >= 0);
        }

        public void AddCity(City city)
        {
            _context.Cities.Add(city);
        }
    }
}
