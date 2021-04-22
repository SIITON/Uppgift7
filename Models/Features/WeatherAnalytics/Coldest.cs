using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Uppgift7.Models.Features.WeatherAnalytics
{
    public class Coldest : IWeatherData
    {
        public WeatherModel GetData(IEnumerable<WeatherModel> data)
        {
            var result = (from d in data
                          orderby d.TemperatureC ascending
                          select d).First();
            result.Identifier = "Coldest";
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
                    measurement.Identifier = "Coldest";
                }
            }
            return weather;
        }
    }
}
