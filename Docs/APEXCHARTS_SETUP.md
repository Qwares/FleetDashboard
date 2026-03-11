# Blazor-ApexCharts Configuration

## 1. Package Reference

In `FleetDashboard.csproj`:

```xml
<PackageReference Include="Blazor-ApexCharts" Version="6.1.0" />
```

## 2. Program.cs

Register the chart service so components can use ApexCharts:

```csharp
using ApexCharts;

// ...

builder.Services.AddApexCharts();
```

No other ApexCharts registration is required for Blazor WebAssembly.

## 3. _Imports.razor

Add the ApexCharts namespace so all Razor pages and components can use `<ApexChart>`, `ApexChartOptions<T>`, `HoverData<T>`, etc. without a full qualifier:

```razor
@using ApexCharts
```

## 4. index.html (optional)

The library serves its own CSS. If you need to override chart styles, ensure Bootstrap (or your base styles) loads before the app. No extra script is required for ApexCharts in Blazor WebAssembly.

## 5. Usage in this project

- **FleetChart.razor** uses `<ApexChart TItem="FleetChartDataItem">` with `<ApexPointSeries>` for Bar/Line and `<ApexPointTooltip>` for custom tooltips.
- Chart options (annotations, tooltip, etc.) are set via `ApexChartOptions<FleetChartDataItem>` in **FleetChart.razor.cs**.
