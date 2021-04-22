using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Uppgift7.Data;
using Uppgift7.Models.Features.InputParser;
using System;
using System.Linq;
using System.Collections.Generic;
using Uppgift7.Migrations;

namespace Uppgift7.Models
{
    public static class SeedData
    {
        private static IEnumerable<WeatherModel> _weatherData;

        public static void Initialize(IServiceProvider serviceProvider)
        {
            _weatherData = new WeatherCsvParser("temperatures.csv").ParseWeatherInput();
            using (var context = new MvcWeatherContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<MvcWeatherContext>>()))
            {
                // Look for any weather measurements.
                if (context.Weather.Any())
                {
                    return;   // DB has been seeded
                }
                var sortedData = from data in _weatherData
                                 orderby data.Timestamp ascending
                                 select data;
                foreach (var measurement in sortedData)
                {
                    context.Add(measurement);
                }
                context.SaveChanges();
            }
        }
    }
}