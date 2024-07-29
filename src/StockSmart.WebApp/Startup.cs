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
using Microsoft.OpenApi.Models;
using StockSmart.Presentation.Extensions;
using StockSmart.Presentation.Filters;
using StockSmart.WebApp.Modules;

namespace StockSmart.WebApp
{

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public ILifetimeScope AutofacContainer { get; private set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var presentation = typeof(Presentation.AssemblyReference).Assembly;
            services
                .AddControllers(options => options.Filters.Add<CustomExceptionFilter>())
                .AddApplicationPart(presentation);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "StockSmart", Version = "v1", Description = "A product stock application" });
                c.CustomSchemaIds(x => x.FullName);
                var xmlFilename = $"{typeof(Presentation.AssemblyReference).Assembly.GetName().Name}.xml";
                c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
            });
            services.AddApiVersioning(options =>
            {
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ReportApiVersions = true;
                options.DefaultApiVersion = ApiVersion.Default;
                options.ApiVersionReader = new HeaderApiVersionReader("X-API-Version");
            });
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = this.Configuration.GetConnectionString("Redis");
                options.InstanceName = "StockSmart";
            });
            services.AddHttpClient();

            Audit.Core.Configuration.Setup()
           .UseFileLogProvider(config =>
            {
                config.Directory(Path.Combine(AppContext.BaseDirectory, "AuditLogs")); // Specify your folder path here
            });
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterMediatR(MediatRConfigurationBuilder
                                            .Create(typeof(Application.AssemblyReference).Assembly)
                                            .WithAllOpenGenericHandlerTypesRegistered()
                                            .WithRegistrationScope(RegistrationScope.Scoped) // currently only supported values are `Transient` and `Scoped`
                                            .Build()
                                    );
            builder.RegisterModule(new ApiAutofacModule());
            builder.RegisterModule(new EntityFrameworkAutofacModule(this.Configuration));

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            this.AutofacContainer = app.ApplicationServices.GetAutofacRoot();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseResponseTimeMiddleware();

            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
