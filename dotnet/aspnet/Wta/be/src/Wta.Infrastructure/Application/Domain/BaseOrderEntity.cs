namespace Wta.Infrastructure.Application.Domain;

public abstract class BaseOrderEntity<TOrderItem> : Entity
{
    [Display(Name = "单据编号")]
    public string Number { get; set; } = null!;

    [Display(Name = "创建时间")]
    public DateTime CreationTime { get; set; }

    [Display(Name = "单据明细")]
    public List<TOrderItem> Items { get; set; } = [];
}

public abstract class BaseOrderItemEntity<TOrder> : Entity
{
    [KeyValue("hideForEdit", true)]
    public Guid OrderId { get; set; }

    public TOrder? Order { get; set; }
}
