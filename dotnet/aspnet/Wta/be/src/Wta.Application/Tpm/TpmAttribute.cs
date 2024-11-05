namespace Wta.Application.Tpm;
[Display(Name = "TPM", Order = 15)]
public class TpmAttribute : GroupAttribute
{
}

[Display(Name = "点检管理", Order = 10)]
public class DevicePlanCheckAttribute : TpmAttribute
{
}
