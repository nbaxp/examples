namespace Wta.Application.Oee.Models;

public class OeeDataModel
{
    public string StatusNumber { get; set; } = default!;
    public int Duration { get; set; }
    public float StandardUpm { get; set; }
    public int Total { get; set; }
    public int EequipmentScrap { get; set; }
    public int NonEequipmentScrap { get; set; }
}

public class OeeDayModel : OeeDataModel
{
    public DateOnly Date { get; set; }
    public string AssetNumber { get; set; } = default!;
}

public class OeeModel
{
    public float Availability { get; set; }
    public float Performance { get; set; }
    public float Quality { get; set; }
}

public class OeeGroupModel : OeeModel
{
    public string Key { get; set; } = default!;
}
