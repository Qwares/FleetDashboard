namespace FleetDashboard.Models;

/// <summary>
/// Chart DTO: one item per vehicle for ApexCharts.
/// Line: DayValue/NightValue (Gündüz/Gece). Bar: DaySafeValue, DayViolationValue, NightSafeValue, NightViolationValue for threshold-based colors.
/// </summary>
public class FleetChartDataItem
{
    public string Plate { get; set; } = string.Empty;
    /// <summary>Value shown in "Day" series (blue); 0 when IsNightShift.</summary>
    public double DayValue { get; set; }
    /// <summary>Value shown in "Night" series (charcoal); 0 when not IsNightShift.</summary>
    public double NightValue { get; set; }
    public double Threshold { get; set; }
    public bool IsNightShift { get; set; }
    public double Value => DayValue + NightValue;

    /// <summary>Bar: Gündüz limit altı (mavi).</summary>
    public double DaySafeValue => Threshold > 0 && DayValue <= Threshold ? DayValue : (Threshold <= 0 ? DayValue : 0);
    /// <summary>Bar: Gündüz limit üstü (kırmızı).</summary>
    public double DayViolationValue => Threshold > 0 && DayValue > Threshold ? DayValue : 0;
    /// <summary>Bar: Gece limit altı (gri).</summary>
    public double NightSafeValue => Threshold > 0 && NightValue <= Threshold ? NightValue : (Threshold <= 0 ? NightValue : 0);
    /// <summary>Bar: Gece limit üstü (kırmızı).</summary>
    public double NightViolationValue => Threshold > 0 && NightValue > Threshold ? NightValue : 0;

    /// <summary>For tooltip: "Safe" or "Danger" based on value vs threshold.</summary>
    public string PolicyStatus => (IsNightShift ? NightValue : DayValue) > Threshold && Threshold > 0 ? "Danger" : "Safe";
    /// <summary>Tooltip: "Limit Altı" or "Limit Üstü".</summary>
    public string DurumText => PolicyStatus == "Danger" ? "Limit Üstü" : "Limit Altı";

    /// <summary>Anomali tespiti: bu nokta aykırıysa değer, değilse 0 (sarı seri için).</summary>
    public double OutlierValue { get; set; }
    /// <summary>Filo ortalamasının üstünde mi (benchmarking tooltip).</summary>
    public bool IsAboveAverage { get; set; }
}
