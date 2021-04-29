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
        private ISmhiApiServices _smhiApiServices;
        private IWeatherInputParser _inputParser;
        private readonly MvcWeatherContext _context;
        private string _smhiCity;

        public WeatherDataController(IEnumerable<IWeatherData> weatherDataServices, IWeatherInputParser inputParser,
                MvcWeatherContext context, ISmhiApiServices smhiApiServices)
        {
            _weatherDataServices = weatherDataServices;
            _inputParser = inputParser;
            _context = context;
            _weatherData = _inputParser.ParseWeatherInput();
            _smhiApiServices = smhiApiServices;
        }

        // Depreciated endpoint
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
        [HttpGet("SMHI/GetCity")]
        public ActionResult<string> GetSMHIcity()
        {
            return Json(_smhiCity);
        }
        [HttpGet("SMHI/GetChart")]
        public async Task<ActionResult<IEnumerable<WeatherModel>>> GetSmhiJson(string city)
        {
            _smhiCity = city;
            var lonlat = new Cities(_smhiCity).LonLat;

            SmhiModel data = await _smhiApiServices.GetPoint(lonlat[0], lonlat[1]);
            var chartData = _smhiApiServices.ConvertToChartJson(data);
            var query = chartData.Select(d => new { date = d.Date.ToString(), value = d.Value });
            return Json(query);
        }
        // GET: Data
        public IActionResult Index()
        {
            ViewData["Path"] = _inputParser.Path;

            return View();
        }
    }
}
