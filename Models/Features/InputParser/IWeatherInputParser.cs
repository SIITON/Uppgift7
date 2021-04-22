using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Uppgift7.Models.Features.InputParser
{
    public interface IWeatherInputParser
    {
        IEnumerable<WeatherModel> GetData();
        IEnumerable<WeatherModel> ParseWeatherInput();
        void SetDataSource(string path);
        string Path { get; set; }
    }
}
