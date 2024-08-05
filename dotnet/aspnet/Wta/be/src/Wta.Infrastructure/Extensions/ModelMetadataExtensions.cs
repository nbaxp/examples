using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Wta.Infrastructure.Application.Domain;

namespace Wta.Infrastructure.Extensions;

public static class ModelMetadataExtensions
{
    public static object GetSchema(this ModelMetadata meta, IServiceProvider serviceProvider, ModelMetadata? parent = null)
    {
        var result = new Dictionary<string, object>();
        var metaData = (meta as DefaultModelMetadata)!;
        var modelType = meta.UnderlyingOrModelType;
        var isValueType = metaData.ModelType.IsValueType;
        var isNullableType = !metaData.IsRequired;
        result.TryAdd("isNullable", isNullableType);
        //是否跟
        if (parent == null)
        {
            result.Add("isRoot", true);
        }
        //是否树
        if (meta.ContainerType == null && modelType.GetBaseClasses().Any(o => o.IsGenericType && o.GetGenericTypeDefinition() == typeof(BaseTreeEntity<>)))
        {
            result.TryAdd("isTree", true);
        }
        if (parent != null &&
            parent.ModelType.GetBaseClasses().Any(o => o.IsGenericType && o.GetGenericTypeDefinition() == typeof(BaseTreeEntity<>)) &&
            meta.PropertyName == nameof(BaseTreeEntity<BaseEntity>.ParentId))
        {
            result.TryAdd("url", $"{parent.ModelType.Name.ToSlugify()}/search");
            result.TryAdd("value", "id");
            result.TryAdd("label", "name");
            result.TryAdd("input", "select");
        }
        //标题
        var title = meta.ContainerType == null ? modelType.GetDisplayName() : meta.ContainerType?.GetProperty(meta.PropertyName!)?.GetDisplayName();
        result.Add("title", title!);
        //简介
        if (meta.Description != null)
        {
            result.Add("description", meta.Description);
        }
        //只读
        if (metaData.Attributes.Attributes!.FirstOrDefault(o => o.GetType() == typeof(ReadOnlyAttribute)) is ReadOnlyAttribute readOnlyAttribute &&
            readOnlyAttribute.IsReadOnly)
        {
            result.Add("readOnly", true);
        }
        //属性
        metaData.Attributes.Attributes?.ForEach(o =>
        {
            if (o is KeyValueAttribute keyValue)
            {
                result.TryAdd(keyValue.Key, keyValue.Value);
            }
            else if (o is DataTypeAttribute dataType)
            {
                if (dataType.DataType == DataType.Password)
                {
                    result.TryAdd("input", "password");
                }
            }
        });
        //输入
        if (metaData.Attributes.Attributes!.Any(o => o.GetType() == typeof(HiddenAttribute)))
        {
            result.TryAdd("hidden", true);
        }
        if (meta.TemplateHint != null)
        {
            result.TryAdd("input", meta.TemplateHint?.ToLowerCamelCase()!);
        }
        //类型
        if (isValueType)//值类型
        {
            if (modelType.IsEnum)
            {
                result.Add("type", "number");
                result.TryAdd("input", "select");
                if (metaData.IsFlagsEnum)
                {
                    result.TryAdd("multiple", true);
                }
                var options = Enum.GetNames(modelType).Select(o => new
                {
                    Value = o,
                    Label = ((Enum)Enum.Parse(modelType, o)).GetDisplayName()
                }).ToArray();
                result.TryAdd("options", options);
            }
            else if (modelType == typeof(bool))
            {
                result.Add("type", "boolean");
                if (isNullableType)
                {
                    result.TryAdd("input", "select");
                }
                else
                {
                    result.TryAdd("input", "checkbox");
                }
            }
            else if (modelType == typeof(DateTime))
            {
                result.Add("type", "string");
                if (metaData.Attributes.Attributes!.FirstOrDefault(o => o.GetType() == typeof(DataTypeAttribute)) is not DataTypeAttribute dataType
                    || dataType.DataType == DataType.DateTime)
                {
                    result.TryAdd("input", "datetime");
                }
                else
                {
                    result.TryAdd("input", "date");
                }
                result.TryAdd("input", "date");
                result.TryAdd("format", "datetime");
            }
            else if (modelType == typeof(Guid))
            {
                result.Add("type", "string");
                result.Add("format", "guid");
            }
            else
            {
                result.Add("type", "number");
            }
        }
        else//引用类型
        {
            if (metaData.IsEnumerableType)//Guid
            {
                result.TryAdd("type", "array");
                //if(modelMetaData.ElementType!.UnderlyingSystemType==typeof(Guid))
                //{
                //    result.TryAdd("multiple", true);
                //}
                if (parent == null)//modelMetaData.ElementType!.UnderlyingSystemType.IsClass && modelMetaData.ElementType.UnderlyingSystemType != typeof(string))
                {
                    result.TryAdd("items", metaData.ElementMetadata!.GetSchema(serviceProvider, meta));
                }
            }
            else if (metaData.ModelType == typeof(string))
            {
                result.Add("type", "string");
            }
            else
            {
                result.Add("type", "object");
                var properties = new Dictionary<string, object>();
                var getOrder = (ModelMetadata o) =>
                {
                    var order = 0;
                    var meta = (o as DefaultModelMetadata)!;
                    var attribute = meta.Attributes.Attributes.FirstOrDefault(o => o.GetType() == typeof(DisplayOrderAttribute));
                    if (attribute != null && attribute is DisplayOrderAttribute displayOrder)
                    {
                        order = displayOrder.Order;
                    }
                    return order;
                };
                var ModelProperties = metaData.Properties.OrderBy(o => getOrder(o));
                foreach (var propertyMetadata in ModelProperties)
                {
                    if (meta.ContainerType == propertyMetadata.ContainerType)
                    {
                        continue;
                    }
                    properties.Add(propertyMetadata.Name!, propertyMetadata.GetSchema(serviceProvider, meta));
                }
                result.Add(nameof(properties), properties);
            }
        }
        //验证
        var validationProvider = serviceProvider.GetRequiredService<IValidationAttributeAdapterProvider>();
        var localizer = serviceProvider.GetRequiredService<IStringLocalizer>();
        var actionContext = new ActionContext { HttpContext = serviceProvider.GetRequiredService<IHttpContextAccessor>().HttpContext! };
        var provider = new EmptyModelMetadataProvider();
        var modelValidationContextBase = new ModelValidationContextBase(actionContext, meta, new EmptyModelMetadataProvider());
        var rules = new List<Dictionary<string, object>>();
        //必填
        if (metaData.IsRequired)
        {
            if (modelType != typeof(bool))
            {
                rules.Add(new Dictionary<string, object> { { "required", true } });
                result.Add("required", true);
            }
        }
        foreach (var item in metaData.Attributes.Attributes!)
        {
            if (item is ValidationAttribute attribute && !string.IsNullOrEmpty(attribute.ErrorMessage))
            {
                var errorMessage = localizer.GetString(attribute.ErrorMessage).Value;
                string? message;
                if (attribute is RemoteAttribute)
                {
                    message = string.Format(CultureInfo.InvariantCulture, errorMessage, title);
                }
                else if (attribute is DataTypeAttribute)
                {
                    if (attribute is FileExtensionsAttribute extensionsAttribute)
                    {
                        message = string.Format(CultureInfo.InvariantCulture, errorMessage, title, extensionsAttribute.Extensions);
                    }
                    else
                    {
                        message = string.Format(CultureInfo.InvariantCulture, errorMessage, title);
                    }
                }
                else
                {
                    message = validationProvider.GetAttributeAdapter(attribute!, localizer)?.GetErrorMessage(modelValidationContextBase);
                }
                var rule = new Dictionary<string, object>();
                if (attribute is RegularExpressionAttribute regularExpression)
                {
                    rule.Add("pattern", regularExpression.Pattern);
                }
                else if (attribute is MaxLengthAttribute maxLength)
                {
                    rule.Add("max", maxLength.Length);
                }
                else if (attribute is RequiredAttribute)
                {
                    //rule.Add("required", true);
                }
                else if (attribute is CompareAttribute compare)//??
                {
                    rule.Add("validator", "compare");
                    rule.Add("compare", compare.OtherProperty.ToLowerCamelCase());
                }
                else if (attribute is MinLengthAttribute minLength)
                {
                    rule.Add("min", minLength.Length);
                }
                else if (attribute is CreditCardAttribute)
                {
                    rule.Add("validator", "creditcard");
                }
                else if (attribute is StringLengthAttribute stringLength)
                {
                    rule.Add("min", stringLength.MinimumLength);
                    rule.Add("max", stringLength.MaximumLength);
                }
                else if (attribute is RangeAttribute range)
                {
                    rule.Add("type", "number");
                    rule.Add("min", range.Minimum is int minInt ? minInt : (double)range.Minimum);
                    rule.Add("max", range.Maximum is int maxInt ? maxInt : (double)range.Maximum);
                }
                else if (attribute is EmailAddressAttribute)
                {
                    rule.Add("type", "email");
                }
                else if (attribute is PhoneAttribute)
                {
                    rule.Add("validator", "phone");
                }
                else if (attribute is UrlAttribute)
                {
                    rule.Add("type", "url");
                }
                else if (attribute is FileExtensionsAttribute fileExtensions)
                {
                    rule.Add("validator", "accept");
                    rule.Add("extensions", fileExtensions.Extensions);
                }
                else if (attribute is RemoteAttribute remote)
                {
                    rule.Add("validator", "remote");
                    var attributes = new Dictionary<string, string>();
                    remote.AddValidation(new ClientModelValidationContext(actionContext, metaData, provider, attributes));
                    rule.Add("remote", attributes["data-val-remote-url"]);
                    //rule.Add("fields", remote.AdditionalFields.Split(',').Where(o => !string.IsNullOrEmpty(o)).Select(o => o.ToLowerCamelCase()).ToList());
                }
                else if (attribute is DataTypeAttribute dataType)
                {
                    var name = dataType.GetDataTypeName();
                    if (name == DataType.Date.ToString())
                    {
                        rule.TryAdd("input", "date");
                    }
                    else if (name == DataType.DateTime.ToString())
                    {
                        rule.TryAdd("input", "datetime");
                    }
                }
                else
                {
                    //Console.WriteLine($"{attribute.GetType().Name}");
                }
                if (rule.Count > 0)
                {
                    rule.Add("message", message!);
                }
                if (rule.Count > 0)
                {
                    rules.Add(rule);
                }
                //rule.Add("trigger", "change");
            }
        }
        if (rules.Any())
        {
            result.TryAdd(nameof(rules), rules);
        }
        return result;
    }
}
