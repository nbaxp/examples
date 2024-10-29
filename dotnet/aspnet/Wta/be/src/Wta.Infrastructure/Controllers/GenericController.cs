using ClosedXML;
using Microsoft.AspNetCore.Mvc;
using Wta.Infrastructure.Application.Domain;
using Wta.Infrastructure.Application.Models;
using Wta.Infrastructure.Auth;
using Wta.Infrastructure.Exceptions;
using Wta.Infrastructure.ImportExport;
using Wta.Infrastructure.Mapper;

namespace Wta.Infrastructure.Controllers;

[GenericControllerNameConvention]
public class GenericController<TEntity, TModel>(ILogger<TEntity> logger,
    IStringLocalizer stringLocalizer,
    IObjerctMapper objectMapper,
    IRepository<TEntity> repository,
    IEventPublisher eventPublisher,
    IExportImportService exportImportService) : BaseController, IResourceService<TEntity>
    where TEntity : BaseEntity
    where TModel : class
{
    public ILogger<TEntity> Logger { get; } = logger;
    public IStringLocalizer StringLocalizer = stringLocalizer;
    public IObjerctMapper ObjectMapper { get; } = objectMapper;
    public IRepository<TEntity> Repository { get; } = repository;
    public IEventPublisher EventPublisher = eventPublisher;

    [HttpGet, AllowAnonymous, Ignore]
    public virtual ApiResult<object> Schema()
    {
        return Json(typeof(TModel).GetMetadataForType());
    }

    [Display(Name = "查询", Order = 1)]
    [Hidden]
    public virtual ApiResult<QueryModel<TModel>> Search(QueryModel<TModel> model)
    {
        model ??= new QueryModel<TModel>();
        if (model.Filters.Any(o => o.Property == "IsDeleted".ToLowerCamelCase() && o.Value != null && o.Value is JsonElement jsonElement && jsonElement.GetBoolean()))
        {
            repository.DisableSoftDeleteFilter();
        }
        var query = Where(model);
        model.TotalCount = query.Count();
        query = OrderBy(query, model.OrderBy);
        if (model.IncludeAll)
        {
            model.PageIndex = 1;
            model.PageSize = query.Count();
        }
        else
        {
            query = SkipTake(query, model.PageIndex, model.PageSize);
        }
        model.Items = query.ToList().Select(o =>
        {
            var model = ObjectMapper.ToModel<TEntity, TModel>(o);
            ToModel(o, model);
            return model;
        }).ToList();
        return Json(model);
    }

    [Display(Name = "详情", Order = 2)]
    [Button(Type = ButtonType.Row)]
    public virtual ApiResult<TModel> Details([FromBody] Guid id)
    {
        var entity = Include(Repository.AsNoTracking()).FirstOrDefault(o => o.Id == id) ?? throw new ProblemException("NotFound");
        var model = ObjectMapper.ToModel<TEntity, TModel>(entity);
        this.ToModel(entity, model);
        return Json(model);
    }

    [Display(Name = "导入模板", Order = 3)]
    [Hidden]
    public virtual FileContentResult ImportTemplate()
    {
        var contentType = Global.Application.Services.GetRequiredService<FileExtensionContentTypeProvider>().Mappings[".xlsx"];
        var result = new FileContentResult(exportImportService.GetImportTemplate<TModel>(), contentType)
        {
            FileDownloadName = $"{typeof(TModel).GetDisplayName()}.xlsx"
        };
        return result;
    }

    [Display(Name = "新建", Order = 4)]
    public virtual ApiResult<bool> Create([FromBody] TModel model)
    {
        if (!ModelState.IsValid)
        {
            throw new BadRequestException();
        }
        var entity = ObjectMapper.ToEntity<TEntity, TModel>(model);
        this.ToEntity(entity, model, true);
        entity.Id = Repository.NewGuid();
        if (entity is BaseTreeEntity<TEntity> node)
        {
            node.Parent = node.ParentId.HasValue ? Repository.Query().FirstOrDefault(o => o.Id == node.ParentId.Value) : null;
            node.UpdateNode();
        }
        Repository.Add(entity);
        EventPublisher.Publish(new EntityCreatedEvent<TEntity>(entity));
        Repository.SaveChanges();
        return Json(true);
    }

    [Consumes("multipart/form-data")]
    [Display(Name = "导入", Order = 5)]
    public virtual ApiResult<bool> Import([FromForm] ImportModel<TModel> model)
    {
        using var ms = new MemoryStream();
        model.File.OpenReadStream().CopyTo(ms);
        var models = exportImportService.Import<TModel>(ms.ToArray());
        foreach (var item in models)
        {
            Create(item);
        }
        return Json(true);
    }

    [Display(Name = "导出", Order = 6)]
    public virtual FileContentResult Export(ExportModel<TModel> model)
    {
        var query = Where(model);
        query = OrderBy(query, model.OrderBy);
        if (!model.IncludeAll)
        {
            query = SkipTake(query, model.PageIndex, model.PageSize);
        }
        var items = query.ToList().Select(o => { var model = ObjectMapper.ToModel<TEntity, TModel>(o); this.ToModel(o, model); return model; }).ToList();
        var contentType = Global.Application.Services.GetRequiredService<FileExtensionContentTypeProvider>().Mappings[$".{model.Format}"];
        var result = new FileContentResult(exportImportService.Export(items), contentType)
        {
            FileDownloadName = (model.Name ?? $"{typeof(TModel).GetDisplayName()}_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}") + "." + model.Format
        };
        return result;
    }

    [Display(Name = "更新", Order = 7)]
    [Button(Type = ButtonType.Row)]
    public virtual ApiResult<bool> Update([FromBody] TModel model)
    {
        typeof(TModel).GetProperties(BindingFlags.Public | BindingFlags.GetProperty | BindingFlags.Instance)
            .Where(o => (o.PropertyType.IsGenericType && o.PropertyType.GetGenericTypeDefinition() == typeof(List<>)) || o.GetAttributes<IgnoreToModelAttribute>().Any())
            .ForEach(o => ModelState.Remove(o.Name));
        if (!ModelState.IsValid)
        {
            throw new BadRequestException();
        }
        var id = (Guid)typeof(TModel).GetProperty("Id")!.GetValue(model)!;
        var entity = Include(Repository.Query()).First(o => o.Id == id);
        ObjectMapper.FromModel(entity, model);
        this.ToEntity(entity, model);
        if (entity is BaseTreeEntity<TEntity> node)
        {
            var parentId = typeof(TModel).GetProperty(nameof(node.ParentId))?.GetValue(model) as Guid?;
            if (node.ParentId != null)
            {
                //防止循环依赖
                if (parentId != null)
                {
                    var current = node;
                    while (current.ParentId.HasValue)
                    {
                        if (current.ParentId.Value == node.Id)
                        {
                            ModelState.AddModelError(nameof(node.ParentId), StringLocalizer.GetString("CircularReferenceError"));
                            throw new BadRequestException();
                        }
                        current = Repository.AsNoTracking().Cast<BaseTreeEntity<TEntity>>().FirstOrDefault(o => o.Id == current.ParentId);
                        if (current == null)
                        {
                            break;
                        }
                    }
                }
            }
            //更新节点和子节点路径
            var prefix = node.Path + "/";
            node.Parent = node.ParentId.HasValue ? Repository.Query().FirstOrDefault(o => o.Id == node.ParentId) : null;
            node.UpdateNode();
            var children = Repository.Query().Cast<BaseTreeEntity<TEntity>>().Where(o => o.Path.StartsWith(prefix)).ToList();
            children.ToTree();
            children.Where(o => o.ParentId == node.Id).ForEach(o => o.UpdateNode());
        }
        //EventPublisher.Publish(new EntityUpdatedEvent<TEntity>(entity));
        Repository.SaveChanges();
        return Json(true);
    }

    [Display(Name = "归档", Order = 8)]
    public virtual ApiResult<bool> Archive([FromBody] Guid[] items)
    {
        foreach (var id in items)
        {
            var entity = Repository.Query().FirstOrDefault(o => o.Id == id);
            if (entity != null)
            {
                entity.IsDeleted = true;
                Repository.SaveChanges();
            }
        }
        return Json(true);
    }

    [Display(Name = "解档", Order = 9)]
    public virtual ApiResult<bool> Unarchive([FromBody] Guid[] items)
    {
        Repository.DisableSoftDeleteFilter();
        foreach (var id in items)
        {
            var entity = Repository.Query().FirstOrDefault(o => o.Id == id);
            if (entity != null)
            {
                entity.IsDeleted = false;
                Repository.SaveChanges();
            }
        }
        return Json(true);
    }

    [Display(Name = "删除", Order = 10)]
    public virtual ApiResult<bool> Delete([FromBody] Guid[] items)
    {
        Repository.DisableSoftDeleteFilter();
        foreach (var id in items)
        {
            var entity = Repository.Query().FirstOrDefault(o => o.Id == id);
            if (entity != null)
            {
                if (!entity.IsDeleted)
                {
                    throw new ProblemException("无法删除未归档的数据");
                }
                Repository.Remove(entity);
                if (typeof(TEntity).IsAssignableTo(typeof(BaseTreeEntity<TEntity>)))
                {
                    var prefix = (entity as BaseTreeEntity<TEntity>)!.Path + "/";
                    var list = Repository.Query().Cast<BaseTreeEntity<TEntity>>().Where(o => o.Path.StartsWith(prefix)).ToList();
                    list.ToTree();
                    list.Where(o => o.ParentId == entity.Id).ForEach(o =>
                    {
                        o.ParentId = null;
                        o.Parent = null;
                        o.UpdateNode();
                    });
                }
                EventPublisher.Publish(new EntityDeletedEvent<TEntity>(entity));
                Repository.SaveChanges();
            }
        }
        return Json(true);
    }

    [Authorize, Ignore]
    public List<TModel> Parents(string number)
    {
        var suffix = Repository.AsNoTracking().Cast<BaseTreeEntity<TEntity>>().Where(o => o.Number == number).Select(o => o.Path).FirstOrDefault();
        if (!string.IsNullOrEmpty(suffix))
        {
            var result = Repository.AsNoTracking().Cast<BaseTreeEntity<TEntity>>()
                .Where(o => suffix.StartsWith(o.Path))
                .ToList()
                .Cast<TEntity>()
                .Select(o =>
                {
                    var model = ObjectMapper.ToModel<TEntity, TModel>(o);
                    this.ToModel(o, model);
                    return model;
                }).ToList();
            return result;
        }
        throw new ProblemException("NotFound");
    }

    [Authorize, Ignore]
    public List<TModel> Children(string number)
    {
        var prefix = Repository.AsNoTracking().Cast<BaseTreeEntity<TEntity>>().Where(o => o.Number == number).Select(o => o.Path).FirstOrDefault();
        if (!string.IsNullOrEmpty(prefix))
        {
            var result = Repository.AsNoTracking().Cast<BaseTreeEntity<TEntity>>()
                .Where(o => o.Path.StartsWith(prefix))
                .ToList()
                .Cast<TEntity>()
                .Select(o => { var model = ObjectMapper.ToModel<TEntity, TModel>(o); ToModel(o, model); return model; }).ToList();
            return result;
        }
        throw new ProblemException("NotFound");
    }

    protected virtual IQueryable<TEntity> Where(QueryModel<TModel> model)
    {
        var query = Repository.AsNoTracking();
        if (model != null)
        {
            if (model.Filters.Count != 0)
            {
                var expression = QueryFilter.ToExpression<TEntity>(model.Filters);
                if (expression != null)
                {
                    query = query.Where(expression);
                }
            }
        }

        return query;
    }

    protected IQueryable<TEntity> OrderBy(IQueryable<TEntity> query, string? orderBy)
    {
        if (!string.IsNullOrEmpty(orderBy))
        {
            query = query.OrderBy(orderBy);
        }
        else if (typeof(TEntity).IsAssignableTo(typeof(IOrdered)))
        {
            query = query.OrderBy(nameof(IOrdered.Order));
        }
        else
        {
            query = query.OrderByDescending(o => o.CreatedOn);
        }
        return query;
    }

    protected IQueryable<TEntity> SkipTake(IQueryable<TEntity> query, int pageIndex, int pageSize)
    {
        return query.Skip((pageIndex - 1) * pageSize).Take(pageSize);
    }

    protected virtual void ToModel(TEntity entity, TModel model)
    {
    }

    protected virtual void ToEntity(TEntity entity, TModel model, bool isCreate = false)
    {
    }

    protected virtual IQueryable<TEntity> Include(IQueryable<TEntity> queryable)
    {
        return queryable;
    }
}
