global using System.ComponentModel;
global using System.ComponentModel.DataAnnotations;
global using System.Globalization;
global using System.IdentityModel.Tokens.Jwt;
global using System.Reflection;
global using System.Security.Claims;
global using System.Text;
global using System.Text.Json.Serialization;
global using System.Text.RegularExpressions;

//
global using Microsoft.AspNetCore.Authentication.JwtBearer;
global using Microsoft.AspNetCore.Authorization;
global using Microsoft.AspNetCore.Builder;
global using Microsoft.AspNetCore.Http;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.AspNetCore.Mvc.ActionConstraints;
global using Microsoft.AspNetCore.Mvc.Controllers;
global using Microsoft.AspNetCore.Mvc.Infrastructure;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.EntityFrameworkCore.Metadata.Builders;
global using Microsoft.Extensions.Configuration;
global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.Extensions.Hosting;
global using Microsoft.Extensions.Localization;
global using Microsoft.Extensions.Logging;
global using Microsoft.Extensions.Options;
global using Microsoft.IdentityModel.Tokens;

//
global using Wta.Application.Platform.Domain;
global using Wta.Application.Platform.Models;
global using Wta.Infrastructure.Application.Configuration;
global using Wta.Infrastructure.Application.Domain;
global using Wta.Infrastructure.Attributes;
global using Wta.Infrastructure.Auth;
global using Wta.Infrastructure.Controllers;
global using Wta.Infrastructure.Data;
global using Wta.Infrastructure.Event;
global using Wta.Infrastructure.Exceptions;
global using Wta.Infrastructure.Extensions;
global using Wta.Infrastructure.ImportExport;
global using Wta.Infrastructure.Security;
global using Wta.Infrastructure.Tenant;
global using Wta.Infrastructure.Web;
