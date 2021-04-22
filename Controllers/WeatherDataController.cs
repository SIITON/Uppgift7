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

        // GET: Data
        public IActionResult Index()
        {
            ViewData["Path"] = _inputParser.Path;
            ViewData["Categories"] = _weatherData.Select(d => d.Date).ToList();
            ViewData["Measurements"] = _weatherData.Select(d => d.TemperatureC).ToList();
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
