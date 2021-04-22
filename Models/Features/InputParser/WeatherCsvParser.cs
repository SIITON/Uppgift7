using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Uppgift7.Models.Features.InputParser
{
    public class WeatherCsvParser : IWeatherInputParser
    {
        private IEnumerable<WeatherModel> _weatherData;
        public string Path { get; set; }
        public WeatherCsvParser(string csvPath)
        {
            Path = csvPath;
        }
        public IEnumerable<WeatherModel> GetData()
        {
            return _weatherData;
        }
        public IEnumerable<WeatherModel> ParseWeatherInput()
        {
            var csvData = System.IO.File.ReadAllLines(Path).Select(lines => lines.Split(';').ToArray());
            _weatherData = csvData.Select(data => new WeatherModel
            {
                Timestamp = int.Parse(data[0]),
                TemperatureC = double.Parse(data[1], CultureInfo.InvariantCulture),
                Date = DateTime.Parse(data[2])
            })
            .AsEnumerable();
            return _weatherData;
        }
        public void SetDataSource(string csvPath)
        {
            Path = csvPath;
        }
    }
}
