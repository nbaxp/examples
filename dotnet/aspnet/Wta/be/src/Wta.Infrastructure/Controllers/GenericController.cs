using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Wta.Application;
using Wta.Application.Identity.Domain;
using Wta.Infrastructure.Abstractions;
using Wta.Infrastructure.Application;
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
public class GenericController<TEntity, TModel>(ILogger<TEntity> logger, IRepository<TEntity> repository, IMapper<TEntity, TModel> mapper, IExportImportService exportImportService) : BaseController, IResourceService<TEntity>
    where TEntity : BaseEntity
    where TModel : class
{
    public ILogger<TEntity> Logger { get; } = logger;
    public IRepository<TEntity> Repository { get; } = repository;
    public IMapper<TEntity, TModel> Mapper { get; } = mapper;

    [Display(Order = -5)]
    public CustomApiResponse<QueryModel<TModel>> Search(QueryModel<TModel> model)
    {
        var query = this.Where(model);
        model.TotalCount = query.Count();
        query = this.OrderBy(query, model.OrderBy);
        if (!model.IncludeAll)
        {
            query = this.SkipTake(query, model.PageIndex, model.PageSize);
        }
        else
        {
            model.PageSize = model.TotalCount;
        }
        model.Items = Mapper.ToModelList(query);
        return Json(model);
    }

    [Display(Order = -2)]
    public FileContentResult Export(ExportModel<TModel> model)
    {
        var query = this.Where(model);
        query = this.OrderBy(query, model.OrderBy);
        if (!model.IncludeAll)
        {
            query = this.SkipTake(query, model.PageIndex, model.PageSize);
        }
        var items = Mapper.ToModelList(query);
        var contentType = WebApp.Instance.WebApplication.Services.GetRequiredService<FileExtensionContentTypeProvider>().Mappings[".xlsx"];
        var result = new FileContentResult(exportImportService.Export(items), contentType);
        result.FileDownloadName = $"{typeof(TModel).GetDisplayName()}.xlsx";
        return result;
    }

    [Button(Type = ButtonType.Row)]
    public CustomApiResponse<TModel> Details([FromBody] Guid id)
    {
        var model = Mapper.ToModelList(Repository.AsNoTracking().Where(o => o.Id == id)).FirstOrDefault();
        if (model == null)
        {
            throw new ProblemException("Not Found");
        }
        return Json(model);
    }

    [Display(Order = -4)]
    public CustomApiResponse<bool> Create(TModel model)
    {
        if (!ModelState.IsValid)
        {
            throw new BadRequestException();
        }
        var entity = Activator.CreateInstance<TEntity>();
        Mapper.FromModel(entity, model);
        Repository.Add(entity);
        Repository.SaveChanges();
        return Json(true);
    }

    [Display(Order = -3), Hidden]
    public FileContentResult ImportTemplate()
    {
        var contentType = WebApp.Instance.WebApplication.Services.GetRequiredService<FileExtensionContentTypeProvider>().Mappings[".xlsx"];
        var result = new FileContentResult(exportImportService.GetImportTemplate<TModel>(), contentType);
        result.FileDownloadName = $"{typeof(TModel).GetDisplayName()}.xlsx";
        return result;
    }

    [Display(Order = -3)]
    public CustomApiResponse<bool> Import(ImportModel<TModel> model)
    {
        foreach (var file in model.Files)
        {
            using var ms = new MemoryStream();
            file.OpenReadStream().CopyTo(ms);
            var models = exportImportService.Import<TModel>(ms.ToArray());
            foreach (var item in models)
            {
                this.Create(item);
            }
        }
        return Json(true);
    }

    [Display(Order = -1)]
    [Button(Type = ButtonType.Row)]
    public CustomApiResponse<bool> Update([FromBody] TModel model)
    {
        if (!ModelState.IsValid)
        {
            throw new BadRequestException();
        }
        var id = (Guid)(typeof(TModel).GetProperty("Id")!.GetValue(model)!);
        var entity = Repository.Query().FirstOrDefault(o => o.Id == id);
        if (entity == null)
        {
            throw new ProblemException("Not Found");
        }
        if (typeof(TEntity).IsAssignableTo(typeof(BaseTreeEntity<>).MakeGenericType(typeof(TEntity))))
        {
            //防止循环依赖
        }
        Mapper.FromModel(entity, model);
        Repository.SaveChanges();
        return Json(true);
    }

    [Display(Order = -1)]
    public CustomApiResponse<bool> Delete([FromBody] Guid[] items)
    {
        foreach (var id in items)
        {
            var entity = Repository.Query().FirstOrDefault(o => o.Id == id);
            if (entity != null)
            {
                Repository.Remove(entity);
            }
        }
        Repository.SaveChanges();
        return Json(true);
    }

    [AllowAnonymous, Hidden]
    public JsonResult Schema()
    {
        return new JsonResult(typeof(TModel).GetMetadataForType());
    }

    protected IQueryable<TEntity> Where(QueryModel<TModel> model)
    {
        var query = Repository.AsNoTracking();
        if (model.Query != null)
        {
            query = query.Where(model.Query);
        }
        if (model.Filters.Any())
        {
            var expression = QueryFilter.ToExpression<TEntity>(model.Filters);
            if (expression != null)
            {
                query = query.Where(expression);
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
}