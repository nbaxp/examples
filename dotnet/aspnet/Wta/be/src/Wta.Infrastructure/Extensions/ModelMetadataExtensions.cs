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
        var result = new Dictionary<string, object>() { { "isRoot", parent == null } };
        var modelMetaData = (meta as DefaultModelMetadata)!;
        if (modelMetaData.ModelType.IsValueType)
        {
            var isNullableType = modelMetaData.ModelType.IsNullableType();
            if (modelMetaData.ModelType.IsEnum)
            {
            }
            else
            {
                if (modelMetaData.ModelType == typeof(Guid))
                {
                }
                else
                {
                }
            }
        }
        else
        {
            if (modelMetaData.ModelType.IsArray)
            {
            }
            else if (modelMetaData.ModelType == typeof(string))
            {
            }
            else if (modelMetaData.ModelType.IsGenericType)
            {
                if (modelMetaData.ModelType.GetGenericTypeDefinition() == typeof(List<>))
                {
                    if (modelMetaData.ModelType.GetGenericArguments().First() == typeof(Guid))
                    {
                    }
                    else
                    {
                    }
                }
                else
                {
                }
            }
            else
            {
            }
        }
        var modelType = meta.UnderlyingOrModelType;

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
        var title = meta.ContainerType == null ? modelType.GetDisplayName() : meta.ContainerType?.GetProperty(meta.PropertyName!)?.GetDisplayName();
        result.Add("title", title!);
        modelMetaData.Attributes.Attributes?.ForEach(o =>
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
                else if (dataType.DataType == DataType.Date)
                {
                    result.TryAdd("input", "date");
                }
                else if (dataType.DataType == DataType.DateTime)
                {
                    result.TryAdd("input", "datetime");
                }
            }
        });
        var roles = meta.GetRules(serviceProvider, title!);
        if (roles.Any())
        {
            result.Add("rules", roles);
        }
        // array
        if (meta.IsEnumerableType)
        {
            if (modelType != meta.ElementMetadata!.ModelType.UnderlyingSystemType)
            {
                result.Add("type", "array");
                result.TryAdd("multiple", true);
                result.Add("items", meta.ElementMetadata.GetSchema(serviceProvider, meta));
            }
        }
        else
        {
            if (!modelType.IsValueType && modelType != typeof(string))
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
                var ModelProperties = modelMetaData.Properties.OrderBy(o => getOrder(o));
                foreach (var propertyMetadata in ModelProperties)
                {
                    if (meta.ContainerType != propertyMetadata.ContainerType)
                    {
                        if (propertyMetadata.IsEnumerableType)
                        {
                            //array
                            if (propertyMetadata.ElementType == propertyMetadata.ContainerType)
                            {
                                continue;
                            }
                        }
                        else if (!propertyMetadata.ModelType.IsValueType && propertyMetadata.ModelType != typeof(string))
                        {
                            //object
                            if (propertyMetadata.ModelType == propertyMetadata.ContainerType)
                            {
                                continue;
                            }
                            if (parent != null)
                            {
                                continue;
                            }
                        }
                        else
                        {
                            //property
                        }
                        properties.Add(propertyMetadata.Name!, propertyMetadata.GetSchema(serviceProvider, meta));
                    }
                }
                result.Add(nameof(properties), properties);
            }
            else
            {
                if (modelType.IsEnum)
                {
                    result.TryAdd("options", Enum.GetNames(modelType).Select(o => new { Value = o, Label = ((Enum)Enum.Parse(modelType, o)).GetDisplayName() }).ToArray());
                    result.TryAdd("input", "select");
                    if (modelType.HasAttribute<FlagsAttribute>())
                    {
                        result.TryAdd("multiple", true);
                    }
                }
                AddType(result, modelType);
                if (meta.ModelType.IsNullableType())
                {
                    result.Add("nullable", true);
                }
            }
        }
        if (meta.Description != null)
        {
            result.Add("description", meta.Description);
        }
        if (meta.DataTypeName != null)
        {
            result.Add("format", meta.DataTypeName?.ToLowerCamelCase()!);
        }
        if (meta.TemplateHint != null)
        {
            result.Add("input", meta.TemplateHint?.ToLowerCamelCase()!);
        }
        if (meta.TemplateHint == "select" && meta.IsEnumerableType && modelType.IsGenericType)
        {
            result.TryAdd("url", modelType.GetGenericArguments().First().Name.ToSlugify()!);
        }
        if (modelMetaData.Name == "Id" || modelMetaData.Attributes.Attributes!.Any(o => o.GetType() == typeof(HiddenAttribute)))
        {
            result.TryAdd("hidden", true);
        }
        var propertyName = modelMetaData.Name;
        if (propertyName != null)
        {
            if (modelMetaData.Attributes.Attributes!.FirstOrDefault(o => o.GetType() == typeof(DefaultValueAttribute)) is DefaultValueAttribute defaultValue)
            {
                result.Add("default", defaultValue.Value!);
            }
            //if (defaultModelMetadata.Attributes.Attributes.FirstOrDefault(o => o.GetType() == typeof(NavigationAttribute)) is NavigationAttribute navigationAttribute)
            //{
            //    var path = navigationAttribute.Property ?? $"{propertyName[..^2]}.Name";
            //    path = string.Join('.', path.Split('.').Select(o => o.ToLowerCamelCase()));
            //    schema.Add("navigation", path);
            //    schema.Add("input", "select");
            //    schema.Add("url", propertyName[..^2].ToSlugify());
            //}
            if (modelMetaData.Attributes.Attributes!.FirstOrDefault(o => o.GetType() == typeof(ScaffoldColumnAttribute)) is ScaffoldColumnAttribute scaffoldColumnAttribute
                && !scaffoldColumnAttribute.Scaffold)
            {
                //列表、详情、新建、更新、查询都不显示
                result.Add("hidden", true);
            }
            if (modelMetaData.Attributes.Attributes!.FirstOrDefault(o => o.GetType() == typeof(ReadOnlyAttribute)) is ReadOnlyAttribute readOnlyAttribute
                && readOnlyAttribute.IsReadOnly)
            {
                //列表、详情显示，编辑时不显示，查询时显示
                result.Add("readOnly", true);
            }
            //if (defaultModelMetadata.Attributes.Attributes.Any(o => o.GetType() == typeof(DisplayOnlyAttribute)))
            //{
            //    //列表、详情、编辑时都只显示，查询时显示
            //    schema.Add("displayOnly", true);
            //}
        }
        return result;
    }

    public static List<Dictionary<string, object>> GetRules(this ModelMetadata meta, IServiceProvider serviceProvider, string title)
    {
        var pm = (meta as DefaultModelMetadata)!;
        var rules = new List<Dictionary<string, object>>();
        var validationProvider = serviceProvider.GetRequiredService<IValidationAttributeAdapterProvider>();
        var localizer = serviceProvider.GetRequiredService<IStringLocalizer>();
        var actionContext = new ActionContext { HttpContext = serviceProvider.GetRequiredService<IHttpContextAccessor>().HttpContext! };
        var provider = new EmptyModelMetadataProvider();
        var modelValidationContextBase = new ModelValidationContextBase(actionContext, meta, new EmptyModelMetadataProvider());
        if (pm.IsRequired &&
            !pm.IsNullableValueType &&
            !pm.UnderlyingOrModelType.IsValueType &&
            !pm.IsEnumerableType &&
            !pm.Attributes.Attributes.Any(o => o.GetType() == typeof(RequiredAttribute))
            )
        {
            var message = string.Format(CultureInfo.InvariantCulture, localizer.GetString(nameof(RequiredAttribute)).Value, title);
            rules.Add(new Dictionary<string, object> { { "required", true }, { "message", message } });
        }
        //if (pm.IsRequired)
        //{
        //    if (!pm.ModelType.IsValueType && pm.Attributes.Attributes.Any(o => o.GetType() == typeof(RequiredAttribute)))
        //    {
        //        var message = string.Format(CultureInfo.InvariantCulture, localizer.GetString(nameof(RequiredAttribute)).Value, title);
        //        rules.Add(new Dictionary<string, object> { { "required", true }, { "message", message } });
        //    }
        //}
        foreach (var item in pm.Attributes.Attributes)
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
                    rule.Add("required", true);
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
                    remote.AddValidation(new ClientModelValidationContext(actionContext, pm, provider, attributes));
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
            else
            {
                //Console.WriteLine($"{item.GetType().Name}");
            }
        }
        return rules;
    }

    private static void AddType(Dictionary<string, object> schema, Type modelType)
    {
        var type = "string";
        if (modelType == typeof(bool))
        {
            type = "boolean";
        }
        else if (modelType == typeof(short) ||
            modelType == typeof(int) ||
            modelType == typeof(long))
        {
            type = "integer";
        }
        else if (
            modelType == typeof(float) ||
            modelType == typeof(double) ||
            modelType == typeof(decimal))
        {
            type = "number";
        }
        schema.Add("type", type);
        if (modelType.GetUnderlyingType() == typeof(DateTime))
        {
            schema.TryAdd("input", "datetime");
        }
    }
}
