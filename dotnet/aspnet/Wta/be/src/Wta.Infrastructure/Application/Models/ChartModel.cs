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
    public AxisLabel AxisLabel { get; set; } = new AxisLabel();

    public SplitLine SplitLine { get; set; } = new SplitLine();
    public SplitArea SplitArea { get; set; } = new SplitArea();
}

public class SplitLine
{
    public bool Show { get; set; } = true;
    public int Interval { get; set; } = 1;
}

public class SplitArea
{
    public bool Show { get; set; } = true;
}

public class AxisLabel
{
    public int Rotate { get; set; } = 45;
}

public class AxisLine
{
    public bool Show { get; set; } = true;
}

public class ChartYAxis
{
    public string Type { get; set; } = "value";
    public AxisLine AxisLine { get; set; } = new AxisLine();
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
