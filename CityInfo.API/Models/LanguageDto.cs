using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfo.API.Models
{
    public class LanguageDto
    {
        public int Id { get; set; }
        public string LanguageName { get; set; }

        public virtual ICollection<CityLanguageDto> CityLanguages { get; set; }
               = new List<CityLanguageDto>();
    }
}
