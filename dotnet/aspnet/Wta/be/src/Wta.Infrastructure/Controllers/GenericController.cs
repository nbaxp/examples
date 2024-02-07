using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Wta.Application;
using Wta.Application.Identity.Domain;
using Wta.Infrastructure.Attributes;
using Wta.Infrastructure.Domain;
using Wta.Infrastructure.Exceptions;
using Wta.Infrastructure.Extensions;
using Wta.Infrastructure.Interfaces;
using Wta.Infrastructure.Models;
using Wta.Infrastructure.Web;
using Wta.Shared;

namespace Wta.Infrastructure.Controllers;

[GenericControllerNameConvention]
public class GenericController<TEntity, TModel>(ILogger<TEntity> logger,
    IStringLocalizer stringLocalizer,
    IRepository<TEntity> repository,
    IExportImportService exportImportService) : BaseController, IResourceService<TEntity>
    where TEntity : BaseEntity
    where TModel : class
{
    public ILogger<TEntity> Logger { get; } = logger;
    public IStringLocalizer StringLocalizer = stringLocalizer;
    public IRepository<TEntity> Repository { get; } = repository;

    [Display(Order = 1)]
    public virtual CustomApiResponse<QueryModel<TModel>> Search(QueryModel<TModel> model)
    {
        var query = Where(model);
        model.TotalCount = query.Count();
        query = OrderBy(query, model.OrderBy);
        if (!model.IncludeAll)
        {
            query = SkipTake(query, model.PageIndex, model.PageSize);
        }
        else
        {
            model.PageSize = model.TotalCount;
        }
        model.Items = query.ToModelList<TEntity, TModel>();
        model.Items.ForEach(this.ToModel);
        return Json(model);
    }

    [Display(Order = 2)]
    [Button(Type = ButtonType.Row)]
    public virtual CustomApiResponse<TModel> Details([FromBody] Guid id)
    {
        var query = Repository.AsNoTracking().Where(o => o.Id == id);
        var model = query.ToModel<TEntity, TModel>();
        if (model == null)
        {
            throw new ProblemException("NotFound");
        }
        this.ToModel(model);
        return Json(model);
    }

    [Display(Order = 3), Hidden]
    public virtual FileContentResult ImportTemplate()
    {
        var contentType = WebApp.Instance.WebApplication.Services.GetRequiredService<FileExtensionContentTypeProvider>().Mappings[".xlsx"];
        var result = new FileContentResult(exportImportService.GetImportTemplate<TModel>(), contentType);
        result.FileDownloadName = $"{typeof(TModel).GetDisplayName()}.xlsx";
        return result;
    }

    [Display(Order = 4)]
    public virtual CustomApiResponse<bool> Import(ImportModel<TModel> model)
    {
        foreach (var file in model.Files)
        {
            using var ms = new MemoryStream();
            file.OpenReadStream().CopyTo(ms);
            var models = exportImportService.Import<TModel>(ms.ToArray());
            foreach (var item in models)
            {
                Create(item);
            }
        }
        return Json(true);
    }

    [Display(Order = 5)]
    public virtual FileContentResult Export(ExportModel<TModel> model)
    {
        var query = Where(model);
        query = OrderBy(query, model.OrderBy);
        if (!model.IncludeAll)
        {
            query = SkipTake(query, model.PageIndex, model.PageSize);
        }
        var items = query.ToModelList<TEntity, TModel>();
        items.ForEach(this.ToModel);
        var contentType = WebApp.Instance.WebApplication.Services.GetRequiredService<FileExtensionContentTypeProvider>().Mappings[".xlsx"];
        var result = new FileContentResult(exportImportService.Export(items), contentType);
        result.FileDownloadName = $"{typeof(TModel).GetDisplayName()}.xlsx";
        return result;
    }

    [Display(Order = 6)]
    public virtual CustomApiResponse<bool> Create(TModel model)
    {
        if (!ModelState.IsValid)
        {
            throw new BadRequestException();
        }
        var entity = Activator.CreateInstance<TEntity>();
        entity.FromModel(model, o => o.Ignore(o => o.Id));
        if (entity is BaseTreeEntity<TEntity> node)
        {
            node.Parent = node.ParentId.HasValue ? Repository.Query().FirstOrDefault(o => o.Id == node.ParentId.Value) : null;
            node.UpdateNode();
        }
        ToEntity(entity, model, true);
        Repository.Add(entity);
        Repository.SaveChanges();
        return Json(true);
    }

    [Display(Order = 7)]
    [Button(Type = ButtonType.Row)]
    public virtual CustomApiResponse<bool> Update([FromBody] TModel model)
    {
        if (!ModelState.IsValid)
        {
            throw new BadRequestException();
        }
        var id = (Guid)typeof(TModel).GetProperty("Id")!.GetValue(model)!;
        var entity = Repository.Query().First(o => o.Id == id);
        if (entity is BaseTreeEntity<TEntity> node)
        {
            node.FromModel(model, o => o.Ignore(o => o.Id).IgnoreAttribute(typeof(ReadOnlyAttribute)));
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
        else
        {
            entity.FromModel(model, o => o.IgnoreAttribute(typeof(ReadOnlyAttribute)));
        }
        ToEntity(entity, model);
        Repository.SaveChanges();
        return Json(true);
    }

    [Display(Order = 8)]
    public virtual CustomApiResponse<bool> Delete([FromBody] Guid[] items)
    {
        foreach (var id in items)
        {
            var entity = Repository.Query().FirstOrDefault(o => o.Id == id);
            if (entity != null)
            {
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
                Repository.SaveChanges();
            }
        }
        return Json(true);
    }

    [Ignore]
    public List<TModel> Parents(string number)
    {
        var suffix = Repository.AsNoTracking().Cast<BaseTreeEntity<TEntity>>().Where(o => o.Number == number).Select(o => o.Path).FirstOrDefault();
        if (!string.IsNullOrEmpty(suffix))
        {
            var result = Repository.AsNoTracking().Cast<BaseTreeEntity<TEntity>>()
                .Where(o => suffix.StartsWith(o.Path))
                .ToModelList<BaseTreeEntity<TEntity>, TModel>();
            //result.ForEach(this.ToModel);
            return result;
        }
        throw new ProblemException("NotFound");
    }

    [Ignore]
    public List<TModel> Children(string number)
    {
        var prefix = Repository.AsNoTracking().Cast<BaseTreeEntity<TEntity>>().Where(o => o.Number == number).Select(o => o.Path).FirstOrDefault();
        if (!string.IsNullOrEmpty(prefix))
        {
            var result = Repository.AsNoTracking().Cast<BaseTreeEntity<TEntity>>()
                .Where(o => o.Path.StartsWith(prefix))
                .ToModelList<BaseTreeEntity<TEntity>, TModel>();
            //result.ForEach(this.ToModel);
            return result;
        }
        throw new ProblemException("NotFound");
    }

    [AllowAnonymous, Ignore]
    public JsonResult Schema()
    {
        return new JsonResult(typeof(TModel).GetMetadataForType());
    }

    protected IQueryable<TEntity> Where(QueryModel<TModel> model)
    {
        var query = Repository.AsNoTracking();

        if (model != null)
        {
            if (model.Query != null)
            {
                query = query.WhereByModel(model.Query);
            }
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
        return query;
    }

    protected IQueryable<TEntity> SkipTake(IQueryable<TEntity> query, int pageIndex, int pageSize)
    {
        return query.Skip((pageIndex - 1) * pageSize).Take(pageSize);
    }

    protected virtual void ToModel(TModel model)
    {
    }

    protected virtual void ToEntity(TEntity entity, TModel model, bool isCreate = false)
    {
    }
}
