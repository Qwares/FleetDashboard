using System.Globalization;
using FleetDashboard.Models;

namespace FleetDashboard.Services;

/// <summary>
/// X ekseni değerini tarih mi etiket mi anlar; tarihse sıralama için kullanılır.
/// </summary>
public static class ChartDataService
{
    private static readonly string[] DateFormats = { "dd.MM.yyyy", "d.M.yyyy", "dd/MM/yyyy", "yyyy-MM-dd", "dd.M.yyyy" };

    public static bool TryParseAsDate(string? value, out DateTime date)
    {
        date = default;
        if (string.IsNullOrWhiteSpace(value)) return false;
        return DateTime.TryParseExact(value.Trim(), DateFormats, CultureInfo.InvariantCulture, DateTimeStyles.None, out date)
            || DateTime.TryParse(value, CultureInfo.GetCultureInfo("tr-TR"), DateTimeStyles.None, out date);
    }

    /// <summary>
    /// Tüm X değerleri (Plate) tarihse true.
    /// </summary>
    public static bool AllXValuesAreDates(IEnumerable<VehicleMetric> metrics)
    {
        return metrics.All(m => string.IsNullOrWhiteSpace(m.Plate) || TryParseAsDate(m.Plate, out _));
    }

    /// <summary>
    /// Tarih modundaysa tarihe göre sıralanmış liste döner; değilse orijinal sıra.
    /// </summary>
    public static List<VehicleMetric> SortForChart(IEnumerable<VehicleMetric> metrics, bool isTimeSeries)
    {
        var list = metrics.ToList();
        if (!isTimeSeries) return list;
        return list
            .OrderBy(m => string.IsNullOrWhiteSpace(m.Plate) ? DateTime.MinValue : TryParseAsDate(m.Plate, out var d) ? d : DateTime.MinValue)
            .ToList();
    }
}
