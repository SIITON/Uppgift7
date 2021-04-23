using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Uppgift7.Models.Features.SMHI;

namespace Uppgift7.Models
{
    public class SmhiModel
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
