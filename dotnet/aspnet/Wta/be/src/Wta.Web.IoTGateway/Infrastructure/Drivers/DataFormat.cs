namespace Wta.Web.IoTGateway.Infrastructure.Drivers;

public enum DataFormat
{
    /// <summary>
    /// 按照顺序排序
    /// </summary>
    ABCD,

    /// <summary>
    /// 按照单字反转
    /// </summary>
    BADC,

    /// <summary>
    /// 按照双字反转
    /// </summary>
    CDAB,

    /// <summary>
    /// 按照倒序排序
    /// </summary>
    DCBA
}
