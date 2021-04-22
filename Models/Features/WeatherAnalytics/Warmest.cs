using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Uppgift7.Models.Features.WeatherAnalytics
{
    public class Warmest : IWeatherData
    {
        public WeatherModel GetData(IEnumerable<WeatherModel> data)
        {
            var result = (from d in data
                          orderby d.TemperatureC descending
                          select d).First();
            result.Identifier = "Warmest";
            return result;
        }
        public IQueryable<WeatherModel> QueryData(IQueryable<WeatherModel> weather)
        {
            var warmestId = weather.Where(d => d.Id == GetData(weather).Id)
                                   .Select(d => d.Id)
                                   .ToArray()[0];
            foreach (var measurement in weather)
            {
                if (measurement.Id == warmestId)
                {
                    measurement.Identifier = "Warmest";
                }
            }
            return weather;
        }
    }
}
