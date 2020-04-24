using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfo.API.Models
{
    public class CityLanguageDto
    {
        public int CityId { get; set; }
        public int LanguageId { get; set; }

        public CityDto City { get; set; }

        public LanguageDto Language { get; set; }
    }
}
