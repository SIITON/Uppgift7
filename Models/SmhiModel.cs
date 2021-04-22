using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Uppgift7.Models
{
    public class SmhiModel
    {
        public class Geometry
        {
            [JsonPropertyName("type")]
            public string Type { get; set; }

            [JsonPropertyName("coordinates")]
            public List<List<double>> Coordinates { get; set; }
        }

        public class Parameter
        {
            [JsonPropertyName("name")]
            public string Name { get; set; }

            [JsonPropertyName("levelType")]
            public string LevelType { get; set; }

            [JsonPropertyName("level")]
            public int Level { get; set; }

            [JsonPropertyName("unit")]
            public string Unit { get; set; }

            [JsonPropertyName("values")]
            public List<double> Values { get; set; }
        }

        public class TimeSery
        {
            [JsonPropertyName("validTime")]
            public DateTime ValidTime { get; set; }

            [JsonPropertyName("parameters")]
            public List<Parameter> Parameters { get; set; }
        }

        public class Root
        {
            [JsonPropertyName("approvedTime")]
            public DateTime ApprovedTime { get; set; }

            [JsonPropertyName("referenceTime")]
            public DateTime ReferenceTime { get; set; }

            [JsonPropertyName("geometry")]
            public Geometry Geometry { get; set; }

            [JsonPropertyName("timeSeries")]
            public List<TimeSery> TimeSeries { get; set; }
        }
    }
}
