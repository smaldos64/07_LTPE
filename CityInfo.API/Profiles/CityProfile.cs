﻿using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfo.API.Profiles
{
    public class CityProfile : Profile
    {
        public CityProfile()
        {
            // CreateMap<source, destination>()
            CreateMap<Entities.City, Models.CityWithoutPointsOfInterestDto>();
            CreateMap<Entities.City, Models.CityDto>();
            // Entities er vores Database model.
            // Models er vores præsentations model

            CreateMap<Models.CityForUpdateDto, Entities.City>()
                .ReverseMap();
        }
    }
}
