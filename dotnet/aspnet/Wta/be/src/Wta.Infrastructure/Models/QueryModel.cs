using System.ComponentModel;

namespace Wta.Infrastructure.Models;

public class QueryModel<TModel>
{
    public QueryModel()
    {
        this.Query = Activator.CreateInstance<TModel>();
    }

    [DefaultValue(1)]
    public int PageIndex { get; set; } = 1;

    [DefaultValue(10)]
    public int PageSize { get; set; } = 10;

    public int TotalCount { get; set; }

    [DefaultValue(null)]
    public string? OrderBy { get; set; }

    public TModel? Query { get; set; }

    public bool IncludeAll { get; set; }

    public List<QueryFilter> Filters { get; set; } = new List<QueryFilter>();

    public List<TModel> Items { get; set; } = new List<TModel>();
}
