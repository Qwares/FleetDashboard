using ApexCharts;
using FleetDashboard.Models;
using FleetDashboard.Services;
using Microsoft.AspNetCore.Components;

namespace FleetDashboard.Components;

/// <summary>
/// X ekseni tarihse zaman serisi (tarihe göre sıralı); değilse etiket. Tooltip ve başlık dinamik.
/// </summary>
public partial class FleetChart
{
    [Parameter] public List<VehicleMetric> Metrics { get; set; } = new();
    [Parameter] public bool IsLineChart { get; set; }
    [Parameter] public double? ThresholdLine { get; set; }
    [Parameter] public string XAxisLabel { get; set; } = "Plaka / Sürücü";
    [Parameter] public string YAxisLabel { get; set; } = "Değer";
    [Parameter] public string Unit { get; set; } = "km/s";

    private List<FleetChartDataItem> ChartData { get; set; } = new();
    private ApexChartOptions<FleetChartDataItem>? ChartOptions { get; set; }
    private bool IsTimeSeries { get; set; }
    private string ChartTitle => (IsTimeSeries ? "Zaman Serisi · " : "") + (IsLineChart ? "Trend Analizi" : "Kıyaslama");

    protected override void OnParametersSet()
    {
        IsTimeSeries = ChartDataService.AllXValuesAreDates(Metrics);
        var ordered = ChartDataService.SortForChart(Metrics, IsTimeSeries);

        var criticalThreshold = ThresholdLine ?? 0;
        ChartData = ordered
            .Select((m, i) => new FleetChartDataItem
            {
                Plate = string.IsNullOrWhiteSpace(m.Plate) ? $"#{i + 1}" : m.Plate,
                DayValue = m.IsNightShift ? 0 : m.Value,
                NightValue = m.IsNightShift ? m.Value : 0,
                Threshold = criticalThreshold > 0 ? criticalThreshold : m.Threshold,
                IsNightShift = m.IsNightShift
            })
            .ToList();

        var values = ChartData.Select(x => x.Value).ToList();
        var avg = values.Count > 0 ? values.Average() : 0;
        var stdDev = values.Count > 2 ? Math.Sqrt(values.Average(v => Math.Pow(v - avg, 2))) : 0;
        for (var i = 0; i < ChartData.Count; i++)
        {
            var item = ChartData[i];
            item.IsAboveAverage = item.Value > avg;
            item.OutlierValue = (values.Count > 2 && stdDev > 0 && Math.Abs(item.Value - avg) > 2 * stdDev) ? item.Value : 0;
        }

        ChartOptions = new ApexChartOptions<FleetChartDataItem>
        {
            Chart = new Chart
            {
                Toolbar = new Toolbar { Show = true },
                Animations = new Animations { Enabled = true, Speed = 400, DynamicAnimation = new DynamicAnimation { Enabled = true } }
            },
            Xaxis = new XAxis { Labels = new XAxisLabels { Rotate = -45 } },
            Tooltip = new Tooltip { Enabled = true, Shared = true, Intersect = false },
            DataLabels = new DataLabels { Enabled = false }
        };

        if (IsLineChart)
        {
            // Trend görünümü: yumuşak çizgi, belirgin noktalar (Gündüz mavi, Gece gri)
            ChartOptions.Stroke = new Stroke { Curve = Curve.Smooth, Width = 2, Show = true };
            ChartOptions.Markers = new Markers
            {
                Size = 5,
                StrokeWidth = 2,
                StrokeColors = new Color("#fff"),
                Hover = new MarkersHover { Size = 7, SizeOffset = 2 }
            };
        }
        else
        {
            // Kıyaslama görünümü: sütun genişliği, eşik aşanlar kırmızı (4 seri ile)
            ChartOptions.PlotOptions = new PlotOptions
            {
                Bar = new PlotOptionsBar
                {
                    ColumnWidth = "65%",
                    BorderRadius = 4,
                    Horizontal = false
                }
            };
        }

        var yAnnotations = new List<AnnotationsYAxis>();
        if (ThresholdLine.HasValue && ThresholdLine.Value > 0)
        {
            yAnnotations.Add(new AnnotationsYAxis
            {
                Y = ThresholdLine.Value,
                BorderColor = "#e74c3c",
                StrokeDashArray = 4
            });
        }
        if (ChartData.Count > 0)
        {
            var fleetAvg = ChartData.Average(x => x.Value);
            if (fleetAvg > 0)
                yAnnotations.Add(new AnnotationsYAxis
                {
                    Y = fleetAvg,
                    BorderColor = "#007AFF",
                    StrokeDashArray = 6,
                    Label = new Label { Text = "Filo Ortalaması", BorderColor = "#007AFF" }
                });
        }
        if (yAnnotations.Count > 0)
            ChartOptions.Annotations = new Annotations { Yaxis = yAnnotations };
    }
}
