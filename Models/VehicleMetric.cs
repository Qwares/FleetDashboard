namespace FleetDashboard.Models;

/// <summary>
/// Represents a single vehicle metric for fleet visualization.
/// Used as the source model for chart X-axis (Plate) and Y-axis (Value: Speed or Fuel).
/// </summary>
public class VehicleMetric
{
    /// <summary>Vehicle plate / identifier (X-axis label).</summary>
    public string Plate { get; set; } = string.Empty;

    /// <summary>Metric value: Speed (e.g. km/h) or Fuel (e.g. L) — Y-axis.</summary>
    public double Value { get; set; }

    /// <summary>When true, usage is counted as off-hours (night shift); drives bar color to Charcoal.</summary>
    public bool IsNightShift { get; set; }

    /// <summary>Safety/Policy limit; values above this are policy violations. Used for threshold line and insights.</summary>
    public double Threshold { get; set; }

    /// <summary>True when Value exceeds Threshold (and Threshold &gt; 0).</summary>
    public bool IsViolation => Value > Threshold && Threshold > 0;

    /// <summary>Display status for tooltips and insights: "Safe" or "Danger".</summary>
    public string PolicyStatus => IsViolation ? "Danger" : "Safe";
}
