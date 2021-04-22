using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Uppgift7.Models.Features.SMHI
{
    public class Cities
    {
        public double[] LonLat { get; set; }
        // [lon, lat]
        private readonly double[] _stockholm = new double[] {18.063240, 59.334591};
        private readonly double[] _visby = new double[] { 18.294840, 57.634800 };
        private readonly double[] _gothenburg = new double[] { 11.974560, 57.708870 };
        public Cities(string cityName)
        {
            switch (cityName)
            {
                case "Stockholm":
                    LonLat = _stockholm;
                    break;
                case "Visby":
                    LonLat = _visby;
                    break;
                case "Gothenburg":
                    LonLat = _gothenburg;
                    break;
                default:
                    LonLat = _stockholm;
                    break;
            }
        }
}
}
