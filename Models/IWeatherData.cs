using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Uppgift7.Models
{
    public interface IWeatherData
    {
        WeatherModel GetData(IEnumerable<WeatherModel> data);
        IQueryable<WeatherModel> QueryData(IQueryable<WeatherModel> context);
    }
}
