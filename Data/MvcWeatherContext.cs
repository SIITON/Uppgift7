using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Uppgift7.Models;

namespace Uppgift7.Data
{
    public class MvcWeatherContext : DbContext
    {
        public MvcWeatherContext(DbContextOptions<MvcWeatherContext> options) : base(options)
        {
        }
        public DbSet<WeatherModel> Weather { get; set; }
    }
}
