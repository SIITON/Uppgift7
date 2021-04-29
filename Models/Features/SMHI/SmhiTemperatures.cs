using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Uppgift7.Models.Features.SMHI
{
    public class SmhiTemperatures : ISmhiApiServices
    {
        public IEnumerable<ChartData> ConvertToChartJson(SmhiModel data)
        {
            var values = (from t in data.TimeSeries
                          from p in t.Parameters
                          where p.Name == "t"
                          select p.Values).SelectMany(v => v).ToArray();

            var dates = (from t in data.TimeSeries
                         select t.ValidTime).ToArray();

            var chartData = new List<ChartData>();
            for (int i = 0; i < values.Length; i++)
            {
                chartData.Add(new ChartData
                {
                    Date = dates[i],
                    Value = values[i]
                });
            }
            return chartData;
        }
        public async Task<SmhiModel> GetPoint(double longitude, double latitude)
        {
            var lon = longitude.ToString(CultureInfo.InvariantCulture);
            var lat = latitude.ToString(CultureInfo.InvariantCulture);

            var url = $"https://opendata-download-metfcst.smhi.se/api/category/pmp3g/version/2/geotype/point/lon/{lon}/lat/{lat}/data.json";
            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync(url);
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<SmhiModel>(content);
        }
    }
}
