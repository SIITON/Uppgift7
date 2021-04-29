using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Uppgift7.Models.Features.SMHI
{
    public interface ISmhiApiServices
    {
        IEnumerable<ChartData> ConvertToChartJson(SmhiModel data);
        Task<SmhiModel> GetPoint(double longitude, double latitude);

    }
}
