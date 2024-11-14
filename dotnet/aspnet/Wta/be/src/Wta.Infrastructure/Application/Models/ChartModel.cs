namespace Wta.Infrastructure.Application.Models;

public class ChartModel
{
    public ChartTitle Title { get; set; } = new ChartTitle();
    public ChartLegend Legend { get; set; } = new ChartLegend();
    public ChartXAxis XAxis { get; set; } = new ChartXAxis();
    public ChartYAxis YAxis { get; set; } = new ChartYAxis();
    public List<ChartSerie> Series { get; set; } = [];
}

public class ChartTitle
{
    public string Left { get; set; } = "center";
    public string Text { get; set; } = "";
}

public class ChartXAxis
{
    public string Type { get; set; } = "category";
    public List<string> Data { get; set; } = [];
}

public class ChartYAxis
{
    public string Type { get; set; } = "value";
}

public class ChartLegend
{
    public bool Show { get; set; }
    public string? Icon { get; set; }
    public string? Left { get; set; }
    public string? Bottom { get; set; }
    public List<string> Data { get; set; } = [];
}

public class ChartSerie
{
    public string? Name { get; set; }
    public string Type { get; set; } = "line";
    public bool ShowSymbol { get; set; } = true;
    public bool Smooth { get; set; } = true;
    public ChartLineStyle? LineStyle { get; set; }
    public List<float> Data { get; set; } = [];
}

public class ChartLineStyle
{
    public string? Color { get; set; }
}
