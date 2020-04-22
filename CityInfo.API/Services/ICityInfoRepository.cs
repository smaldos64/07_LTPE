using CityInfo.API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfo.API.Services
{
    public interface ICityInfoRepository
    {
        IEnumerable<City> GetCitiesAdvanced(bool includePointsOfInterest = false,
                                            bool includeLanguages = false);
        City GetCityAdvanced(int cityId,
                             bool includePointsOfInterest = false,
                             bool includeLanguages = false);

        void AddCity(City city);

        IEnumerable<City> GetCities();
        
        City GetCity(int cityId, bool includePointsOfInterest);
        
        IEnumerable<PointOfInterest> GetPointsOfInterestForCity(int cityId);

        PointOfInterest GetPointOfInterestForCity(int cityId, int pointOfInterestId);

        bool CityExists(int cityId);

        void AddPointOfInterestForCity(int cityId, PointOfInterest pointOfInterest);

        void UpdatePointOfInterestForCity(int cityId, PointOfInterest pointOfInterest);

        void DeletePointOfInterest(PointOfInterest pointOfInterest);

        bool Save();
    }
}
