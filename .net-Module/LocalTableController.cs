using Microsoft.EntityFrameworkCore.ChangeTracking;
using WeatherStats.Models;

namespace WeatherStats;

public class LocalTableController
{
    public void InsertData(WeatherInfoContext context)
    {
        var infoEF = new WeatherInfoEF(DateTime.Now, 98, 99);
        context.WeatherInfos.Add(infoEF);
        context.SaveChanges();
    }

    public List<WeatherInfoEF> ReadData(WeatherInfoContext context)
    {
        return context.WeatherInfos.ToList();
    }

    public EntityEntry<WeatherInfoEF> DeleteData(WeatherInfoContext weatherContext, WeatherInfoEF infoEF)
    {
        return weatherContext.Remove(infoEF);
    }
}