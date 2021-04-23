using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Uppgift7.Models;
using Uppgift7.Models.Features.InputParser;
using Uppgift7.Data;
using System.Text.Encodings.Web;
using Microsoft.EntityFrameworkCore;
using System.Net;
using Uppgift7.Models.Features.SMHI;
using Newtonsoft.Json;
using System.Text;
using System.Net.Http;
using System.Globalization;

namespace Uppgift7.Controllers
{
    [ApiController]
    [Route("Data")]
    public class WeatherDataController : Controller
    {
        private IEnumerable<IWeatherData> _weatherDataServices;
        private IEnumerable<WeatherModel> _weatherData;
        private IWeatherInputParser _inputParser;
        private readonly MvcWeatherContext _context;
        private string _smhiCity;
        private IEnumerable<ChartData> _chartData;

        public WeatherDataController(IEnumerable<IWeatherData> weatherDataServices, IWeatherInputParser inputParser,
                MvcWeatherContext context)
        {
            _weatherDataServices = weatherDataServices;
            _inputParser = inputParser;
            _context = context;
            _weatherData = _inputParser.ParseWeatherInput();
        }

        [HttpGet("Analyze")]
        public ActionResult<IEnumerable<WeatherModel>> GetStuff()
        {
            var result = new List<WeatherModel>();
            foreach (var service in _weatherDataServices)
            {
                result.Add(service.GetData(_weatherData));
            }
            return Ok(result);
        }
        [HttpGet("GetJson")]
        public ActionResult<IEnumerable<WeatherModel>> GetJson()
        {
            var weather = from w in _context.Weather
                          select w;
            var query = weather.OrderBy(d => d.Timestamp)
                               .Select(d => new { date = d.Date, value = d.TemperatureC });
            return Json(query);
        }
        [HttpGet("GetSmhiJson")]
        public async Task<ActionResult<IEnumerable<WeatherModel>>> GetSmhiJson()
        {
            //_smhiCity = "Stockholm";
            var lonlat = new Cities(_smhiCity).LonLat;
            var data = await GetSmhiData(lonlat[0], lonlat[1]);

            var values = (from t in data.TimeSeries
                          from p in t.Parameters
                          where p.Name == "t"
                          select p.Values).SelectMany(v => v).ToArray();

            var dates = (from t in data.TimeSeries
                        select t.ValidTime).ToArray();

            var chartData = new List<ChartData>();
            for (int i = 0; i < values.Length; i++)
            {
                chartData.Add(new ChartData
                {
                    Date = dates[i],
                    Value = values[i]
                });
            }

            var query = chartData.Select(d => new { date = d.Date, value = d.Value });
            return Json(query);
        }
        [HttpGet("GetSmhiData")]
        public async Task<SmhiModel> GetSmhiData(double longitude, double latitude)
        {
            var lon = longitude.ToString(CultureInfo.InvariantCulture);
            var lat = latitude.ToString(CultureInfo.InvariantCulture);

            var url = $"https://opendata-download-metfcst.smhi.se/api/category/pmp3g/version/2/geotype/point/lon/{lon}/lat/{lat}/data.json";
            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync(url);
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<SmhiModel>(content);
        }

        // GET: Data
        public IActionResult Index(string cityName)
        {
            ViewData["Path"] = _inputParser.Path;
            if (String.IsNullOrEmpty(cityName))
            {
                ViewData["ChartJsonSource"] = "/Data/GetJson";
            }
            else
            {
                _smhiCity = cityName;
                ViewData["ChartJsonSource"] = "/Data/GetSmhiJson";
            }
            //_smhiCity = "Stockholm";
            //ViewData["ChartJsonSource"] = "/Data/GetSmhiJson";

            return View();
        }

        // GET: /Data/Welcome/
        [HttpGet("Welcome")]
        public IActionResult Welcome(string name, int numTimes = 1)
        {
            ViewData["Message"] = "Hello " + name;
            ViewData["NumTimes"] = numTimes;

            return View();
        }
    }
}
