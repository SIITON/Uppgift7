using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Uppgift7.Models.Features.SMHI
{
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
}
