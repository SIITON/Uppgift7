using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Uppgift7.Models.Features.SMHI
{
    public class TimeSery
    {
        [JsonPropertyName("validTime")]
        public DateTime ValidTime { get; set; }

        [JsonPropertyName("parameters")]
        public List<Parameter> Parameters { get; set; }
    }
}
