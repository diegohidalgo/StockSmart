using System;
using System.IO;
using Asp.Versioning;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using MediatR.Extensions.Autofac.DependencyInjection;
using MediatR.Extensions.Autofac.DependencyInjection.Builder;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Serilog;
using StockSmart.Presentation.Extensions;
using StockSmart.Presentation.Filters;
using StockSmart.WebApp.Modules;

var builder = WebApplication.CreateBuilder(args);

//IConfiguration configuration = new ConfigurationBuilder()
//                        .SetBasePath(Directory.GetCurrentDirectory())
//                        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
//                        .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true)
//                        .AddJsonFile($"appsettings.{Environment.MachineName}.json", optional: true)
//                        .AddEnvironmentVariables()
//                        .Build();

IConfiguration configuration = builder.Configuration;

Log.Logger = new LoggerConfiguration()
                           .ReadFrom
                           .Configuration(configuration)
                           .CreateLogger();

builder.Logging.ClearProviders();
builder.Logging.AddSerilog();

builder.Host
    .UseServiceProviderFactory(new AutofacServiceProviderFactory())
    .ConfigureContainer<ContainerBuilder>((container) =>
    {
        container.RegisterModule(new ApiAutofacModule());
        container.RegisterModule(new EntityFrameworkAutofacModule(configuration));
        container.RegisterMediatR(MediatRConfigurationBuilder
                                        .Create(typeof(StockSmart.Application.AssemblyReference).Assembly)
                                        .WithAllOpenGenericHandlerTypesRegistered()
                                        .WithRegistrationScope(RegistrationScope.Scoped) // currently only supported values are `Transient` and `Scoped`
                                        .Build()
                                );
    });

var presentation = typeof(StockSmart.Presentation.AssemblyReference).Assembly;

builder.Services.AddControllers(options => options.Filters.Add<CustomExceptionFilter>()).AddApplicationPart(presentation);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "StockSmart", Version = "v1", Description = "A product stock application" });
    c.CustomSchemaIds(x => x.FullName);
    var xmlFilename = $"{typeof(StockSmart.Presentation.AssemblyReference).Assembly.GetName().Name}.xml";
    c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});
builder.Services.AddApiVersioning(options =>
{
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
    options.DefaultApiVersion = ApiVersion.Default;
    options.ApiVersionReader = new HeaderApiVersionReader("X-API-Version");
});
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = configuration.GetConnectionString("Redis");
    options.InstanceName = "StockSmart";
});

Audit.Core.Configuration
            .Setup()
            .UseFileLogProvider(config => config.Directory(Path.Combine(AppContext.BaseDirectory, "AuditLogs")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}

app.UseResponseTimeMiddleware();

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.Run();
