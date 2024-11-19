namespace Wta.Infrastructure.Application.Models;

/// <summary>
/// https://echarts.apache.org/zh/option.html
/// </summary>
public class ChartModel
{
    public ChartTitle Title { get; set; } = new ChartTitle();
    public ChartLegend Legend { get; set; } = new ChartLegend();
    public ChartXAxis XAxis { get; set; } = new ChartXAxis();
    public ChartYAxis YAxis { get; set; } = new ChartYAxis();
    public List<ChartSerie> Series { get; set; } = [];
    public ChartTooltip Tooltip { get; set; } = new ChartTooltip();
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
    public ChartAxisLabel AxisLabel { get; set; } = new ChartAxisLabel();

    public ChartSplitLine SplitLine { get; set; } = new ChartSplitLine();
    public ChartSplitArea SplitArea { get; set; } = new ChartSplitArea();
}

public class ChartSplitLine
{
    public bool Show { get; set; } = true;
    public int Interval { get; set; } = 1;
}

public class ChartSplitArea
{
    public bool Show { get; set; } = true;
}

public class ChartAxisLabel
{
    public int Rotate { get; set; } = 45;
}

public class ChartAxisLine
{
    public bool Show { get; set; } = true;
}

public class ChartYAxis
{
    public string Type { get; set; } = "value";
    public ChartAxisLine AxisLine { get; set; } = new ChartAxisLine();
}

public class ChartLegend
{
    public bool Show { get; set; } = true;
    public string? Icon { get; set; }
    public string? Left { get; set; } = "auto";
    public string? Top { get; set; } = "auto";
    public string? Right { get; set; } = "auto";
    public string? Bottom { get; set; } = "auto";
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

public class ChartAxisPointer
{
    public ChartLineStyle LineStyle { get; set; } = new ChartLineStyle();
}

public class ChartTooltip
{
    public string Trigger { get; set; } = "axis";
    public ChartAxisPointer AxisPointer { get; set; } = new ChartAxisPointer();
}
