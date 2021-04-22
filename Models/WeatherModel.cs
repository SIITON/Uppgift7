using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Uppgift7.Models
{
    public class WeatherModel
    {
        public int Id { get; set; }
#pragma warning disable CS8632
        public string? Identifier { get; set; }
#pragma warning restore CS8632
        [Display(Name = "UNIX Epoch time")]
        public int Timestamp { get; set; }
        [Display(Name = "Date & time of measurement")]
        public DateTime Date { get; set; }
        [Display(Name = "Temperature [°C]")]
        public double TemperatureC { get; set; }
    }
}
