global using System.ComponentModel;
global using System.ComponentModel.DataAnnotations;
global using System.Globalization;
global using System.IdentityModel.Tokens.Jwt;
global using System.Reflection;
global using System.Security.Claims;
global using System.Text;
global using System.Text.Encodings.Web;
global using System.Text.Json;
global using System.Text.Json.Serialization;
global using System.Text.Unicode;
global using Microsoft.AspNetCore.Authentication.JwtBearer;

//
global using Microsoft.AspNetCore.Authorization;
global using Microsoft.AspNetCore.Builder;
global using Microsoft.AspNetCore.Http.Features;
global using Microsoft.AspNetCore.Http.Json;
global using Microsoft.AspNetCore.Localization;
global using Microsoft.AspNetCore.Mvc.ApiExplorer;
global using Microsoft.AspNetCore.Mvc.ApplicationModels;
global using Microsoft.AspNetCore.Mvc.ModelBinding;
global using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
global using Microsoft.AspNetCore.Mvc.Razor;
global using Microsoft.AspNetCore.StaticFiles;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.Extensions.Configuration;
global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.Extensions.DependencyInjection.Extensions;
global using Microsoft.Extensions.FileProviders;
global using Microsoft.Extensions.Hosting;
global using Microsoft.Extensions.Localization;
global using Microsoft.Extensions.Logging;
global using Microsoft.Extensions.Options;
global using Microsoft.IdentityModel.JsonWebTokens;
global using Microsoft.IdentityModel.Tokens;
global using Microsoft.OpenApi.Models;

//
global using Wta.Infrastructure.Application.Configuration;
global using Wta.Infrastructure.Attributes;
global using Wta.Infrastructure.Controllers;
global using Wta.Infrastructure.Data;
global using Wta.Infrastructure.Event;
global using Wta.Infrastructure.Extensions;
global using Wta.Infrastructure.Resources;
global using Wta.Infrastructure.SignalR;
global using Wta.Infrastructure.Web;
