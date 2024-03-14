using System.Globalization;
using System.Windows.Controls;

namespace WpfApp1;

public class RequiredRule : ValidationRule
{
    public override ValidationResult Validate(object value, CultureInfo cultureInfo)
    {
        return new ValidationResult(!string.IsNullOrWhiteSpace((value ?? "").ToString()), "不能为空");
    }
}