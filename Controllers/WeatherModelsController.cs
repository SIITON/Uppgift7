using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Uppgift7.Data;
using Uppgift7.Models;

namespace Uppgift7.Controllers
{
    public class WeatherModelsController : Controller
    {
        private readonly MvcWeatherContext _context;
        private readonly IEnumerable<IWeatherData> _weatherAnalyticServices;

        public WeatherModelsController(MvcWeatherContext context, IEnumerable<IWeatherData> weatherAnalyticServices)
        {
            _context = context;
            _weatherAnalyticServices = weatherAnalyticServices;
        }

        // GET: WeatherModels
        public async Task<IActionResult> Index()
        {
            var weather = from w in _context.Weather
                          select w;
            foreach (var service in _weatherAnalyticServices)
            {
                weather = service.QueryData(weather);
            }
            return View(await weather.OrderBy(d => d.Timestamp).ToListAsync());
        }

        // GET: WeatherModels
        public async Task<IActionResult> IndexSearched(double lower, double upper)
        {
            var weather = from w in _context.Weather
                          select w;
            if (lower != 0 && upper != 0)
            {
                weather = weather.Where(t => t.Date.Hour >= lower && t.Date.Hour < upper);
            }
            weather = weather.OrderBy(w => w.Timestamp);
            foreach (var service in _weatherAnalyticServices)
            {
                weather = service.QueryData(weather);
            }
            return View(await weather.ToListAsync());
            //return View(await _context.Weather.ToListAsync());
        }

        // GET: WeatherModels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var weatherModel = await _context.Weather
                .FirstOrDefaultAsync(m => m.Id == id);
            if (weatherModel == null)
            {
                return NotFound();
            }

            return View(weatherModel);
        }

        // GET: WeatherModels/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: WeatherModels/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Identifier,Timestamp,Date,TemperatureC")] WeatherModel weatherModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(weatherModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(weatherModel);
        }

        // GET: WeatherModels/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var weatherModel = await _context.Weather.FindAsync(id);
            if (weatherModel == null)
            {
                return NotFound();
            }
            return View(weatherModel);
        }

        // POST: WeatherModels/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Identifier,Timestamp,Date,TemperatureC")] WeatherModel weatherModel)
        {
            if (id != weatherModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(weatherModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WeatherModelExists(weatherModel.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(weatherModel);
        }

        // GET: WeatherModels/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var weatherModel = await _context.Weather
                .FirstOrDefaultAsync(m => m.Id == id);
            if (weatherModel == null)
            {
                return NotFound();
            }

            return View(weatherModel);
        }

        // POST: WeatherModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var weatherModel = await _context.Weather.FindAsync(id);
            _context.Weather.Remove(weatherModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WeatherModelExists(int id)
        {
            return _context.Weather.Any(e => e.Id == id);
        }
    }
}
